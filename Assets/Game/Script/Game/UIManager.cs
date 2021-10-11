using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class UIManager : MonoSingleton<UIManager>
    {
        public GameObject m_TapUI;
        public GameObject m_VictoryUI;
        public GameObject m_LoseUI;

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

            m_TapUI.SetActive(true);
            m_VictoryUI.SetActive(false);
            m_LoseUI.SetActive(false);
        }

        public void TapToPlay()
        {
            m_TapUI.SetActive(false);
            CameraController.Instance.MoveToDraw(() =>
            {
                StaticVariable.GameState = GameState.DRAW;
            });
        }

        public void ShowVictoryUI()
        {
            m_VictoryUI.SetActive(true);
        }    

        public void ShowLoseUI()
        {
            m_LoseUI.SetActive(true);
        }    
    }
}
