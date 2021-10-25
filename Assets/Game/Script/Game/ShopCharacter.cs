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

        private Transform m_HairContainer = null;
        public MeshRenderer GetHairWithName(string str)
        {
            if (m_HairContainer == null)
            {
                m_HairContainer = new GameObject("HairContainer").transform;
                m_HairContainer.parent = m_Head;
                m_HairContainer.localPosition = Vector3.zero;
                m_HairContainer.localRotation = Quaternion.identity;
                m_HairContainer.localScale = Vector3.one;
            }

            MeshRenderer rs = null;
            for(int i = 0; i < m_HairContainer.childCount; i++)
            {
                if(m_HairContainer.GetChild(i).name == str)
                {
                    rs = m_HairContainer.GetChild(i).GetComponent<MeshRenderer>();
                    m_HairContainer.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    m_HairContainer.GetChild(i).gameObject.SetActive(false);
                }
            }    

            if(rs == null)
            {
                var findObject = Game.ResourceManager.Instance.GetHairWithId(str);
                if (findObject == null)
                    return rs;
                GameObject HairObject = Instantiate(Game.ResourceManager.Instance.GetHairWithId(str), m_HairContainer) as GameObject;
                HairObject.name = HairObject.name.Replace("(Clone)", "");
                rs = HairObject.GetComponent<MeshRenderer>();
            }

            return rs;

        }

        private Transform m_HatContainer = null;
        public MeshRenderer GetHatWithName(string str)
        {
            if (m_HatContainer == null)
            {
                m_HatContainer = new GameObject("HatContainer").transform;
                m_HatContainer.parent = m_Head;
                m_HatContainer.localPosition = Vector3.zero;
                m_HatContainer.localRotation = Quaternion.identity;
                m_HatContainer.localScale = Vector3.one;
            }

            MeshRenderer rs = null;
            for (int i = 0; i < m_HatContainer.childCount; i++)
            {
                if (m_HatContainer.GetChild(i).name == str)
                {
                    rs = m_HatContainer.GetChild(i).GetComponent<MeshRenderer>();
                    m_HatContainer.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    m_HatContainer.GetChild(i).gameObject.SetActive(false);
                }
            }

            if (rs == null)
            {
                var findObject = Game.ResourceManager.Instance.GetHatWithId(str);
                if (findObject == null)
                    return rs;
                GameObject HatObject = Instantiate(Game.ResourceManager.Instance.GetHatWithId(str), m_HatContainer) as GameObject;
                HatObject.name = HatObject.name.Replace("(Clone)", "");
                rs = HatObject.GetComponent<MeshRenderer>();
            }

            return rs;
        }

        public enum FaceType
        {
            Worried,
            Angry,
            Happy
        }

        public void SetFace(FaceType faceType)
        {
            switch(faceType)
            {
                case FaceType.Happy:
                    GetFaceWithName("Female Face 01 Black");
                    break;
                case FaceType.Angry:
                    GetFaceWithName("Female Face 02 Black");
                    break;
                case FaceType.Worried:
                    GetFaceWithName("Female Face 03 Black");
                    break;
            }    
        }    

        private Transform m_FaceContainer = null;
        public MeshRenderer GetFaceWithName(string str)
        {
            if (m_FaceContainer == null)
            {
                for(int i = 0; i < m_Head.childCount; i++)
                {
                    if (m_Head.GetChild(i).name == "FaceContainer")
                        m_FaceContainer = m_Head.GetChild(i);
                }    

                if(m_FaceContainer == null)
                {
                    m_FaceContainer = new GameObject("FaceContainer").transform;
                    m_FaceContainer.parent = m_Head;
                    m_FaceContainer.localPosition = Vector3.zero;
                    m_FaceContainer.localRotation = Quaternion.identity;
                    m_FaceContainer.localScale = Vector3.one;
                }    
            }

            MeshRenderer rs = null;
            for (int i = 0; i < m_FaceContainer.childCount; i++)
            {
                if(m_FaceContainer.GetChild(i).name == str)
                {
                    rs = m_FaceContainer.GetChild(i).GetComponent<MeshRenderer>();
                    m_FaceContainer.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    m_FaceContainer.GetChild(i).gameObject.SetActive(false);
                }
            }

            if(rs == null)
            {
                GameObject FaceObject = Instantiate(Game.ResourceManager.Instance.GetUtilityWithId(str), m_FaceContainer) as GameObject;
                FaceObject.name = FaceObject.name.Replace("(Clone)", "");
                rs = FaceObject.GetComponent<MeshRenderer>();
            }

            return rs;
        }

        public int GetIndexOfFace(string str)
        {
            int rs = 0;

            for (int i = 0; i < m_FaceContainer.childCount; i++)
            {
                Debug.Log(m_FaceContainer.GetChild(i).name + " " + str);
                if (m_FaceContainer.GetChild(i).name == str)
                {
                    return i;
                }
            }

            return rs;
        }
    }
}
