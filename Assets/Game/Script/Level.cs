using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class Level
    {
        public LevelData LevelData;
    }

    [System.Serializable]
    public class LevelData
    {
        public float SizeCamera;
        public Enum.AttackType AttackType;
        public Enum.WaterType WaterType;
        public List<ObjectData> Datas;
    }

    [System.Serializable]
    public class ObjectData
    {
        public Enum.ObjectType ObjectType;
        public Enum.CharacterType CharacterType;
        public Enum.EnemyType EnemyType;
        public string NameObject;
        public bool IsEnable = true;
        public Vector3 Postion;
        public Quaternion Rotation;
        public Vector3 LocalScale;
    }
}
