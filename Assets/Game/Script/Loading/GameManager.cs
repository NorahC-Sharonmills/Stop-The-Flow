using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public enum PopupStatus
    {
        Show,
        Hide
    }

    public class GameManager : MonoSingletonGlobal<GameManager>
    { 
        public static void LoadScene(string scene, bool isLoading = false, bool isFade = false)
        {
            IsFade = isFade;
            if(IsFade)
            {
                Game.Fade.Instance.ShowFade();
                CoroutineUtils.PlayCoroutine(() =>
                {
                    SceneManager.LoadScene(scene, LoadSceneMode.Single);
                }, 0.2f);
            }
            else
            {
                SceneManager.LoadScene(scene, LoadSceneMode.Single);
            }

        }

        public static PopupStatus PopupStatus = PopupStatus.Hide;
        public static bool IsFade = false;

        private void OnApplicationQuit()
        {
            RuntimeStorageData.SaveAllData();
        }

        private void OnApplicationPause(bool IsPause)
        {
            if (IsPause)
                RuntimeStorageData.SaveAllData();
        }
    }
}

