using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shop", menuName = "Data/Shop", order = 1)]
public class ShopScriptableObject : ScriptableObject
{
    public GameObject[] m_PrefabOutfits;
    public GameObject[] m_PrefabHats;
    public GameObject[] m_PrefabHairs;
    public GameObject[] m_PrefabsUtilitys;
}
