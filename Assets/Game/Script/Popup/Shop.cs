using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        public Image[] buttons;
        public Color m_DisableColor;
        public Color m_EnableColor;

        private List<ShopItem> Clothes = new List<ShopItem>();
        private List<ShopItem> Hats = new List<ShopItem>();
        private List<ShopItem> Hairs = new List<ShopItem>();
        private List<ShopItem> Utilitys = new List<ShopItem>();

        private string RuntimeTab = "";

        [Header("color")]
        public Image[] Colors;

        [Header("character")]
        public GameObject m_ShopObjectPreview;
        public ShopCharacter[] m_Characters;
        public Transform[] m_CharacterNoCameraRenderers;

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
                Item.id = ImageObject.name;
                Item.type = "Clothes";
                Clothes.Add(Item);
                //Item.UIObject3D.ObjectPrefab = Data.m_PrefabOutfits[i].transform;
                Item.UIObject3D.ObjectPrefab = m_CharacterNoCameraRenderers[i].transform;
            }

            for (int i = 0; i < Data.m_PrefabHairs.Length; i++)
            {
                GameObject ImageObject = Instantiate(m_ImagePrefabs, m_GridHair);
                ImageObject.SetActive(true);
                ImageObject.name = Data.m_PrefabHairs[i].name;

                ShopItem Item = ImageObject.GetComponent<ShopItem>();
                Item.id = ImageObject.name;
                Item.type = "Hair";
                Hairs.Add(Item);
                Item.UIObject3D.ObjectPrefab = Data.m_PrefabHairs[i].transform;
            }

            for (int i = 0; i < Data.m_PrefabHats.Length; i++)
            {
                GameObject ImageObject = Instantiate(m_ImagePrefabs, m_GridHat);
                ImageObject.SetActive(true);
                ImageObject.name = Data.m_PrefabHats[i].name;

                ShopItem Item = ImageObject.GetComponent<ShopItem>();
                Item.id = ImageObject.name;
                Item.type = "Hat";
                Hats.Add(Item);
                Item.UIObject3D.ObjectPrefab = Data.m_PrefabHats[i].transform;
            }

            for (int i = 0; i < Data.m_PrefabsUtilitys.Length; i++)
            {
                GameObject ImageObject = Instantiate(m_ImagePrefabs, m_GridUtility);
                ImageObject.SetActive(true);
                ImageObject.name = Data.m_PrefabsUtilitys[i].name;

                ShopItem Item = ImageObject.GetComponent<ShopItem>();
                Item.id = ImageObject.name;
                Item.type = "Utility";
                Utilitys.Add(Item);
                Item.UIObject3D.ObjectPrefab = Data.m_PrefabsUtilitys[i].transform;
            }
        }

        public void Initializeded()
        {
            for(int i = 0; i < Colors.Length; i++)
            {
                if (i < Game.ResourceManager.Instance.ShopInfo.m_OutfitsColors.Length)
                {
                    Colors[i].gameObject.SetActive(true);
                    Colors[i].color = Game.ResourceManager.Instance.ShopInfo.m_OutfitsColors[i];
                }
                else
                {
                    Colors[i].gameObject.SetActive(false);
                }    
            }

            for (int i = 0; i < m_Characters.Length; i++)
            {
                if (m_Characters[i].name == RuntimeStorageData.PLAYER.character_using)
                {
                    m_Characters[i].gameObject.SetActive(true);
                }
                else
                {
                    m_Characters[i].gameObject.SetActive(false);
                }
            }
        }

        public void ChooseColor(int index)
        {
            switch (RuntimeTab)
            {
                case "clothes":
                    for (int i = 0; i < m_CharacterNoCameraRenderers.Length; i++)
                    {
                        SkinnedMeshRenderer Renderer = m_CharacterNoCameraRenderers[i].GetComponent<ShopCharacter>().GetSkinnedMeshRenderer;
                        Material[] mats = Renderer.materials;
                        mats[0] = Game.ResourceManager.Instance.ShopInfo.m_MaterialWhiteOutfitsColors[index];
                        Renderer.materials = mats;
                        Clothes[i].UIObject3D.HardUpdateDisplay();

                        m_Characters[i].GetSkinnedMeshRenderer.materials = mats;
                    }
                    RuntimeStorageData.PLAYER.character_color_using = index;
                    break;
                case "hair":

                    break;
                case "hat":

                    break;
                case "utility":

                    break;
            }
        }

        public void Show()
        {
            m_Canvas.SetActive(true);
            m_ShopObject.SetActive(true);
            m_Animator.Play("Show");

            OnTab("clothes");
            for (int i = 0; i < m_CharacterNoCameraRenderers.Length; i++)
            {
                SkinnedMeshRenderer Renderer = m_CharacterNoCameraRenderers[i].GetChild(0).GetComponent<SkinnedMeshRenderer>();
                Material[] mats = Renderer.materials;
                mats[0] = Game.ResourceManager.Instance.ShopInfo.m_MaterialWhiteOutfitsColors[RuntimeStorageData.PLAYER.character_color_using];
                Renderer.materials = mats;

                m_Characters[i].GetSkinnedMeshRenderer.materials = mats;
            }

            m_ShopObjectPreview.SetActive(true);
        }

        public void Home()
        {
            m_Animator.Play("Hide");
            CoroutineUtils.PlayCoroutine(() =>
            {
                m_ShopObjectPreview.SetActive(false);

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
                buttons[i].color = m_DisableColor;
            }

            RuntimeTab = tab;

            switch (tab)
            {
                case "clothes":
                    tabs[0].SetActive(true);
                    buttons[0].color = m_EnableColor;

                    for(int i = 0; i < Clothes.Count; i++)
                    {
                        Clothes[i].Initialized();
                    }
                    break;
                case "hair":
                    tabs[1].SetActive(true);
                    buttons[1].color = m_EnableColor;

                    for (int i = 0; i < Hairs.Count; i++)
                    {
                        Hairs[i].Initialized();
                    }
                    break;
                case "hat":
                    tabs[2].SetActive(true);
                    buttons[2].color = m_EnableColor;

                    for (int i = 0; i < Hats.Count; i++)
                    {
                        Hats[i].Initialized();
                    }
                    break;
                case "utility":
                    tabs[3].SetActive(true);
                    buttons[3].color = m_EnableColor;

                    for (int i = 0; i < Utilitys.Count; i++)
                    {
                        Utilitys[i].Initialized();
                    }
                    break;
            }    
        }    
    }
}
