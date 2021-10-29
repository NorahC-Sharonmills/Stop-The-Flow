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
                if(RuntimeStorageData.PLAYER.m_SkinBoughts.Exists(x => x.id == id))
                {
                    if(RuntimeStorageData.PLAYER.m_SkinUsing == id)
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
                if (RuntimeStorageData.PLAYER.m_HatBoughts.Exists(x => x.id == id))
                {
                    if (RuntimeStorageData.PLAYER.m_HatUsing == id)
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
                if (RuntimeStorageData.PLAYER.m_HairBoughts.Exists(x => x.id == id))
                {
                    if (RuntimeStorageData.PLAYER.m_HairUsing == id)
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
                if (RuntimeStorageData.PLAYER.m_UtilityBoughts.Contains(id))
                {
                    if (RuntimeStorageData.PLAYER.m_UtilityUsing == id)
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

        Debug.Log(string.Format("--- click type {0} id {1}", type, id));
        switch (type)
        {
            case "Clothes":
                if(RuntimeStorageData.PLAYER.m_SkinBoughts.Exists(x => x.id == id))
                {
                    //da mua
                    RuntimeStorageData.PLAYER.m_SkinUsing = id;
                    Game.Shop.Instance.ChooseSkinPreviewWithId(id);
                    Game.Shop.Instance.ReloadButton(type);
                }
                else
                {
                    //chua mua
                    FirebaseManager.Instance.ShowRewardUnlock();
                    IronSourceManager.Instance.ShowReward(() =>
                    {
                        RuntimeStorageData.PLAYER.m_SkinUsing = id;
                        //if (!RuntimeStorageData.PLAYER.characters_bought.Contains(id))
                        //    RuntimeStorageData.PLAYER.characters_bought.Add(id);
                        ItemBought _skin = new ItemBought();
                        _skin.id = id;
                        _skin.colors.Add(0);
                        RuntimeStorageData.PLAYER.m_SkinBoughts.Add(_skin);
                        Game.Shop.Instance.ChooseSkinPreviewWithId(id);
                        Game.Shop.Instance.ReloadButton(type);
                    }, () =>
                    {

                    });
                }
                break;
            case "Hat":
                if (RuntimeStorageData.PLAYER.m_HatBoughts.Exists(x => x.id == id))
                {
                    //da mua
                    RuntimeStorageData.PLAYER.m_HatUsing = id;
                    Game.Shop.Instance.ChooseHatPreviewWithId(id);
                    Game.Shop.Instance.ReloadButton(type);
                }
                else
                {
                    //chua mua
                    FirebaseManager.Instance.ShowRewardUnlock();
                    IronSourceManager.Instance.ShowReward(() =>
                    {
                        RuntimeStorageData.PLAYER.m_HatUsing = id;
                        //if (!RuntimeStorageData.PLAYER.hats_bought.Contains(id))
                        //    RuntimeStorageData.PLAYER.hats_bought.Add(id);
                        ItemBought _hat = new ItemBought();
                        _hat.id = id;
                        _hat.colors.Add(0);
                        RuntimeStorageData.PLAYER.m_HatBoughts.Add(_hat);
                        Game.Shop.Instance.ChooseHatPreviewWithId(id);
                        Game.Shop.Instance.ReloadButton(type);
                    }, () =>
                    {

                    });
                }

                break;
            case "Hair":
                if (RuntimeStorageData.PLAYER.m_HairBoughts.Exists(x => x.id == id))
                {
                    //da mua
                    RuntimeStorageData.PLAYER.m_HairUsing = id;
                    Game.Shop.Instance.ChooseHairPreviewWithId(id);
                    Game.Shop.Instance.ReloadButton(type);
                }
                else
                {
                    //chua mua
                    FirebaseManager.Instance.ShowRewardUnlock();
                    IronSourceManager.Instance.ShowReward(() =>
                    {
                        RuntimeStorageData.PLAYER.m_HairUsing = id;
                        //if (!RuntimeStorageData.PLAYER.hairs_bought.Contains(id))
                        //    RuntimeStorageData.PLAYER.hairs_bought.Add(id);
                        ItemBought _hair = new ItemBought();
                        _hair.id = id;
                        _hair.colors.Add(0);
                        RuntimeStorageData.PLAYER.m_HairBoughts.Add(_hair);
                        Game.Shop.Instance.ChooseHairPreviewWithId(id);
                        Game.Shop.Instance.ReloadButton(type);
                    }, () =>
                    {

                    });
                }
                break;
            case "Utility":
                if (RuntimeStorageData.PLAYER.m_UtilityBoughts.Contains(id))
                {
                    //da mua
                    RuntimeStorageData.PLAYER.m_UtilityUsing = id;
                    Game.Shop.Instance.ChooseUtilityPreviewWithId(id);
                    Game.Shop.Instance.ReloadButton(type);
                }
                else
                {
                    //chua mua
                    FirebaseManager.Instance.ShowRewardUnlock();
                    IronSourceManager.Instance.ShowReward(() =>
                    {
                        RuntimeStorageData.PLAYER.m_UtilityUsing = id;
                        if (!RuntimeStorageData.PLAYER.m_UtilityBoughts.Contains(id))
                            RuntimeStorageData.PLAYER.m_UtilityBoughts.Add(id);
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
