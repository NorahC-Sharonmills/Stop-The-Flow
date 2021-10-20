using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shop", menuName = "Data/Shop", order = 1)]
public class ShopScriptableObject : ScriptableObject
{
    [Header("Outfits")]
    public GameObject[] m_PrefabOutfits;
    public Color[] m_OutfitsColors;
    public Material[] m_MaterialWhiteOutfitsColors;
    public Material[] m_MaterialBlackOutfitsColors;
    [Header("Hats")]
    public GameObject[] m_PrefabHats;
    public GameObject[] m_PrefabHairs;
    public GameObject[] m_PrefabsUtilitys;
}
