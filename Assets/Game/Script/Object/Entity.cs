using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Rigidbody))]
    public class Entity : MonoBehaviour
    {
        public Enum.ObjectType ObjectType;
        protected Rigidbody m_Rigibody;
        protected virtual void Awake()
        {
            m_Rigibody = this.GetComponent<Rigidbody>();
            if (m_Rigibody == null)
                m_Rigibody = this.gameObject.AddComponent<Rigidbody>();
        }
    }
}
