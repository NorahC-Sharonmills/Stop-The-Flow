using System.Collections;
using System.Collections.Generic;
using UI.ThreeDimensional;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public UIObject3D UIObject3D;
    public GameObject m_None;
    public GameObject m_Image;
    public GameObject m_ObjectUsing;
    public GameObject m_ObjectLocked;
    public Image m_BoderUsing;

    public Color ColorUsing;
    public Color ColorUnusing;
    public Color ColorLocked;

    public string id = "";
    public string type = "";

    public void Initialized()
    {
        m_None.SetActive(false);
        m_Image.SetActive(true);

        switch(type)
        {
            case "Clothes":
                if(RuntimeStorageData.PLAYER.characters_bought.Contains(id))
                {
                    if(RuntimeStorageData.PLAYER.character_using == id)
                    {
                        m_BoderUsing.color = ColorUsing;
                        m_ObjectLocked.SetActive(false);
                        m_ObjectUsing.SetActive(true);
                    }
                    else
                    {
                        m_BoderUsing.color = ColorUnusing;
                        m_ObjectLocked.SetActive(false);
                        m_ObjectUsing.SetActive(false);
                    }
                }
                else
                {
                    m_BoderUsing.color = ColorLocked;
                    m_ObjectLocked.SetActive(true);
                    m_ObjectUsing.SetActive(false);
                }
                break;
            case "Hat":
                if (RuntimeStorageData.PLAYER.hats_bought.Contains(id))
                {
                    if (RuntimeStorageData.PLAYER.hat_using == id)
                    {
                        m_BoderUsing.color = ColorUsing;
                        m_ObjectLocked.SetActive(false);
                        m_ObjectUsing.SetActive(true);
                    }
                    else
                    {
                        m_BoderUsing.color = ColorUnusing;
                        m_ObjectLocked.SetActive(false);
                        m_ObjectUsing.SetActive(false);
                    }
                }
                else
                {
                    m_BoderUsing.color = ColorLocked;
                    m_ObjectLocked.SetActive(true);
                    m_ObjectUsing.SetActive(false);
                }

                if (id == "None")
                {
                    m_ObjectLocked.SetActive(false);
                    m_None.SetActive(true);
                    m_Image.SetActive(false);
                }
                break;
            case "Hair": 
                if (RuntimeStorageData.PLAYER.hairs_bought.Contains(id))
                {
                    if (RuntimeStorageData.PLAYER.hair_using == id)
                    {
                        m_BoderUsing.color = ColorUsing;
                        m_ObjectLocked.SetActive(false);
                        m_ObjectUsing.SetActive(true);
                    }
                    else
                    {
                        m_BoderUsing.color = ColorUnusing;
                        m_ObjectLocked.SetActive(false);
                        m_ObjectUsing.SetActive(false);
                    }
                }
                else
                {
                    m_BoderUsing.color = ColorLocked;
                    m_ObjectLocked.SetActive(true);
                    m_ObjectUsing.SetActive(false);
                }

                if (id == "None")
                {
                    m_ObjectLocked.SetActive(false);
                    m_None.SetActive(true);
                    m_Image.SetActive(false);
                }
                break;
            case "Utility":
                if (RuntimeStorageData.PLAYER.utility_bought.Contains(id))
                {
                    if (RuntimeStorageData.PLAYER.utility_using == id)
                    {
                        m_BoderUsing.color = ColorUsing;
                        m_ObjectLocked.SetActive(false);
                        m_ObjectUsing.SetActive(true);
                    }
                    else
                    {
                        m_BoderUsing.color = ColorUnusing;
                        m_ObjectLocked.SetActive(false);
                        m_ObjectUsing.SetActive(false);
                    }
                }
                else
                {
                    m_BoderUsing.color = ColorLocked;
                    m_ObjectLocked.SetActive(true);
                    m_ObjectUsing.SetActive(false);
                }

                if (id == "None")
                {
                    m_ObjectLocked.SetActive(false);
                    m_None.SetActive(true);
                    m_Image.SetActive(false);
                }
                break;
        }    
    }

    public void OnClick()
    {
        switch (type)
        {
            case "Clothes":
                if(RuntimeStorageData.PLAYER.characters_bought.Contains(id))
                {
                    //da mua
                    RuntimeStorageData.PLAYER.character_using = id;
                    Game.Shop.Instance.ChooseSkinPreviewWithId(id);
                    Game.Shop.Instance.ReloadButton(type);
                }
                else
                {
                    //chua mua
                    FirebaseManager.Instance.ShowRewardUnlock();
                    IronSourceManager.Instance.ShowReward(() =>
                    {
                        RuntimeStorageData.PLAYER.character_using = id;
                        if (!RuntimeStorageData.PLAYER.characters_bought.Contains(id))
                            RuntimeStorageData.PLAYER.characters_bought.Add(id);
                        Game.Shop.Instance.ChooseSkinPreviewWithId(id);
                        Game.Shop.Instance.ReloadButton(type);
                    }, () =>
                    {

                    });
                }
                break;
            case "Hat":
                if (RuntimeStorageData.PLAYER.hats_bought.Contains(id))
                {
                    //da mua
                    RuntimeStorageData.PLAYER.hat_using = id;
                    Game.Shop.Instance.ChooseHatPreviewWithId(id);
                    Game.Shop.Instance.ReloadButton(type);
                }
                else
                {
                    //chua mua
                    FirebaseManager.Instance.ShowRewardUnlock();
                    IronSourceManager.Instance.ShowReward(() =>
                    {
                        RuntimeStorageData.PLAYER.hat_using = id;
                        if (!RuntimeStorageData.PLAYER.hats_bought.Contains(id))
                            RuntimeStorageData.PLAYER.hats_bought.Add(id);
                        Game.Shop.Instance.ChooseHatPreviewWithId(id);
                        Game.Shop.Instance.ReloadButton(type);
                    }, () =>
                    {

                    });
                }

                break;
            case "Hair":
                if (RuntimeStorageData.PLAYER.hairs_bought.Contains(id))
                {
                    //da mua
                    RuntimeStorageData.PLAYER.hair_using = id;
                    Game.Shop.Instance.ChooseHairPreviewWithId(id);
                    Game.Shop.Instance.ReloadButton(type);
                }
                else
                {
                    //chua mua
                    FirebaseManager.Instance.ShowRewardUnlock();
                    IronSourceManager.Instance.ShowReward(() =>
                    {
                        RuntimeStorageData.PLAYER.hair_using = id;
                        if (!RuntimeStorageData.PLAYER.hairs_bought.Contains(id))
                            RuntimeStorageData.PLAYER.hairs_bought.Add(id);
                        Game.Shop.Instance.ChooseHairPreviewWithId(id);
                        Game.Shop.Instance.ReloadButton(type);
                    }, () =>
                    {

                    });
                }
                break;
            case "Utility":
                if (RuntimeStorageData.PLAYER.utility_bought.Contains(id))
                {
                    //da mua
                    RuntimeStorageData.PLAYER.utility_using = id;
                    Game.Shop.Instance.ChooseUtilityPreviewWithId(id);
                    Game.Shop.Instance.ReloadButton(type);
                }
                else
                {
                    //chua mua
                    FirebaseManager.Instance.ShowRewardUnlock();
                    IronSourceManager.Instance.ShowReward(() =>
                    {
                        RuntimeStorageData.PLAYER.utility_using = id;
                        if (!RuntimeStorageData.PLAYER.utility_bought.Contains(id))
                            RuntimeStorageData.PLAYER.utility_bought.Add(id);
                        Game.Shop.Instance.ChooseUtilityPreviewWithId(id);
                        Game.Shop.Instance.ReloadButton(type);
                    }, () =>
                    {

                    });
                }
                break;
        }
    }
}
