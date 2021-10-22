using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Character : Entity
    {
        public Enum.CharacterType CharacterType;

        public RuntimeAnimatorController m_AnimatorController;
        private Animator m_Animator;

        //private Collider m_Collider;

        [Range(0.1f, 10f)] public float m_ScaleForce = 0.1f;

        private GameObject CharacterModels;

        protected override void Awake()
        {
            base.Awake();
            m_Rigibody.useGravity = false;
            m_Rigibody.constraints = RigidbodyConstraints.FreezeAll;

            m_Animator = this.GetComponent<Animator>();
            switch(CharacterType)
            {
                case Enum.CharacterType.Animal:
                    m_Animator.SetBool("Eat", true);
                    break;
                case Enum.CharacterType.Human:
                    m_Animator.enabled = false;
                    CharacterModels = Instantiate(Game.Shop.Instance.GetSkinRuntime(), transform);
                    CharacterModels.transform.localPosition = Vector3.zero;
                    CharacterModels.transform.localRotation = Quaternion.identity;
                    SetGameLayerRecursive(CharacterModels, gameObject.layer);
                    m_Animator = CharacterModels.GetComponent<Animator>();
                    m_Animator.runtimeAnimatorController = m_AnimatorController;
                    break;
            }
        }

        public void ReloadCharacter()
        {
            switch (CharacterType)
            {
                case Enum.CharacterType.Human:
                    CharacterModels = Instantiate(Game.Shop.Instance.GetSkinRuntime(), transform);
                    CharacterModels.transform.localPosition = Vector3.zero;
                    CharacterModels.transform.localRotation = Quaternion.identity;
                    SetGameLayerRecursive(CharacterModels, gameObject.layer);
                    break;
            }
        }

        private void SetGameLayerRecursive(GameObject _go, int _layer)
        {
            _go.layer = _layer;
            foreach (Transform child in _go.transform)
            {
                child.gameObject.layer = _layer;

                Transform _HasChildren = child.GetComponentInChildren<Transform>();
                if (_HasChildren != null)
                    SetGameLayerRecursive(child.gameObject, _layer);

            }
        }

        private string[] detechedCollisions = new string[]
            {
                "Plane",
                "rock",
                "wall"
            };

        private void OnCollisionEnter(Collision col)
        {
            if(col.collider.gameObject.layer != LayerMask.NameToLayer("Bullet"))
            {
                for (int i = 0; i < detechedCollisions.Length; i++)
                {
                    if (col.gameObject.name.Contains(detechedCollisions[i]))
                        return;
                }
            }

            if (Game.LevelManager.Instance.IsVictory)
                return;
            Rigidbody colRig = col.gameObject.GetComponent<Rigidbody>();
            if (colRig == null)
                return;

            direction = Physics.gravity + colRig.velocity;
            Game.LevelManager.Instance.OnLose();
            ShowDead();
        }

        private void FixedUpdate()
        {
            if(IsForce)
            {
                m_Rigibody.MovePosition(transform.position + direction.normalized * m_ScaleForce * Time.deltaTime);
            }
        }

        private bool IsForce = false;
        private Vector3 direction; 

        public void ShowVictory()
        {
            m_Rigibody.isKinematic = true;
            switch(CharacterType)
            {
                case Enum.CharacterType.Human:
                    m_Animator.Play("Victory");
                    break;
                case Enum.CharacterType.Animal:
                    m_Animator.Play("Victory");
                    m_Animator.SetBool("Eat", false);
                    for(int i = 0; i < m_Animator.runtimeAnimatorController.animationClips.Length; i++)
                    {
                        if(m_Animator.runtimeAnimatorController.animationClips[i].name == "Jump W Root")
                        {
                            m_Animator.Play("Jump W Root");
                            System.Action jump = () =>
                            {
                                m_Animator.Play("Jump W Root");
                            };
                            CoroutineUtils.PlayManyCoroutine(0, m_Animator.runtimeAnimatorController.animationClips[i].length, jump, jump, jump, jump, jump);
                        }                   
                    }
                    break;
            }    
        }

        public void ShowDead()
        {
            //m_Rigibody.isKinematic = true;
            m_Rigibody.constraints = RigidbodyConstraints.FreezePositionY | 
                RigidbodyConstraints.FreezeRotationX | 
                RigidbodyConstraints.FreezeRotationZ;
            switch (CharacterType)
            {
                case Enum.CharacterType.Human:
                    IsForce = true;
                    m_Animator.Play("Dead");
                    break;
                case Enum.CharacterType.Animal:
                    IsForce = true;
                    m_Animator.Play("Dead");
                    break;
            }

            if (Game.LevelManager.Instance.m_AttackType == Enum.AttackType.Enemy)
            {
                IsForce = false;
                m_Rigibody.isKinematic = true;
                m_Rigibody.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }
}
