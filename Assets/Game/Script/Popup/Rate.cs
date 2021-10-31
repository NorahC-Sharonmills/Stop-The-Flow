using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Rate : MonoSingleton<Rate>
    {
        public GameObject m_Canvas;
        public GameObject m_RateObject;

        public bool IsRate
        {
            get { return m_RateObject.activeInHierarchy; }
        }

        [Header("Link")]
        public string IOSAppID = "";

        public void Show()
        {
            m_RateObject.SetActive(true);
            m_Canvas.SetActive(true);
        }

        public void Hide()
        {
            SoundManager.Instance.PlayOnShot(Sound.CLICK);
        }    

        public void RateUs()
        {
            SoundManager.Instance.PlayOnShot(Sound.CLICK);
#if UNITY_EDITOR
            Application.OpenURL($"https://play.google.com/store/apps/details?id=water.color.sort.puzzle.pour.game");
#elif UNITY_ANDROID
        Application.OpenURL($"market://details?id={Application.identifier}");
#elif UNITY_IOS
        Application.OpenURL($"http://itunes.apple.com/app/id{IOSAppID}");
#else
        Application.OpenURL($"market://details?id={Application.identifier}");
#endif
        }
    }
}
