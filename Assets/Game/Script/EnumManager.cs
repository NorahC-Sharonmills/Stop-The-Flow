using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enum
{
    public enum ObjectType
    {
        None,
        Character,
        Enemy,
        Object,
        Clear
    }

    public enum CharacterType
    {
        None,
        Human,
        Tresual,
        Bulding,
        Animal
    }

    public enum AttackType
    {
        Water,
        Enemy
    }

    public enum EnemyType
    {
        None,
        Animal,
        Goblin,
        Weapon
    }

    public enum WaterType
    {
        Top,
        Bottom,
        Left,
        Right
    }
}

