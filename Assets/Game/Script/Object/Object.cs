using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Object : Entity
    {
        protected override void Awake()
        {
            base.Awake();
            m_Rigibody.isKinematic = true;
        }
    }
}
