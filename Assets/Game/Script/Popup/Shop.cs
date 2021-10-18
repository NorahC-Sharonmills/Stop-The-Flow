using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Shop : MonoSingletonGlobal<Shop>
    {
        public GameObject m_Canvas;
        public GameObject m_ShopObject;
        public Animator m_Animator;
        public GameObject m_ImagePrefabs;
        public Transform m_GridClothes;
        public Transform m_GridHair;
        public Transform m_GridHat;
        public Transform m_GridUtility;

        [Header("tab")]
        public GameObject[] tabs;

        protected override void Awake()
        {
            base.Awake();
            ShopScriptableObject Data = ResourceManager.Instance.ShopInfo;
            for (int i = 0; i < Data.m_PrefabOutfits.Length; i++)
            {
                GameObject ImageObject = Instantiate(m_ImagePrefabs, m_GridClothes);
                ImageObject.SetActive(true);
                ImageObject.name = Data.m_PrefabOutfits[i].name;

                ShopItem Item = ImageObject.GetComponent<ShopItem>();
                Item.UIObject3D.ObjectPrefab = Data.m_PrefabOutfits[i].transform;
            }

            for (int i = 0; i < Data.m_PrefabHairs.Length; i++)
            {
                GameObject ImageObject = Instantiate(m_ImagePrefabs, m_GridHair);
                ImageObject.SetActive(true);
                ImageObject.name = Data.m_PrefabHairs[i].name;

                ShopItem Item = ImageObject.GetComponent<ShopItem>();
                Item.UIObject3D.ObjectPrefab = Data.m_PrefabHairs[i].transform;
            }

            for (int i = 0; i < Data.m_PrefabHats.Length; i++)
            {
                GameObject ImageObject = Instantiate(m_ImagePrefabs, m_GridHat);
                ImageObject.SetActive(true);
                ImageObject.name = Data.m_PrefabHats[i].name;

                ShopItem Item = ImageObject.GetComponent<ShopItem>();
                Item.UIObject3D.ObjectPrefab = Data.m_PrefabHats[i].transform;
            }

            for (int i = 0; i < Data.m_PrefabsUtilitys.Length; i++)
            {
                GameObject ImageObject = Instantiate(m_ImagePrefabs, m_GridUtility);
                ImageObject.SetActive(true);
                ImageObject.name = Data.m_PrefabsUtilitys[i].name;

                ShopItem Item = ImageObject.GetComponent<ShopItem>();
                Item.UIObject3D.ObjectPrefab = Data.m_PrefabsUtilitys[i].transform;
            }

            OnTab("clothes");
        }

        public void Show()
        {
            m_Canvas.SetActive(true);
            m_ShopObject.SetActive(true);
            m_Animator.Play("Show");
        }

        public void Home()
        {
            m_Animator.Play("Hide");
            CoroutineUtils.PlayCoroutine(() =>
            {
                m_Canvas.SetActive(false);
                m_ShopObject.SetActive(false);
                Game.UIManager.Instance.Home();
            }, 0.2f);
        }

        public void OnTab(string tab)
        {
            for(int i = 0; i < tabs.Length; i++)
            {
                tabs[i].SetActive(false);
            }

            switch(tab)
            {
                case "clothes":
                    tabs[0].SetActive(true);
                    break;
                case "hair":
                    tabs[1].SetActive(true);
                    break;
                case "hat":
                    tabs[2].SetActive(true);
                    break;
                case "utility":
                    tabs[3].SetActive(true);
                    break;
            }    
        }    
    }
}
