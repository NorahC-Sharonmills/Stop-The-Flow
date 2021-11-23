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

        public RenderTexture[] textures;

        private int LoseValue = 0;
        private int VictoryValue = 0;
        public void Lose(System.Action success, System.Action fail)
        {
            LoseValue += 1;
            VictoryValue = 0;
            if(LoseValue < 3)
            {
                success?.Invoke();
            }
            else
            {
                LoseValue = 0;
                fail?.Invoke();
            }
        }

        public void Victory(System.Action success, System.Action fail)
        {
            LoseValue = 0;
            VictoryValue += 1;
            success?.Invoke();
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

