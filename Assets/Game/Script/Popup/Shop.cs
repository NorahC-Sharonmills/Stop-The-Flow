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

        public void ReloadButton(string str)
        {
            switch(str)
            {
                case "Clothes":
                    for(int i = 0; i < Clothes.Count; i++)
                    {
                        Clothes[i].Initialized();
                    }    
                    break;
                case "Hat":
                    for (int i = 0; i < Hats.Count; i++)
                    {
                        Hats[i].Initialized();
                    }
                    break;
                case "Hair":
                    for (int i = 0; i < Hairs.Count; i++)
                    {
                        Hairs[i].Initialized();
                    }
                    break;
                case "Utility":
                    for (int i = 0; i < Utilitys.Count; i++)
                    {
                        Utilitys[i].Initialized();
                    }
                    break;
            }    
        }    

        private string RuntimeTab = "";

        public GameObject m_ListColorObject;

        [Header("color")]
        public Image[] Colors;

        [Header("character")]
        public GameObject m_ShopObjectPreview;
        public ShopCharacter[] m_Characters;
        public Transform[] m_CharacterNoCameraRenderers;
        [Header("Hair")]
        public Transform[] m_HairNoCameraRenderers;
        [Header("Hat")]
        public Transform[] m_HatNoCameraRenderers;
        [Header("Utility")]
        public Transform[] m_UtilityCameraRenderers;

        public GameObject GetSkinRuntime()
        {
            GameObject rs = null;

            for(int i = 0; i < m_Characters.Length; i++)
            {
                if(m_Characters[i].name == RuntimeStorageData.PLAYER.m_SkinUsing)
                {
                    rs = m_Characters[i].gameObject;
                }
            }

            return rs;
        }

        public GameObject GetHairRuntime()
        {
            GameObject rs = null;

            for(int i = 0; i < m_HairNoCameraRenderers.Length; i++)
            {
                if (m_HairNoCameraRenderers[i].name == RuntimeStorageData.PLAYER.m_HairUsing)
                {
                    rs = m_HairNoCameraRenderers[i].gameObject;
                }
            }

            return rs;
        }

        [HideInInspector] public bool IsInitialized = false;
        public void InitializedAllItem()
        {
            IsInitialized = true;
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
                Item.UIObject3D.ObjectPrefab = m_CharacterNoCameraRenderers[i];
            }

            GameObject ImageObjectHairNone = Instantiate(m_ImagePrefabs, m_GridHair);
            ImageObjectHairNone.SetActive(true);
            ImageObjectHairNone.name = "None";

            ShopItem ItemHairNone = ImageObjectHairNone.GetComponent<ShopItem>();
            ItemHairNone.id = ImageObjectHairNone.name;
            ItemHairNone.type = "Hair";
            Hairs.Add(ItemHairNone);
            ItemHairNone.UIObject3D.ObjectPrefab = null;

            for (int i = 0; i < Data.m_PrefabHairs.Length; i++)
            {
                GameObject ImageObject = Instantiate(m_ImagePrefabs, m_GridHair);
                ImageObject.SetActive(true);
                ImageObject.name = Data.m_PrefabHairs[i].name;

                ShopItem Item = ImageObject.GetComponent<ShopItem>();
                Item.id = ImageObject.name;
                Item.type = "Hair";
                Hairs.Add(Item);
                Item.UIObject3D.ObjectPrefab = m_HairNoCameraRenderers[i];
                Item.UIObject3D.LightIntensity = 1.1f;
            }

            GameObject ImageObjectHitNone = Instantiate(m_ImagePrefabs, m_GridHat);
            ImageObjectHitNone.SetActive(true);
            ImageObjectHitNone.name = "None";

            ShopItem ItemHitNone = ImageObjectHitNone.GetComponent<ShopItem>();
            ItemHitNone.id = ImageObjectHitNone.name;
            ItemHitNone.type = "Hat";
            Hats.Add(ItemHitNone);
            ItemHitNone.UIObject3D.ObjectPrefab = null;

            for (int i = 0; i < Data.m_PrefabHats.Length; i++)
            {
                GameObject ImageObject = Instantiate(m_ImagePrefabs, m_GridHat);
                ImageObject.SetActive(true);
                ImageObject.name = Data.m_PrefabHats[i].name;

                ShopItem Item = ImageObject.GetComponent<ShopItem>();
                Item.id = ImageObject.name;
                Item.type = "Hat";
                Hats.Add(Item);
                Item.UIObject3D.ObjectPrefab = m_HatNoCameraRenderers[i];
            }

            GameObject ImageObjectUtilityNone = Instantiate(m_ImagePrefabs, m_GridUtility);
            ImageObjectUtilityNone.SetActive(true);
            ImageObjectUtilityNone.name = "None";

            ShopItem ItemUtilityNone = ImageObjectUtilityNone.GetComponent<ShopItem>();
            ItemUtilityNone.id = ImageObjectUtilityNone.name;
            ItemUtilityNone.type = "Utility";
            Utilitys.Add(ItemUtilityNone);
            ItemUtilityNone.UIObject3D.ObjectPrefab = null;

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
                Item.UIObject3D.TargetRotation = DefaultRotationUtility;
                Item.UIObject3D.CameraDistance = Data.m_UtilityShowSettings[i].CameraDistance;
                Item.UIObject3D.CameraFOV = Data.m_UtilityShowSettings[i].CameraFOV;
            }

            for (int i = 0; i < m_Characters.Length; i++)
            {
                if (m_Characters[i].name == RuntimeStorageData.PLAYER.m_SkinUsing)
                {
                    m_Characters[i].gameObject.SetActive(true);
                }
                else
                {
                    m_Characters[i].gameObject.SetActive(false);
                }

                MeshRenderer HairRenderer = m_Characters[i].GetHairWithName(RuntimeStorageData.PLAYER.m_HairUsing);
                if (RuntimeStorageData.PLAYER.m_HairUsing != "None")
                {
                    Material[] Hairmats = HairRenderer.materials;
                    Hairmats[0] = Game.ResourceManager.Instance.ShopInfo.m_MaterialHairColors[RuntimeStorageData.PLAYER.m_HairColorUsing];
                    HairRenderer.materials = Hairmats;
                }


                MeshRenderer HatRenderer = m_Characters[i].GetHatWithName(RuntimeStorageData.PLAYER.m_HatUsing);
                if (RuntimeStorageData.PLAYER.m_HatUsing != "None")
                {
                    Material[] HatMats = HatRenderer.materials;
                    HatMats[0] = Game.ResourceManager.Instance.ShopInfo.m_MaterialHatColors[RuntimeStorageData.PLAYER.m_HatColorUsing];
                    HatRenderer.materials = HatMats;
                }

                MeshRenderer UtilityRenderer = m_Characters[i].GetItemOnHand(RuntimeStorageData.PLAYER.m_UtilityUsing);

                MeshRenderer FaceRenderer = m_Characters[i].GetFaceWithName(RuntimeStorageData.PLAYER.m_FaceUsing);
                Material[] FaceMats = FaceRenderer.materials;
                FaceMats[0] = Game.ResourceManager.Instance.ShopInfo.m_MaterialFaceColors[0];
                FaceRenderer.materials = FaceMats;
            }

            for (int i = 0; i < m_CharacterNoCameraRenderers.Length; i++)
            {
                SkinnedMeshRenderer Renderer = m_CharacterNoCameraRenderers[i].GetChild(0).GetComponent<SkinnedMeshRenderer>();
                Material[] mats = Renderer.materials;
                mats[0] = Game.ResourceManager.Instance.ShopInfo.m_MaterialWhiteOutfitsColors[RuntimeStorageData.PLAYER.m_SkinColor];
                Renderer.materials = mats;

                m_Characters[i].GetSkinnedMeshRenderer.materials = mats;
                m_Characters[i].GetHeadMeshRenderer.materials = mats;
            }

            for (int i = 0; i < m_HairNoCameraRenderers.Length; i++)
            {
                MeshRenderer Renderer = m_HairNoCameraRenderers[i].GetComponent<MeshRenderer>();
                Material[] mats = Renderer.materials;
                mats[0] = Game.ResourceManager.Instance.ShopInfo.m_MaterialHairColors[RuntimeStorageData.PLAYER.m_HairColorUsing];
                Renderer.materials = mats;
            }

            for (int i = 0; i < m_HatNoCameraRenderers.Length; i++)
            {
                MeshRenderer Renderer = m_HatNoCameraRenderers[i].GetComponent<MeshRenderer>();
                Material[] mats = Renderer.materials;
                mats[0] = Game.ResourceManager.Instance.ShopInfo.m_MaterialHatColors[RuntimeStorageData.PLAYER.m_HatColorUsing];
                Renderer.materials = mats;
            }

            HairChoose = RuntimeStorageData.PLAYER.m_HairUsing;
            HatChoose = RuntimeStorageData.PLAYER.m_HatUsing;
            UtilityChoose = RuntimeStorageData.PLAYER.m_UtilityUsing;
        }

        private Vector3 DefaultRotationUtility = new Vector3(-37.5f, -270f, 0f);

        public void ChooseSkinPreviewWithId(string str)
        {
            for (int i = 0; i < m_Characters.Length; i++)
            {
                if (m_Characters[i].name == str)
                {
                    m_Characters[i].gameObject.SetActive(true);
                    m_Characters[i].Dance();
                }
                else
                {
                    m_Characters[i].gameObject.SetActive(false);
                }
            }
        }   
        
        public void ChooseHairPreviewWithId(string str)
        {
            HairChoose = str;
            for (int i = 0; i < m_Characters.Length; i++)
            {
                MeshRenderer Renderer = m_Characters[i].GetHairWithName(str);
                if (str == "None")
                    continue;
                Material[] mats = Renderer.materials;
                mats[0] = Game.ResourceManager.Instance.ShopInfo.m_MaterialHairColors[RuntimeStorageData.PLAYER.m_HairColorUsing];
                Renderer.materials = mats;
            }
        }

        public void ChooseHatPreviewWithId(string str)
        {
            HatChoose = str;

            for (int i = 0; i < m_Characters.Length; i++)
            {
                MeshRenderer Renderer = m_Characters[i].GetHatWithName(str);
                if (str == "None")
                    continue;
                Material[] mats = Renderer.materials;
                mats[0] = Game.ResourceManager.Instance.ShopInfo.m_MaterialHatColors[RuntimeStorageData.PLAYER.m_HatColorUsing];
                Renderer.materials = mats;
            }
        }

        public void ChooseUtilityPreviewWithId(string str)
        {
            Debug.Log(str);
            UtilityChoose = str;
            for (int i = 0; i < m_Characters.Length; i++)
            {
                MeshRenderer Renderer = m_Characters[i].GetItemOnHand(str);
            }
        }

        [HideInInspector]
        public string HairChoose = "";
        [HideInInspector]
        public string HatChoose = "";
        [HideInInspector]
        public string UtilityChoose = "";
        public void ChooseColor(int index)
        {
            switch (RuntimeTab)
            {
                case "clothes":
                    for (int i = 0; i < m_CharacterNoCameraRenderers.Length; i++)
                    {
                        SkinnedMeshRenderer Renderer = m_CharacterNoCameraRenderers[i].GetComponent<ShopCharacter>().GetSkinnedMeshRenderer;
                        Material[] mats = Renderer.materials;
                        switch(RuntimeStorageData.PLAYER.m_SkinColorUsing)
                        {
                            case "white":
                                mats[0] = Game.ResourceManager.Instance.ShopInfo.m_MaterialWhiteOutfitsColors[index];
                                break;
                            case "black":
                                mats[0] = Game.ResourceManager.Instance.ShopInfo.m_MaterialBlackOutfitsColors[index];
                                break;
                            default:
                                mats[0] = Game.ResourceManager.Instance.ShopInfo.m_MaterialWhiteOutfitsColors[index];
                                break;
                        }
                        Renderer.materials = mats;

                        m_Characters[i].GetSkinnedMeshRenderer.materials = mats;
                        m_Characters[i].GetHeadMeshRenderer.materials = mats;
                    }


                    for (int i = 0; i < Clothes.Count; i++)
                    {
                        Clothes[i].UIObject3D.HardUpdateDisplay();
                    }

                    RuntimeStorageData.PLAYER.m_SkinColor = index;
                    break;
                case "hair":
                    for (int i = 0; i < m_HairNoCameraRenderers.Length; i++)
                    {
                        MeshRenderer Renderer = m_HairNoCameraRenderers[i].GetComponent<MeshRenderer>();
                        Material[] mats = Renderer.materials;
                        mats[0] = Game.ResourceManager.Instance.ShopInfo.m_MaterialHairColors[index];
                        Renderer.materials = mats;
                    }

                    for (int i = 0; i < Hairs.Count; i++)
                    {
                        Hairs[i].UIObject3D.HardUpdateDisplay();
                    }

                    for (int i = 0; i < m_Characters.Length; i++)
                    {
                        MeshRenderer Renderer = m_Characters[i].GetHairWithName(HairChoose);
                        Material[] mats = Renderer.materials;
                        mats[0] = Game.ResourceManager.Instance.ShopInfo.m_MaterialHairColors[index];
                        Renderer.materials = mats;
                    }

                    RuntimeStorageData.PLAYER.m_HairColorUsing = index;
                    break;
                case "hat":
                    for (int i = 0; i < m_HatNoCameraRenderers.Length; i++)
                    {
                        MeshRenderer Renderer = m_HatNoCameraRenderers[i].GetComponent<MeshRenderer>();
                        Material[] mats = Renderer.materials;
                        mats[0] = Game.ResourceManager.Instance.ShopInfo.m_MaterialHatColors[index];
                        Renderer.materials = mats;                   
                    }

                    for(int i = 0; i < Hats.Count; i++)
                    {
                        Hats[i].UIObject3D.HardUpdateDisplay();
                    }    

                    for (int i = 0; i < m_Characters.Length; i++)
                    {
                        MeshRenderer Renderer = m_Characters[i].GetHatWithName(HatChoose);
                        Material[] mats = Renderer.materials;
                        mats[0] = Game.ResourceManager.Instance.ShopInfo.m_MaterialHatColors[index];
                        Renderer.materials = mats;
                    }

                    RuntimeStorageData.PLAYER.m_HatColorUsing = index;
                    break;
                case "utility":
                    break;
            }
        }

        public void ChooseSkinColor(int index)
        {
            switch(index)
            {
                case 0:
                    RuntimeStorageData.PLAYER.m_SkinColorUsing = "white";
                    break;
                case 1:
                    RuntimeStorageData.PLAYER.m_SkinColorUsing = "black";
                    break;
            }

            for (int i = 0; i < m_CharacterNoCameraRenderers.Length; i++)
            {
                SkinnedMeshRenderer Renderer = m_CharacterNoCameraRenderers[i].GetComponent<ShopCharacter>().GetSkinnedMeshRenderer;
                Material[] mats = Renderer.materials;
                switch (RuntimeStorageData.PLAYER.m_SkinColorUsing)
                {
                    case "white":
                        mats[0] = Game.ResourceManager.Instance.ShopInfo.m_MaterialWhiteOutfitsColors[RuntimeStorageData.PLAYER.m_SkinColor];
                        break;
                    case "black":
                        mats[0] = Game.ResourceManager.Instance.ShopInfo.m_MaterialBlackOutfitsColors[RuntimeStorageData.PLAYER.m_SkinColor];
                        break;
                    default:
                        mats[0] = Game.ResourceManager.Instance.ShopInfo.m_MaterialWhiteOutfitsColors[RuntimeStorageData.PLAYER.m_SkinColor];
                        break;
                }
                Renderer.materials = mats;
                Clothes[i].UIObject3D.HardUpdateDisplay();

                m_Characters[i].GetSkinnedMeshRenderer.materials = mats;
                m_Characters[i].GetHeadMeshRenderer.materials = mats;
            }
        }

        public bool IsShop = false;
        public void Show()
        {
            m_Canvas.SetActive(true);
            m_ShopObject.SetActive(true);
            m_Animator.Play("Show");

            OnTab("clothes");

            m_ShopObjectPreview.SetActive(true);
            IsShop = true;
        }

        public void Home()
        {
            var chars = Game.LevelManager.Instance.Characters;
            for (int i = 0; i < chars.Count; i++)
            {
                var _sc = chars[i].GetComponent<Game.Character>();
                if (_sc != null)
                    _sc.ReloadCharacter();
            }

            SoundManager.Instance.PlayOnShot(Sound.CLICK);
            FirebaseManager.Instance.ShowInterBackHome();
            IronSourceManager.Instance.ShowInter(() =>
            {
                m_Animator.Play("Hide");
                CoroutineUtils.PlayCoroutine(() =>
                {
                    m_ShopObjectPreview.SetActive(false);

                    m_Canvas.SetActive(false);
                    m_ShopObject.SetActive(false);
                    Game.UIManager.Instance.Home();
                }, 0.2f);
                IsShop = false;
            });   
        }

        public void OnTab(string tab)
        {
            SoundManager.Instance.PlayOnShot(Sound.CLICK);
            for (int i = 0; i < tabs.Length; i++)
            {
                tabs[i].SetActive(false);
                buttons[i].color = m_DisableColor;
            }

            RuntimeTab = tab;

            switch (tab)
            {
                case "clothes":
                    m_ListColorObject.SetActive(true);
                    tabs[0].SetActive(true);
                    buttons[0].color = m_EnableColor;

                    for(int i = 0; i < Clothes.Count; i++)
                    {
                        Clothes[i].Initialized();
                    }

                    for (int i = 0; i < Colors.Length; i++)
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
                    break;
                case "hair":
                    m_ListColorObject.SetActive(true);
                    tabs[1].SetActive(true);
                    buttons[1].color = m_EnableColor;

                    for (int i = 0; i < Hairs.Count; i++)
                    {
                        Hairs[i].Initialized();
                    }

                    for (int i = 0; i < Colors.Length; i++)
                    {
                        if (i < Game.ResourceManager.Instance.ShopInfo.m_HairColors.Length)
                        {
                            Colors[i].gameObject.SetActive(true);
                            Colors[i].color = Game.ResourceManager.Instance.ShopInfo.m_HairColors[i];
                        }
                        else
                        {
                            Colors[i].gameObject.SetActive(false);
                        }
                    }
                    break;
                case "hat":
                    m_ListColorObject.SetActive(true);
                    tabs[2].SetActive(true);
                    buttons[2].color = m_EnableColor;

                    for (int i = 0; i < Hats.Count; i++)
                    {
                        Hats[i].Initialized();
                    }

                    for (int i = 0; i < Colors.Length; i++)
                    {
                        if (i < Game.ResourceManager.Instance.ShopInfo.m_HatColors.Length)
                        {
                            Colors[i].gameObject.SetActive(true);
                            Colors[i].color = Game.ResourceManager.Instance.ShopInfo.m_HatColors[i];
                        }
                        else
                        {
                            Colors[i].gameObject.SetActive(false);
                        }
                    }
                    break;
                case "utility":
                    m_ListColorObject.SetActive(false);
                    tabs[3].SetActive(true);
                    buttons[3].color = m_EnableColor;

                    for (int i = 0; i < Utilitys.Count; i++)
                    {
                        Utilitys[i].Initialized();
                    }

                    for (int i = 0; i < Colors.Length; i++)
                    {
                        if (i < Game.ResourceManager.Instance.ShopInfo.m_FaceColors.Length)
                        {
                            Colors[i].gameObject.SetActive(true);
                            Colors[i].color = Game.ResourceManager.Instance.ShopInfo.m_FaceColors[i];
                        }
                        else
                        {
                            Colors[i].gameObject.SetActive(false);
                        }
                    }
                    break;
            }    
        }    
    }
}
