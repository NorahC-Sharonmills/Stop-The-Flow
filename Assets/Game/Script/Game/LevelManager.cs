﻿using System.Collections;
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

        [SerializeField] private Camera m_Camera;
        [Header("Hiện lên để xem thông tin level")]
        [SerializeField] private Level m_Level;
        [Header("Hiện thông tin attack")]
        public Enum.AttackType m_AttackType;
        public Enum.WaterType m_WaterType;

        [HideInInspector]
        public List<GameObject> Characters = new List<GameObject>();

        private Vector3 m_CameraOpenPositon = new Vector3(0f, 70f, -20f);
        private Vector3 m_CameraOpenRotation = new Vector3(70f, 0f, 0f);
        private Vector3 m_CameraDrawPosition = new Vector3(0f, 70f, 6.5f);
        private Vector3 m_CameraDrawRotation = new Vector3(90f, 0f, 0f);
        private Vector3 m_CameraPlayPosition = new Vector3(0f, 70f, -60f);
        private Vector3 m_CameraPlayRotation = new Vector3(34f, 0f, 0f);
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

                if (objects[i].ObjectType == Enum.ObjectType.Character)
                    Characters.Add(_object);
            }

            StaticVariable.GameState = GameState.PAUSE;
        }

        public void ChangePositionCamera(CameraType type, bool IsLerp, System.Action Complete)
        {
            if (IsLerp)
            {

            }
            else
            {
                switch (type)
                {
                    case CameraType.PLAY:
                        m_Camera.transform.position = m_CameraOpenPositon;
                        m_Camera.transform.eulerAngles = m_CameraOpenRotation;
                        break;
                    case CameraType.DRAW:
                        m_Camera.transform.position = m_CameraDrawPosition;
                        m_Camera.transform.eulerAngles = m_CameraDrawRotation;
                        break;
                    case CameraType.START:
                        m_Camera.transform.position = m_CameraPlayPosition;
                        m_Camera.transform.eulerAngles = m_CameraPlayRotation;
                        break;
                }
            }
        }
    }
}
