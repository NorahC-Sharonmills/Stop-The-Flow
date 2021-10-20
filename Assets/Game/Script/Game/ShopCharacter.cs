using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ShopCharacter : MonoBehaviour
    {
        public Transform m_LHand;
        public Transform m_RHand;
        public Transform m_Head;
        public Transform m_Back;

        private SkinnedMeshRenderer _skinned;
        public SkinnedMeshRenderer GetSkinnedMeshRenderer
        {
            get
            {
                if (_skinned == null)
                    _skinned = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();

                return _skinned;
            }
        }
    }
}
