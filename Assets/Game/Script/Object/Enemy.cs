using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Enemy : Entity
    {
        public Enum.EnemyType EnemyType;
        private Collider m_Collider;
        public GameObject m_Bullet;
        public Transform m_BulletPositionSpawner;

        protected override void Awake()
        {
            base.Awake();

            m_Collider = this.GetComponent<Collider>();

            switch (EnemyType)
            {
                case Enum.EnemyType.Weapon:
                    m_Rigibody.isKinematic = true;
                    break;
            }    
        }

        private void Start()
        {
            switch (EnemyType)
            {
                case Enum.EnemyType.Weapon:
                    m_Rigibody.isKinematic = true;
                    IsShoot = false;
                    break;
            }
        }

        private void Update()
        {
            if(StaticVariable.GameState == GameState.PLAY)
            {
                switch (EnemyType)
                {
                    case Enum.EnemyType.Weapon:
                        OnceShoot();
                        break;
                }
            }
        }

        private bool IsShoot = false;
        private void OnceShoot()
        {
            if (IsShoot)
                return;
            IsShoot = true;

            var bullet = Instantiate(m_Bullet, m_BulletPositionSpawner.position, Quaternion.identity).GetComponent<Game.Bullet>();
            bullet.AddForce(m_BulletPositionSpawner.forward * 800);
        }
    }
}


