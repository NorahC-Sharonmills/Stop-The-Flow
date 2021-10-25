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
    public Color[] m_HatColors;
    public Material[] m_MaterialHatColors;
    [Header("Hair")]
    public GameObject[] m_PrefabHairs;
    public Color[] m_HairColors;
    public Material[] m_MaterialHairColors;
    [Header("Face")]
    public GameObject[] m_PrefabsFaces;
    public Color[] m_FaceColors;
    public Material[] m_MaterialFaceColors;
    [Header("Utility")]
    public GameObject[] m_PrefabsUtilitys;
    public PositionUtility[] m_UtilityPositions;
}

public enum PositionUtility
{
    Back,
    Hand
}
