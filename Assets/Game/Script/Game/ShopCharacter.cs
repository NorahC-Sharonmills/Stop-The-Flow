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

        private SkinnedMeshRenderer _Skinned;
        public SkinnedMeshRenderer GetSkinnedMeshRenderer
        {
            get
            {
                if (_Skinned == null)
                    _Skinned = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();

                return _Skinned;
            }
        }

        private MeshRenderer _HeadMesh;
        public MeshRenderer GetHeadMeshRenderer
        {
            get
            {
                if (_HeadMesh == null)
                    _HeadMesh = m_Head.GetChild(0).GetComponent<MeshRenderer>();

                return _HeadMesh;
            }
        }
    }
}
