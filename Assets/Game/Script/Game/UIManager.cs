using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class UIManager : MonoSingleton<UIManager>
    {
        public GameObject m_Tap;
        public GameObject m_Victory;

        public void ButtonNext()
        {
            RuntimeStorageData.PLAYER.level += 1;
            GameManager.LoadScene(SceneName.Game, true);
        }

        public void ButtonBack()
        {
            RuntimeStorageData.PLAYER.level -= 1;
            if (RuntimeStorageData.PLAYER.level < 1)
                RuntimeStorageData.PLAYER.level = 1;

            GameManager.LoadScene(SceneName.Game, true);
        }

        public void Replay()
        {
            GameManager.LoadScene(SceneName.Game, true);
        }

        protected override void Awake()
        {
            base.Awake();

            m_Tap.SetActive(true);
            m_Victory.SetActive(false);
        }

        public void TapToPlay()
        {
            m_Tap.SetActive(false);
            CameraController.Instance.MoveToDraw(() =>
            {
                StaticVariable.GameState = GameState.DRAW;
            });
        }

        public void ShowVictoryUI()
        {
            m_Victory.SetActive(true);
        }    
    }
}
