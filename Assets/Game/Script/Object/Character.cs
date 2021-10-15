using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Character : Entity
    {
        public Enum.CharacterType CharacterType;

        private Animator Anim;

        protected override void Awake()
        {
            base.Awake();
            m_Rigibody.useGravity = false;
            m_Rigibody.constraints = RigidbodyConstraints.FreezeAll;

            Anim = this.GetComponent<Animator>();
        }

        private string[] detechedCollisions = new string[]
            {
                "Plane",
                "rock"
            };

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.collider.name);

            if(collision.collider.gameObject.layer != LayerMask.NameToLayer("Bullet"))
            {
                for (int i = 0; i < detechedCollisions.Length; i++)
                {
                    if (collision.collider.name.Contains(detechedCollisions[i]))
                        return;
                }
            }    

            if (Game.LevelManager.Instance.IsVictory)
                return;

            Game.LevelManager.Instance.OnLose();
            ShowDead();
        }

        public void ShowVictory()
        {
            m_Rigibody.isKinematic = true;
            switch(CharacterType)
            {
                case Enum.CharacterType.Human:
                    Anim.Play("Victory");
                    break;
                case Enum.CharacterType.Animal:
                    Anim.Play("Victory");
                    break;
            }    
        }

        public void ShowDead()
        {
            m_Rigibody.isKinematic = true;
            switch (CharacterType)
            {
                case Enum.CharacterType.Human:
                    Anim.Play("Dead");
                    break;
                case Enum.CharacterType.Animal:
                    Anim.Play("Dead");
                    break;
            }
        }
    }
}
