using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Loading : MonoSingletonGlobal<Loading>
    {
        public GameObject m_Canvas;
        public GameObject m_Loading;

        public bool IsLoading
        {
            get
            {
                return m_Loading.activeInHierarchy;
            }
        }

        public void Hide()
        {
            if (!IsLoading)
                return;

            if (Game.Rate.Instance.IsRate)
            {
                CoroutineUtils.PlayCoroutine(() =>
                {
                    m_Loading.SetActive(false);
                }, 2f);
            }
            else
            {
                CoroutineUtils.PlayCoroutine(() =>
                {
                    m_Loading.SetActive(false);
                    m_Canvas.SetActive(false);
                }, 2f);
            }    
        }    
    }
}
