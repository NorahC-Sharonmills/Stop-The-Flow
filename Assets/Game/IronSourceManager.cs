using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronSourceManager : MonoSingletonGlobal<IronSourceManager>
{
//    private string ironsource_your_app_key = "";
//    private Coroutine cacheCoroutineInter;
//    private Coroutine cacheCoroutineReward;

//    private Action rewardSuccess;
//    private Action rewardFail;
//    private Action interSuccess;
//    private Action interFail;

//    void OnApplicationPause(bool isPaused)
//    {
//        IronSource.Agent.onApplicationPause(isPaused);
//    }

//    protected override void Awake()
//    {
//        base.Awake();
//    }

//    public IEnumerator InitializedIronsource()
//    {
//#if UNITY_ANDROID || UNITY_EDITOR
//        ironsource_your_app_key = "10a3fec7d";
//        Debug.Log(ironsource_your_app_key + " android");
//#endif
//#if UNITY_IOS
//        ironsource_your_app_key = "1101a6edd";
//        Debug.Log(ironsource_your_app_key + " ios:");
//#endif

//        //For Rewarded Video
//        IronSource.Agent.init(ironsource_your_app_key);
//        IronSource.Agent.shouldTrackNetworkState(true);
//        IronSource.Agent.validateIntegration();
//        IronSource.Agent.loadInterstitial();

//        yield return null;
//        AddListenInter();
//        AddListenRewardVideo();

//        yield return null;
//        LoadBanner();
//        LoadInter();
//        LoadReward();
//    }

//    private void AddListenInter()
//    {
//        IronSourceEvents.onInterstitialAdReadyEvent += () =>
//        {
//            Debug.Log("onInterstitialAdReadyEvent");
//        };
//        IronSourceEvents.onInterstitialAdLoadFailedEvent += (IronSourceError) =>
//        {
//            Debug.Log("onInterstitialAdLoadFailedEvent");
//        };
//        IronSourceEvents.onInterstitialAdShowSucceededEvent += () =>
//        {
//            Debug.Log("onInterstitialAdShowSucceededEvent");
//        };
//        IronSourceEvents.onInterstitialAdShowFailedEvent += (IronSourceError) =>
//        {
//            Debug.Log("onInterstitialAdShowFailedEvent");
//            interFail?.Invoke();
//            LoadInter();
//        };
//        IronSourceEvents.onInterstitialAdClickedEvent += () =>
//        {
//            Debug.Log("onInterstitialAdClickedEvent");
//        };
//        IronSourceEvents.onInterstitialAdOpenedEvent += () =>
//        {
//            Debug.Log("onInterstitialAdOpenedEvent");
//        };
//        IronSourceEvents.onInterstitialAdClosedEvent += () =>
//        {
//            Debug.Log("onInterstitialAdClosedEvent");
//            interSuccess?.Invoke();
//            LoadInter();
//#if UNITY_IOS
//            Time.timeScale = 1;
//            AudioListener.pause = false;
//            IronSource.Agent.onApplicationPause(false);
//#endif
//        };
//    }

//    private bool isReward = false;
//    private void AddListenRewardVideo()
//    {
//        IronSourceEvents.onRewardedVideoAdOpenedEvent += () =>
//        {
//            Debug.Log("onRewardedVideoAdOpenedEvent");
//        };
//        IronSourceEvents.onRewardedVideoAdClickedEvent += (IronSourcePlacement) =>
//        {
//            Debug.Log("onRewardedVideoAdClickedEvent");
//        };
//        IronSourceEvents.onRewardedVideoAdClosedEvent += () =>
//        {
//            Debug.Log("onRewardedVideoAdClosedEvent");
//            if(isReward)
//            {
//#if UNITY_IOS
//                Time.timeScale = 1;
//                AudioListener.pause = false;
//                IronSource.Agent.onApplicationPause(false);
//#endif
//                rewardSuccess?.Invoke();
//                LoadReward();
//            }
//            else
//            {
//                rewardFail?.Invoke();
//                LoadReward();
//            }    
//        };
//        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += (isChange) =>
//        {
//            Debug.Log("onRewardedVideoAvailabilityChangedEvent " + isChange);
//            if (!isChange)
//            {
//                LoadReward();
//            }
//        };
//        IronSourceEvents.onRewardedVideoAdStartedEvent += () =>
//        {
//            Debug.Log("onRewardedVideoAdStartedEvent");
//        };
//        IronSourceEvents.onRewardedVideoAdEndedEvent += () =>
//        {
//            Debug.Log("onRewardedVideoAdEndedEvent");
//        };
//        IronSourceEvents.onRewardedVideoAdRewardedEvent += (IronSourcePlacement) =>
//        {
//            Debug.Log("onRewardedVideoAdRewardedEvent");
//            isReward = true;
//        };
//        IronSourceEvents.onRewardedVideoAdShowFailedEvent += (IronSourceError) =>
//        {
//            Debug.Log("onRewardedVideoAdShowFailedEvent");
//            rewardFail?.Invoke();
//            LoadReward();
//        };
//    }

//    private IEnumerator CorotineLoadReward()
//    {
//        yield return WaitForSecondCache.WAIT_TIME_ZERO_POINT_ONE;
//        bool available = IronSource.Agent.isRewardedVideoAvailable();
//        Debug.Log("isRewardedVideoAvailable " + available);
//    }

//    private IEnumerator CorotineLoadInter()
//    {
//        yield return WaitForSecondCache.WAIT_TIME_ZERO_POINT_ONE;
//        IronSource.Agent.loadInterstitial();
//        yield return WaitForSecondCache.WAIT_TIME_HAFT;
//        bool available = IronSource.Agent.isInterstitialReady();
//        Debug.Log("isInterstitialReady " + available);
//    }    

//    private void LoadInter()
//    {
//        Debug.Log("Load Inter");
//        if (cacheCoroutineInter != null)
//            StopCoroutine(cacheCoroutineInter);
//        cacheCoroutineInter = StartCoroutine(CorotineLoadInter());
//    }

//    private void LoadReward()
//    {
//        Debug.Log("Load Reward");
//        if (cacheCoroutineReward != null)
//            StopCoroutine(cacheCoroutineReward);
//        cacheCoroutineReward = StartCoroutine(CorotineLoadReward());

//    }

//    private void LoadBanner()
//    {
//        Debug.Log("Load Banner");
//        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
//        //BannerHide();
//        IronSourceEvents.onBannerAdLoadFailedEvent += (IronSourceError) =>
//        {
//            CoroutineUtils.PlayCoroutine(() =>
//            {
//                Debug.Log("Load Banner");
//                IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
//                BannerShow();
//            }, 0.5f);
//        };

//        BannerShow();
//    }

//    public void BannerShow()
//    {
//        if(RuntimeStorageData.PLAYER.isAds)
//        {
//            Debug.Log("show banner");
//            IronSource.Agent.displayBanner();
//        }   
//        else
//        {
//            BannerHide();
//        }
//    }

//    public void BannerHide()
//    {
//        Debug.Log("hide banner");
//        IronSource.Agent.hideBanner();
//    }

//    public bool m_SetAction = true;
    
//    public void ShowInter(Action success, Action fail)
//    {
//        //show ads inter show level 3;
//        if(RuntimeStorageData.PLAYER.level <= 3)
//        {
//            success?.Invoke();
//            return;
//        }

//        if(RuntimeStorageData.PLAYER.isAds)
//        {
//#if UNITY_EDITOR
//            if (m_SetAction)
//            {
//                success?.Invoke();
//            }
//            else
//            {
//                fail?.Invoke();
//            }
//#elif UNITY_IOS
//            Time.timeScale = 0;
//            AudioListener.pause = true;
//            IronSource.Agent.onApplicationPause(true);

//            IronSource.Agent.showInterstitial();
//            interSuccess = success;
//            interFail = () =>
//            {
//                fail?.Invoke();
//                Time.timeScale = 1;
//                AudioListener.pause = false;
//                IronSource.Agent.onApplicationPause(false);
//            };
//#elif UNITY_ANDROID
//        IronSource.Agent.showInterstitial();
//        interSuccess = success;
//        interFail = fail;
//#endif
//        }
//        else
//        {
//            success?.Invoke();
//        }
//    }

//    public void ShowReward(Action success, Action fail)
//    {
//        isReward = false;
//#if UNITY_EDITOR
//        if (m_SetAction)
//        {
//            success?.Invoke();
//        }
//        else
//        {
//            fail?.Invoke();
//        }
//#elif UNITY_IOS
//        Time.timeScale = 0;
//        AudioListener.pause = true;
//        IronSource.Agent.onApplicationPause(true);

//        IronSource.Agent.showRewardedVideo();
//        rewardSuccess = success;
//        rewardFail = () =>
//        {
//            fail?.Invoke();
//            Time.timeScale = 1;
//            AudioListener.pause = false;
//            IronSource.Agent.onApplicationPause(false);
//        };
//#elif UNITY_ANDROID
//        IronSource.Agent.showRewardedVideo();
//        rewardSuccess = success;
//        rewardFail = fail;
//#endif
//    }
}
