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

            if(Instance.PrefabDics.ContainsKey(objects.NameObject))
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

