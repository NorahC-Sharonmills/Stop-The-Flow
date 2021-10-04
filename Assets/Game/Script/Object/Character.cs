using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Character : Entity
    {
        public Enum.CharacterType CharacterType;

        protected override void Awake()
        {
            base.Awake();
            m_Rigibody.useGravity = false;
        }
    }
}
