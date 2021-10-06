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

            Anim = this.GetComponent<Animator>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.collider.name != "Plane")
            {
                Game.LevelManager.Instance.OnLose();
                ShowDead();
            }    
        }

        public void ShowVictory()
        {
            m_Rigibody.isKinematic = true;
            Anim.Play("Victory");
        }

        public void ShowDead()
        {
            m_Rigibody.isKinematic = true;
            Anim.Play("Dead");
        }
    }
}
