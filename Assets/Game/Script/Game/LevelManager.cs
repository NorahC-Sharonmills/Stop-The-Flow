using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        [SerializeField] private Camera m_Camera;
        [Header("Hiện lên để xem thông tin level")]
        [SerializeField] private Level m_Level;
        [Header("Hiện thông tin attack")]
        public Enum.AttackType m_AttackType;
        public Enum.WaterType m_WaterType;
        protected override void Awake()
        {
            base.Awake();
            m_Level = ResourceManager.GetLevel(RuntimeStorageData.PLAYER.level);
            m_Camera.orthographicSize = m_Level.LevelData.SizeCamera;
            m_AttackType = m_Level.LevelData.AttackType;
            m_WaterType = m_Level.LevelData.WaterType;


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
            }

            StaticVariable.GameState = GameState.DRAW;
        }
    }
}
