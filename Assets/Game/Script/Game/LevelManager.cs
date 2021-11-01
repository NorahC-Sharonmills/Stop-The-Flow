using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        public enum CameraType
        {
            START,
            DRAW,
            PLAY
        }

        [System.Serializable]
        public struct DataTank
        {
            public Enum.WaterType waterType;
            public GameObject waterTank;
            public GameObject waterFall;
            public Vector3 waterGravity;
            public Transform[] waterPosition;
        }

        [HideInInspector] public float m_SizeCamera = 6.95f;
        //[SerializeField] private Camera m_Camera;
        [Header("Hiện lên để xem thông tin level")]
        [SerializeField] private Level m_Level;
        [Header("Hiện thông tin attack")]
        public Enum.AttackType m_AttackType;
        public Enum.WaterType m_WaterType;

        [HideInInspector]
        public List<GameObject> Characters = new List<GameObject>();
        [HideInInspector]
        public List<GameObject> Enemys = new List<GameObject>();

        [Header("Water")]
        public GameObject Water2DSpawn;

        [Header("Water Tank")]
        public DataTank[] dataTanks;

        private bool isCustomCharacter = false;
        public bool IsCustomCharacter => isCustomCharacter;

        protected override void Awake()
        {
            base.Awake();
            m_Level = ResourceManager.GetLevel(RuntimeStorageData.PLAYER.level);
            //m_Camera.orthographicSize = m_Level.LevelData.SizeCamera;
            m_AttackType = m_Level.LevelData.AttackType;
            m_WaterType = m_Level.LevelData.WaterType;
            m_SizeCamera = m_Level.LevelData.SizeCamera;

            switch(RuntimeStorageData.PLAYER.level)
            {
                case 50:
                    isCustomCharacter = false;
                    break;
                default:
                    isCustomCharacter = true;
                    break;
            }    


            var objects = m_Level.LevelData.Datas;
            for(int i = 0; i < objects.Count; i++)
            {
                if (!objects[i].IsEnable)
                    continue;

                var _object = Instantiate(ResourceManager.GetPrefabs(objects[i])) as GameObject;
                _object.transform.position = objects[i].Postion;
                _object.transform.rotation = objects[i].Rotation;
                _object.transform.localScale = objects[i].LocalScale;
                _object.name = _object.name.Replace("(Clone)", "");
                _object.SetActive(true);

                if (objects[i].ObjectType == Enum.ObjectType.Character)
                {
                    _object.name = "character";
                    Characters.Add(_object);
                    if(objects[i].CharacterType == Enum.CharacterType.Human)
                    {
                        Vector3 pos = _object.transform.position;
                        pos.x += 0.65f;
                        pos.y = 2.3f;
                        Vector3 rot = Vector3.right * 25f;
                        GameObject fx = PoolByID.Instance.GetPrefab(Game.ResourceManager.Instance.m_EffectHelp, pos, Quaternion.identity, null);
                        fx.transform.eulerAngles = rot;
                        helps.Add(fx);
                    }    
                }
                if (objects[i].ObjectType == Enum.ObjectType.Enemy)
                {
                    Enemys.Add(_object);
                }
            }

            if(m_Level.LevelData.AttackType == Enum.AttackType.Water)
            {
                Water2DSpawn.SetActive(true);
                Tank = ActiveWater(m_Level.LevelData.WaterType);   
            }

            if(RuntimeStorageData.PLAYER.level == 3 && PlayerPrefs.GetInt("Key_RateUs", 0) == 0)
            {
                Game.Rate.Instance.Show();
                PlayerPrefs.SetInt("Key_RateUs", 1);
            }

            if(RuntimeStorageData.PLAYER.level == 1 && PlayerPrefs.GetInt("Key_Tut", 0) == 0)
            {
                Tutorial = Instantiate(Game.ResourceManager.Instance.m_Tutorial);
                Tutorial.transform.position = new Vector3(0f, 0f, 9f);
                Tutorial.transform.eulerAngles = new Vector3(90f, 0f, 0f);
                Tutorial.name = Tutorial.name.Replace("(Clone)", "");
                Tutorial.gameObject.SetActive(false);
                IsTutorial = true;
            }

            FirebaseManager.Instance.LevelPlay();

            StaticVariable.GameState = GameState.PAUSE;
        }

        private void Start()
        {
            Game.Loading.Instance.Hide();
        }

        private GameObject Tutorial;
        private bool IsTutorial = false;
        private List<GameObject> helps = new List<GameObject>();
        public void HideHelpIcon()
        {
            for(int i = 0; i < helps.Count; i++)
            {
                PoolByID.Instance.PushToPool(helps[i]);
            }    
        }

        public void ActiveTutorial()
        {
            if (IsTutorial)
            {
                Tutorial.gameObject.SetActive(true);
            }
        }

        public void HideTutorial()
        {
            if(IsTutorial)
            {
                IsTutorial = false;
                PlayerPrefs.SetInt("Key_Tut", 1);
                Tutorial.gameObject.SetActive(false);
            }
        }    

        [HideInInspector] public DataTank Tank;
        private DataTank ActiveWater(Enum.WaterType waterType)
        {
            DataTank data = dataTanks[0];

            for (int i = 0; i < dataTanks.Length; i++)
            {
                if (dataTanks[i].waterType == waterType)
                {
                    data = dataTanks[i];
                    data.waterTank.SetActive(true);
                }
            }

            Physics.gravity = data.waterGravity;
            return data;
        }

        private void Update()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Game.Setting.Instance.ConnectionError();
            }
        }

        [HideInInspector] public bool IsVictory = false;
        [HideInInspector] public bool IsLose = false;

        public void OnVictory()
        {
            if (IsLose)
                return;
            if (IsVictory)
                return;

            IsVictory = true;
            IsLose = false;

            Game.GameManager.Instance.Victory(() =>
            {
                SoundManager.Instance.PlayOnShot(Sound.WIN);
                for (int i = 0; i < Characters.Count; i++)
                {
                    var script = Characters[i].GetComponent<Character>();
                    script.ShowVictory();
                }

                Game.UIManager.Instance.ShowVictoryUI();
            }, () =>
            {
                SoundManager.Instance.PlayOnShot(Sound.WIN);
                for (int i = 0; i < Characters.Count; i++)
                {
                    var script = Characters[i].GetComponent<Character>();
                    script.ShowVictory();
                }

                Game.UIManager.Instance.ShowVictoryUI();
            });
        }

        public void OnLose()
        {
            if (IsLose)
                return;
            if (IsVictory)
                return;

            IsLose = true;
            IsVictory = false;
            Game.UIManager.Instance.ShowLoseUI();
        }

        private bool IsSound = false;
        public void PlaySound(Sound sound)
        {
            if (IsSound)
                return;
            IsSound = true;
            SoundManager.Instance.PlayOnShot(sound);
        }    
    }
}
