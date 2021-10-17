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
        public static void LoadScene(string scene, bool isLoading = false)
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }

        public static PopupStatus PopupStatus = PopupStatus.Hide;

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

