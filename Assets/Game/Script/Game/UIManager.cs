using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UIManager : MonoSingleton<UIManager>
    {
        public GameObject m_HomeUI;
        public GameObject m_VictoryUI;
        public GameObject m_LoseUI;
        public GameObject m_GameUI;
        public GameObject m_LevelUI;

        [Header("Camera")]
        public GameObject m_GameCamera;
        public GameObject m_ShopCamera;

        [Header("Particles")]
        public GameObject m_VictoryParticles;

        [Header("Text")]
        public Text m_LevelText;

        public void ButtonNext()
        {
            RuntimeStorageData.PLAYER.level += 1;
            GameManager.LoadScene(SceneName.Game, true, false);
        }

        public void ButtonBack()
        {
            RuntimeStorageData.PLAYER.level -= 1;
            if (RuntimeStorageData.PLAYER.level < 1)
                RuntimeStorageData.PLAYER.level = 1;

            GameManager.LoadScene(SceneName.Game, true, false);
        }

        public void Replay()
        {
            GameManager.LoadScene(SceneName.Game, true, true);
        }

        protected override void Awake()
        {
            base.Awake();

            m_LevelText.text = string.Format("Level {0}", RuntimeStorageData.PLAYER.level);

            m_HomeUI.SetActive(true);
            m_LevelUI.SetActive(true);

            m_GameCamera.SetActive(true);


            m_VictoryUI.SetActive(false);
            m_LoseUI.SetActive(false);
            m_GameUI.SetActive(false);

            m_ShopCamera.SetActive(false);
        }

        public void Home()
        {
            m_HomeUI.SetActive(true);
            m_LevelUI.SetActive(true);

            m_GameCamera.SetActive(true);


            m_VictoryUI.SetActive(false);
            m_LoseUI.SetActive(false);
            m_GameUI.SetActive(false);

            m_ShopCamera.SetActive(false);
        }

        public void TapToPlay()
        {
            if (Game.GameManager.PopupStatus == PopupStatus.Show)
                return;

            ShowGameUI();
            Game.LevelManager.Instance.HideHelpIcon();
            CameraController.Instance.MoveToDraw(() =>
            {
                StaticVariable.GameState = GameState.DRAW;
            });


        }

        public void ShowGameUI()
        {
            m_HomeUI.SetActive(false);
            m_GameUI.SetActive(true);
            m_LevelUI.SetActive(true);
        }

        public void ShowVictoryUI()
        {
            m_VictoryUI.SetActive(true);
            m_LevelUI.SetActive(false);
            m_VictoryParticles.SetActive(true);
        }    

        public void ShowLoseUI()
        {
            //m_LoseUI.SetActive(true);
            //m_LevelUI.SetActive(false);
            CoroutineUtils.PlayCoroutine(() =>
            {
                Replay();
            }, 3f);
        }    

        public void OpenShop()
        {
            m_GameCamera.SetActive(false);
            m_ShopCamera.SetActive(true);

            Game.Shop.Instance.Show();

            m_LevelUI.SetActive(false);
            m_HomeUI.SetActive(false);
        }

        public void OnSettingShow()
        {
            Game.Setting.Instance.OnShow();
        }
    }
}
