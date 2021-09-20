using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    [HideInInspector]
    public Camera Camera;
    [HideInInspector]
    public List<GameObject> Objects = new List<GameObject>();
    [HideInInspector]
    public Transform Center;
    [HideInInspector]
    public Enum.WaterType WaterType;
    [HideInInspector]
    public Enum.AttackType AttackType;
}
