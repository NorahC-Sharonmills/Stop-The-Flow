﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSerializable
{
    public string id = "hihidochoo";
    public bool isAds = true;
    public int level = 1;
    public int coin = 0;
    public string character_using = "";
    public int character_color_using = 0;
    public List<string> characters_bought;
    public string hat_using = "";
    public List<string> hats_bought;
    public string hair_using = "";
    public List<string> hairs_bought;
    public string utility_using = "";
    public List<string> utility_bought;

}
