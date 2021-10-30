using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Enemy : Entity
    {
        public Enum.EnemyType EnemyType;
        private Collider m_Collider;
        [Header("weapon")]
        public GameObject m_Bullet;
        public Transform m_BulletPositionSpawner;
        public Transform m_Target;
        [Header("animal")]
        public Animator m_Animation;
        public string m_IdleAnim;
        public string m_RunAnim;
        public string m_AttackAnim;
        public string m_ClipAttack;
        public string m_VictoryAnim;


        protected override void Awake()
        {
            base.Awake();

            m_Collider = this.GetComponent<Collider>();
            if (m_Animation == null)
                m_Animation = this.GetComponent<Animator>();


            switch (EnemyType)
            {
                case Enum.EnemyType.Weapon:
                    DrawManager.Instance.hight = 1f;
                    m_Rigibody.isKinematic = true;
                    break;
                case Enum.EnemyType.Animal:
                    //m_Rigibody.isKinematic = true;
                    m_Animation.Play(m_IdleAnim); 
                    break;
            }
        }

        private void Start()
        {
            switch (EnemyType)
            {
                case Enum.EnemyType.Weapon:
                    IsShoot = false;
                    m_Target = Game.LevelManager.Instance.Characters[0].transform;
                    break;
                case Enum.EnemyType.Animal:
                    IsShoot = false;
                    m_Target = Game.LevelManager.Instance.Characters[0].transform;
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
                    case Enum.EnemyType.Animal:
                        OnceAttack();
                        break;
                }
            }
        }

        private void FixedUpdate()
        {
            switch (EnemyType)
            {
                case Enum.EnemyType.Animal:
                    if (IsShoot && !IsAttack)
                    {
                        var targetPostion = m_Target.position;
                        targetPostion.y = transform.position.y;
                        moveDir = (targetPostion - transform.position).normalized;
                        transform.forward = moveDir;
                        m_Rigibody.velocity = moveDir * speed * Time.deltaTime;
                    }
                    break;
            }
        }

        private bool IsShoot = false;
        private bool IsAttack = false;
        Vector3 moveDir;
        private float speed = 100f;
        private void OnceAttack()
        {
            if (IsShoot)
                return;
            IsShoot = true;
            m_Animation.Play(m_RunAnim);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.collider.name == "character")
            {

                switch (EnemyType)
                {
                    case Enum.EnemyType.Animal:
                        IsAttack = true;
                        m_Rigibody.isKinematic = true;
                        m_Animation.Play(m_AttackAnim);
                        AnimationClip[] clips = m_Animation.runtimeAnimatorController.animationClips;
                        for (int i = 0; i < clips.Length; i++)
                        {
                            if (clips[i].name == m_ClipAttack)
                            {
                                CoroutineUtils.PlayCoroutine(() =>
                                {
                                    for(int i = 0; i < Game.LevelManager.Instance.Enemys.Count; i++)
                                    {
                                        Game.LevelManager.Instance.Enemys[i].GetComponent<Animator>().Play(m_VictoryAnim);
                                    }
                                }, clips[i].length);
                            }
                        }
                        break;
                }
            }
        }

        private void OnceShoot()
        {
            if (IsShoot)
                return;
            IsShoot = true;

            var bullet = Instantiate(m_Bullet, m_BulletPositionSpawner.position, Quaternion.identity).GetComponent<Game.Bullet>();

            moveDir = (m_Target.position - transform.position).normalized;
            bullet.transform.forward = moveDir;
            bullet.AddForce(moveDir);
        }
    }
}


