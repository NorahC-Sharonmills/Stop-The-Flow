using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Setting : MonoSingletonGlobal<Setting>
    {
        public Sprite m_EnableSprite;
        public Sprite m_DisableSprite;
        public Color m_ShadowEnable;
        public Color m_ShadowDisable;
        public Sprite m_VibrateActive;
        public Sprite m_VibrateDisable;
        public Image m_IconVibrate;
        public Shadow m_ShadowVibrate;
        public Sprite m_SoundActive;
        public Sprite m_SoundDisable;
        public Image m_IconSound;
        public Shadow m_ShadowSound;
        [Header("Button")]
        public Image m_ImageVibrate;
        public Image m_ImageSound;

        [Header("Show")]
        public GameObject m_Canvas;
        public GameObject m_PanelSetting;
        public GameObject m_ConnectionError;
        
        public void Initializeded()
        {

        }

        public void ConnectionError()
        {
            m_ConnectionError.SetActive(true);
            m_Canvas.SetActive(true);
        }

        public void OnShow()
        {
            if (RuntimeStorageData.SOUND.isVibrate)
            {
                m_ImageVibrate.sprite = m_EnableSprite;
                m_IconVibrate.sprite = m_VibrateActive;
                m_ShadowVibrate.effectColor = m_ShadowEnable;
            }
            else
            {
                m_ImageVibrate.sprite = m_DisableSprite;
                m_IconVibrate.sprite = m_VibrateDisable;
                m_ShadowVibrate.effectColor = m_ShadowDisable;
            }

            if (RuntimeStorageData.SOUND.isSound)
            {
                m_ImageSound.sprite = m_EnableSprite;
                m_IconSound.sprite = m_SoundActive;
                m_ShadowSound.effectColor = m_ShadowEnable;
            }
            else
            {
                m_ImageSound.sprite = m_DisableSprite;
                m_ShadowSound.effectColor = m_ShadowDisable;
            }

            m_Canvas.SetActive(true);
            m_PanelSetting.SetActive(true);

            Game.GameManager.PopupStatus = PopupStatus.Show;
        }

        public void OnHide()
        {
            Game.GameManager.PopupStatus = PopupStatus.Hide;

            m_Canvas.SetActive(false);
            m_PanelSetting.SetActive(false);
        }

        public void OnClick(string id)
        {
            switch(id)
            {
                case "vibrate":
                    RuntimeStorageData.SOUND.isVibrate = !RuntimeStorageData.SOUND.isVibrate;
                    if (RuntimeStorageData.SOUND.isVibrate)
                    {
                        m_ImageVibrate.sprite = m_EnableSprite;
                        m_IconVibrate.sprite = m_VibrateActive;
                        m_ShadowVibrate.effectColor = m_ShadowEnable;
                    }
                    else
                    {
                        m_ImageVibrate.sprite = m_DisableSprite;
                        m_IconVibrate.sprite = m_VibrateDisable;
                        m_ShadowVibrate.effectColor = m_ShadowDisable;
                    }
                    break;
                case "sound":
                    SoundManager.Instance.isEnable = !SoundManager.Instance.isEnable;
                    if (RuntimeStorageData.SOUND.isSound)
                    {
                        m_ImageSound.sprite = m_EnableSprite;
                        m_IconSound.sprite = m_SoundActive;
                        m_ShadowSound.effectColor = m_ShadowEnable;
                    }
                    else
                    {
                        m_ImageSound.sprite = m_DisableSprite;
                        m_IconSound.sprite = m_SoundDisable;
                        m_ShadowSound.effectColor = m_ShadowDisable;
                    }
                    break;
                case "restore_purchase":
                    break;
            }
        }
    }
}

