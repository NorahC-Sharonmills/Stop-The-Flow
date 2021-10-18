using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ShopManager : MonoBehaviour
    {
        public ShopCharacter[] Characters;

        public void Awake()
        {
            Load();
        }

        public void Load()
        {
            for (int i = 0; i < Characters.Length; i++)
            {
                if (Characters[i].name == RuntimeStorageData.PLAYER.character_using)
                {
                    Characters[i].gameObject.SetActive(true);
                }
                else
                {
                    Characters[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
