using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Game
{
    public class ResourceManager : MonoSingletonGlobal<ResourceManager>
    {
        [SerializeField]
        private TextAsset[] levels;

        public TextAsset GetLevelInTool(int level)
        {
            return levels[level];
        }    

        [SerializeField]
        private ShopScriptableObject shopInfo;

        public ShopScriptableObject ShopInfo => shopInfo;

        public GameObject GetClothesWithId(string id)
        {
            GameObject rs = null;
            for(int i = 0; i < shopInfo.m_PrefabOutfits.Length; i++)
            {
                if(shopInfo.m_PrefabOutfits[i].name == id)
                {
                    rs = shopInfo.m_PrefabOutfits[i];
                }
            }

            return rs;
        }

        public GameObject GetFaceWithId(string id)
        {
            GameObject rs = null;
            for (int i = 0; i < shopInfo.m_PrefabsFaces.Length; i++)
            {
                if (shopInfo.m_PrefabsFaces[i].name == id)
                {
                    rs = shopInfo.m_PrefabsFaces[i];
                }
            }

            return rs;
        }    

        public GameObject GetUtilityWithId(string id)
        {
            GameObject rs = null;
            for (int i = 0; i < shopInfo.m_PrefabsFaces.Length; i++)
            {
                if (shopInfo.m_PrefabsUtilitys[i].name == id)
                {
                    rs = shopInfo.m_PrefabsUtilitys[i];
                }
            }

            return rs;
        }

        public GameObject GetHairWithId(string id)
        {
            GameObject rs = null;
            for (int i = 0; i < shopInfo.m_PrefabHairs.Length; i++)
            {
                if (shopInfo.m_PrefabHairs[i].name == id)
                {
                    rs = shopInfo.m_PrefabHairs[i];
                }
            }

            return rs;
        }

        public GameObject GetHatWithId(string id)
        {
            GameObject rs = null;
            for (int i = 0; i < shopInfo.m_PrefabHats.Length; i++)
            {
                if (shopInfo.m_PrefabHats[i].name == id)
                {
                    rs = shopInfo.m_PrefabHats[i];
                }
            }

            return rs;
        }

        private Dictionary<int, Level> LevelDics = new Dictionary<int, Level>();
        private Dictionary<string, GameObject> PrefabDics = new Dictionary<string, GameObject>();

        protected override void Awake()
        {
            base.Awake();

        }

        public IEnumerator InitializedResource()
        {
            for (int i = 0; i < levels.Length; i++)
            {
                var str = levels[i].text;
                var level = JsonUtility.FromJson<Level>(str);

                LevelDics.Add(i + 1, level);
            }
            yield return null;
        }

        public static Level GetLevel(int level)
        {
            if(Instance.LevelDics.ContainsKey(level))
            {
                return Instance.LevelDics[level];
            }
            return Instance.LevelDics.Values.Last();
        }

        public static GameObject GetPrefabs(ObjectData objects)
        {
            string path = "";
            GameObject obj = null;

            if (objects.NameObject.Contains("Character"))
                objects.NameObject = "Character";

            if (Instance.PrefabDics.ContainsKey(objects.NameObject))
            {
                obj = Instance.PrefabDics[objects.NameObject];
            }
            else
            {
                switch (objects.ObjectType)
                {
                    case Enum.ObjectType.None:
                        path = string.Format("Prefabs/{0}", objects.NameObject);
                        break;
                    case Enum.ObjectType.Character:
                        path = string.Format("Prefabs/Character/{0}", objects.NameObject);
                        break;
                    case Enum.ObjectType.Enemy:
                        path = string.Format("Prefabs/Enemy/{0}", objects.NameObject);
                        break;
                    case Enum.ObjectType.Object:
                        path = string.Format("Prefabs/Object/{0}", objects.NameObject);
                        break;
                    case Enum.ObjectType.Clear:
                        path = string.Format("Prefabs/Clear/{0}", objects.NameObject);
                        break;
                }

                obj = Resources.Load<GameObject>(path);
                Instance.PrefabDics.Add(objects.NameObject, obj);
            }
            return obj;
        }
    }
}

