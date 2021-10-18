using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class SceneName
    {
        public static readonly string Loading = "0.Init";
        public static readonly string Game = "1.Game";
    }

    public static class PlayerPrefsString
    {
        public static readonly string IsFirst = "isfirst";
    }

    public class InitializedManager : MonoSingletonGlobal<InitializedManager>
    {
        private IEnumerator Start()
        {
            Debug.Log("Read all data");
            bool IsFirst = PlayerPrefs.GetInt(PlayerPrefsString.IsFirst, 0) == 0 ? true : false;
            if (IsFirst)
            {
                PlayerPrefs.SetInt(PlayerPrefsString.IsFirst, 1);
                RuntimeStorageData.ReadNewData();
            }
            else
            {
                RuntimeStorageData.ReadAllData();
            }
            // thêm các đoạn load các thứ vào đây
            yield return ResourceManager.Instance.InitializedResource();

            Game.Setting.Instance.Initializeded();
            Game.Shop.Instance.Initializeded();



            // kiểm tra xem data đã sẵn sàng chưa
            yield return new WaitUntil(() => RuntimeStorageData.IsReady);
            GameManager.LoadScene(SceneName.Game);
        }
    }
}
