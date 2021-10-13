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

        //private Vector3 m_CameraOpenPositon = new Vector3(0f, 70f, -20f);
        //private Vector3 m_CameraOpenRotation = new Vector3(70f, 0f, 0f);
        //private Vector3 m_CameraDrawPosition = new Vector3(0f, 70f, 6.5f);
        //private Vector3 m_CameraDrawRotation = new Vector3(90f, 0f, 0f);
        //private Vector3 m_CameraPlayPosition = new Vector3(0f, 70f, -60f);
        //private Vector3 m_CameraPlayRotation = new Vector3(34f, 0f, 0f);

        [Header("Water")]
        public GameObject Water2DSpawn;

        [Header("Water Tank")]
        public DataTank[] dataTanks;
        protected override void Awake()
        {
            base.Awake();
            m_Level = ResourceManager.GetLevel(RuntimeStorageData.PLAYER.level);
            //m_Camera.orthographicSize = m_Level.LevelData.SizeCamera;
            m_AttackType = m_Level.LevelData.AttackType;
            m_WaterType = m_Level.LevelData.WaterType;

            m_SizeCamera = m_Level.LevelData.SizeCamera;


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
                }
            }

            if(m_Level.LevelData.AttackType == Enum.AttackType.Water)
            {
                Water2DSpawn.SetActive(true);
                Tank = ActiveWater(m_Level.LevelData.WaterType);   
            }

            StaticVariable.GameState = GameState.PAUSE;
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

            switch (waterType)
            {
                case Enum.WaterType.Top:
                    Physics.gravity = data.waterGravity;
                    break;
                case Enum.WaterType.Bottom:
                    Physics.gravity = data.waterGravity;
                    break;
            }

            return data;
        }

        //public void ChangePositionCamera(CameraType type, bool IsLerp, System.Action Complete)
        //{
        //    if (IsLerp)
        //    {

        //    }
        //    else
        //    {
        //        switch (type)
        //        {
        //            case CameraType.PLAY:
        //                m_Camera.transform.position = m_CameraOpenPositon;
        //                m_Camera.transform.eulerAngles = m_CameraOpenRotation;
        //                break;
        //            case CameraType.DRAW:
        //                m_Camera.transform.position = m_CameraDrawPosition;
        //                m_Camera.transform.eulerAngles = m_CameraDrawRotation;
        //                break;
        //            case CameraType.START:
        //                m_Camera.transform.position = m_CameraPlayPosition;
        //                m_Camera.transform.eulerAngles = m_CameraPlayRotation;
        //                break;
        //        }
        //    }
        //}

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

            Debug.Log("Victory");
            for(int i = 0; i < Characters.Count; i++)
            {
                var script = Characters[i].GetComponent<Character>();
                script.ShowVictory();
            }

            Game.UIManager.Instance.ShowVictoryUI();
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
    }
}
