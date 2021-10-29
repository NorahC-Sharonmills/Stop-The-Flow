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
        private ShopCharacter CharacterInfo;

        [Header("Human")]
        public Material[] FaceStates;

        private Vector3 m_Position;
        private Quaternion m_Rotation;

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
                    //SoundManager.Instance.PlayLoopInfinity(Sound.SCREAM);
                    m_Animator.enabled = false;
                    ReloadCharacter();
                    break;
            }
        }

        private void LateUpdate()
        {
            switch (CharacterType)
            {
                case Enum.CharacterType.Animal:
                    break;
                case Enum.CharacterType.Human:
                    //if(StaticVariable.GameState == GameState.PAUSE &&
                    //    Game.LevelManager.Instance.IsVictory == false &&
                    //    Game.LevelManager.Instance.IsLose == false)
                    //{
                    //    SoundManager.Instance.PlaySoundAsyncWithDelay(Sound.SCREAM, 2.5f);
                    //}    
                    CharacterModels.transform.localPosition = Vector3.zero;
                    CharacterModels.transform.localRotation = Quaternion.identity;
                    break;
            }
        }


        public void ReloadCharacter()
        {
            switch (CharacterType)
            {
                case Enum.CharacterType.Human:
                    bool IsCreated = true;
                    GameObject prefabs = Game.Shop.Instance.GetSkinRuntime();
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        if(transform.GetChild(i).name == prefabs.name)
                        {
                            IsCreated = false;
                            transform.GetChild(i).gameObject.SetActive(true);
                            CharacterModels = transform.GetChild(i).gameObject;
                        }
                        else
                        {
                            transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                    if (IsCreated)
                    {
                        CharacterModels = Instantiate(Game.Shop.Instance.GetSkinRuntime(), transform);
                        CharacterModels.name = CharacterModels.name.Replace("(Clone)", "");
                    }

                    CharacterModels.transform.localPosition = Vector3.zero;
                    CharacterModels.transform.localRotation = Quaternion.identity;
                    m_Animator = CharacterModels.GetComponent<Animator>();
                    m_Animator.enabled = true;
                    m_Animator.runtimeAnimatorController = m_AnimatorController;
                    m_Animator.Play("Idle");

                    CharacterInfo = CharacterModels.GetComponent<ShopCharacter>();
                    CharacterInfo.SetFace(ShopCharacter.FaceType.Worried);

                    ReloadAccessory();

                    SetGameLayerRecursive(CharacterModels, gameObject.layer);
                    break;
            }
        }

        public void ReloadAccessory()
        {
            switch (CharacterType)
            {
                case Enum.CharacterType.Human:
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        if (transform.GetChild(i).gameObject.activeInHierarchy)
                        {
                            CharacterModels = transform.GetChild(i).gameObject;
                        }
                    }

                    var HairRenderer = CharacterInfo.GetHairWithName(RuntimeStorageData.PLAYER.m_HairUsing);
                    if(HairRenderer != null)
                    {
                        Material[] Hairmats = HairRenderer.materials;
                        Hairmats[0] = Game.ResourceManager.Instance.ShopInfo.m_MaterialHairColors[RuntimeStorageData.PLAYER.m_HairColorUsing];
                        HairRenderer.materials = Hairmats;
                    }

                    var HatRenderer = CharacterInfo.GetHatWithName(RuntimeStorageData.PLAYER.m_HatUsing);
                    if (HatRenderer != null)
                    {
                        Material[] HatMats = HatRenderer.materials;
                        HatMats[0] = Game.ResourceManager.Instance.ShopInfo.m_MaterialHatColors[RuntimeStorageData.PLAYER.m_HatColorUsing];
                        HatRenderer.materials = HatMats;
                    }

                    var UtilityRenderer = CharacterInfo.GetItemOnHand(RuntimeStorageData.PLAYER.m_UtilityUsing);

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
                "wall",
                "Fir"
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
                    CharacterInfo.SetFace(ShopCharacter.FaceType.Happy);
                    SetGameLayerRecursive(CharacterModels, gameObject.layer);
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
            Debug.Log("Dead");

            //m_Rigibody.isKinematic = true;
            m_Rigibody.constraints = RigidbodyConstraints.FreezePositionY | 
                RigidbodyConstraints.FreezeRotationX | 
                RigidbodyConstraints.FreezeRotationZ;
            switch (CharacterType)
            {
                case Enum.CharacterType.Human:
                    IsForce = true;
                    m_Animator.Play("Dead");
                    CharacterInfo.SetFace(ShopCharacter.FaceType.Angry);
                    SetGameLayerRecursive(CharacterModels, gameObject.layer);
                    PlaySound(global::Sound.SCREAM);
                    break;
                case Enum.CharacterType.Animal:
                    IsForce = true;
                    m_Animator.Play("Dead");
                    PlaySound(Sound.CHICKEN_DYING);
                    break;
            }

            if (Game.LevelManager.Instance.m_AttackType == Enum.AttackType.Enemy)
            {
                IsForce = false;
                m_Rigibody.isKinematic = true;
                m_Rigibody.constraints = RigidbodyConstraints.FreezeAll;
            }
        }

        private bool isSound = false;
        private void PlaySound(Sound sound)
        {
            if (isSound)
                return;
            isSound = true;
            SoundManager.Instance.PlayOnShot(sound);
        }    
    }
}
