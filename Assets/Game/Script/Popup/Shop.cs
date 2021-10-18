using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Shop : MonoSingletonGlobal<Shop>
    {
        public GameObject m_Canvas;
        public GameObject m_ShopObject;
        public Animator m_Animator;

        bool IsShow = false;
        public void Show()
        {
            m_Canvas.SetActive(true);
            m_ShopObject.SetActive(true);
            m_Animator.Play("Show");

            IsShow = true;
        }

        public void Home()
        {
            IsShow = false;
            m_Animator.Play("Hide");
            CoroutineUtils.PlayCoroutine(() =>
            {
                m_Canvas.SetActive(false);
                m_ShopObject.SetActive(false);
                Game.UIManager.Instance.Home();
            }, 0.2f);
        }

        //private void Update()
        //{
        //     if(IsShow && !m_ShopObject.activeInHierarchy)
        //    {
        //        Show();
        //    }    

        //    if(!IsShow && m_ShopObject.activeInHierarchy)
        //    {
        //        IsShow = false;
        //        Home();
        //    }    
        //}
    }
}
