using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSerializable
{
    public string id = "hihidochoo";
    public bool isAds = true;
    public int level = 1;
    public int coin = 0;

    public string m_SkinUsing = "";
    public int m_SkinColor = 0;
    public string m_SkinColorUsing = "";
    public List<ItemBought> m_SkinBoughts;

    public string m_HatUsing = "";
    public int m_HatColorUsing = 0;
    public List<ItemBought> m_HatBoughts;

    public string m_HairUsing = "";
    public int m_HairColorUsing = 0;
    public List<ItemBought> m_HairBoughts;

    public string m_FaceUsing = "";

    public string m_UtilityUsing = "";
    public List<string> m_UtilityBoughts;
}

[System.Serializable]
public class ItemBought
{
    public string id;
    public List<int> colors;

    public ItemBought()
    {
        colors = new List<int>();
    }

    public ItemBought(string _id, List<int> _colors)
    {
        id = _id;
        colors = _colors;
    }
}
