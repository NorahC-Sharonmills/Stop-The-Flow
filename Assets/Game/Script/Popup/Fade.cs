using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Fade : MonoSingletonGlobal<Fade>
    {
        public GameObject m_Canvas;
        public GameObject m_FadeObject;
        public Animator m_Animator;

        private float FadeIn_Time = 0;
        private float FadeOut_Time = 0;
        protected override void Awake()
        {
            base.Awake();
            AnimationClip[] clips = m_Animator.runtimeAnimatorController.animationClips;
            for (int i = 0; i < clips.Length; i++)
            {
                if(clips[i].name == "FadeIn")
                {
                    FadeIn_Time = clips[i].length;
                    continue;
                }
                if(clips[i].name == "FadeOut")
                {
                    FadeOut_Time = clips[i].length;
                    continue;
                }
            }
        }

        public void ShowFade()
        {
            m_Canvas.SetActive(true);
            m_FadeObject.SetActive(true);
            m_Animator.Play("FadeIn");
            CoroutineUtils.PlayCoroutine(() =>
            {
                m_Animator.Play("FadeOut");
                CoroutineUtils.PlayCoroutine(() =>
                {
                    m_Canvas.SetActive(false);
                    m_FadeObject.SetActive(false);
                }, FadeOut_Time);
            }, FadeIn_Time);
        }

        public void FadeIn(bool isAuto = false)
        {
            m_Canvas.SetActive(true);
            m_FadeObject.SetActive(true);
            m_Animator.Play("FadeIn");
            if(isAuto)
            {
                CoroutineUtils.PlayCoroutine(() =>
                {
                    m_Animator.Play("FadeOut");
                }, FadeIn_Time);
            }
        }    
        
        public void FadeOut(bool isAuto = false)
        {
            m_Canvas.SetActive(true);
            m_Animator.Play("FadeOut");
            if (isAuto)
            {
                CoroutineUtils.PlayCoroutine(() =>
                {
                    m_Animator.Play("FadeIn");
                }, FadeOut_Time);
            }
        }    
    }
}

