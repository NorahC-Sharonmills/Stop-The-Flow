using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronSourceManager : MonoSingletonGlobal<IronSourceManager>
{
    private string ironsource_your_app_key = "";
    private Coroutine cacheCoroutineInter;
    private Coroutine cacheCoroutineReward;

    private Action m_RewardSuccess;
    private Action m_RewardFail;
    private Action m_InterSuccess;
    private Action m_InterFail;

    public enum AdsState
    {
        LOADING,
        READY,
        MISSING
    }

    private bool IsAutoLoadAds = false;

    private AdsState InterAdsState;
    private AdsState RewardAdsState;

    void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);
    }

    protected override void Awake()
    {
        base.Awake();
    }

    public IEnumerator InitializedIronsource()
    {
#if UNITY_ANDROID || UNITY_EDITOR
        ironsource_your_app_key = "110ae14e5";
        Debug.Log(ironsource_your_app_key + " android");
#endif
#if UNITY_IOS
            ironsource_your_app_key = "1101a6edd";
            Debug.Log(ironsource_your_app_key + " ios:");
#endif
        yield return null;
        AddListenInter();
        AddListenRewardVideo();

        yield return null;
        LoadBanner();
        LoadInter();
        LoadReward();
        yield return WaitForSecondCache.WAIT_TIME_ONE;
        IronSource.Agent.shouldTrackNetworkState(true);
        IronSource.Agent.validateIntegration();
        IsAutoLoadAds = true;

        AutoSpamStatusAds();
    }

    private void AutoSpamStatusAds()
    {
        StartCoroutine(CorotineSpamStatusAds());
    }

    private IEnumerator CorotineSpamStatusAds()
    {
        Debug.Log(string.Format("ads status : inter is {0} - reward is {1}", InterAdsState, RewardAdsState));
        yield return WaitForSecondCache.WAIT_TIME_FIVE;
        StartCoroutine(CorotineSpamStatusAds());
    }

    private void AddListenInter()
    {
        // Invoked when the Interstitial is Ready to shown after load function is called
        IronSourceEvents.onInterstitialAdReadyEvent += () =>
        {
            InterAdsState = AdsState.READY;
            Debug.Log("onInterstitialAdReadyEvent");
        };
        // Invoked when the initialization process has failed.
        // @param description - string - contains information about the failure.
        IronSourceEvents.onInterstitialAdLoadFailedEvent += (IronSourceError) =>
        {
            InterAdsState = AdsState.MISSING;
            Debug.Log("onInterstitialAdLoadFailedEvent");
        };
        // Invoked right before the Interstitial screen is about to open.
        // NOTE - This event is available only for some of the networks. 
        // You should treat this event as an interstitial impression, but rather use InterstitialAdOpenedEvent
        IronSourceEvents.onInterstitialAdShowSucceededEvent += () =>
        {
            Debug.Log("onInterstitialAdShowSucceededEvent");
        };
        // Invoked when the ad fails to show.
        // @param description - string - contains information about the failure.
        IronSourceEvents.onInterstitialAdShowFailedEvent += (IronSourceError) =>
        {
            Debug.Log("onInterstitialAdShowFailedEvent");
            m_InterFail?.Invoke();
            InterAdsState = AdsState.MISSING;
        };
        // Invoked when end user clicked on the interstitial ad
        IronSourceEvents.onInterstitialAdClickedEvent += () =>
        {
            Debug.Log("onInterstitialAdClickedEvent");
        };
        // Invoked when the Interstitial Ad Unit has opened
        IronSourceEvents.onInterstitialAdOpenedEvent += () =>
        {
            Debug.Log("onInterstitialAdOpenedEvent");
        };
        // Invoked when the interstitial ad closed and the user goes back to the application screen.
        IronSourceEvents.onInterstitialAdClosedEvent += () =>
        {
            Debug.Log("onInterstitialAdClosedEvent");
            m_InterSuccess?.Invoke();
#if UNITY_IOS
                Time.timeScale = 1;
                AudioListener.pause = false;
                IronSource.Agent.onApplicationPause(false);
#endif
            };
        bool available = IronSource.Agent.isInterstitialReady();
        if (!available)
            InterAdsState = AdsState.MISSING;
    }

    private bool isReward = false;
    private void AddListenRewardVideo()
    {
        //Invoked when the RewardedVideo ad view has opened.
        //Your Activity will lose focus. Please avoid performing heavy 
        //tasks till the video ad will be closed.
        IronSourceEvents.onRewardedVideoAdOpenedEvent += () =>
        {
            Debug.Log("onRewardedVideoAdOpenedEvent");       
        };
        IronSourceEvents.onRewardedVideoAdClickedEvent += (IronSourcePlacement) =>
        {
            Debug.Log("onRewardedVideoAdClickedEvent");
        };
        //Invoked when the RewardedVideo ad view is about to be closed.
        //Your activity will now regain its focus.
        IronSourceEvents.onRewardedVideoAdClosedEvent += () =>
        {
            Debug.Log("onRewardedVideoAdClosedEvent");
            if (isReward)
            {
#if UNITY_IOS
                    Time.timeScale = 1;
                    AudioListener.pause = false;
                    IronSource.Agent.onApplicationPause(false);
#endif
                m_RewardSuccess?.Invoke();
            }
            else
            {
                m_RewardFail?.Invoke();
            }

            bool available = IronSource.Agent.isRewardedVideoAvailable();
            if (!available)
                RewardAdsState = AdsState.MISSING;
        };
        //Invoked when there is a change in the ad availability status.
        //@param - available - value will change to true when rewarded videos are available. 
        //You can then show the video by calling showRewardedVideo().
        //Value will change to false when no videos are available.
        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += (available) =>
        {
            Debug.Log("onRewardedVideoAvailabilityChangedEvent " + available);
            //Change the in-app 'Traffic Driver' state according to availability.
            bool rewardedVideoAvailability = available;
            if (!available)
                RewardAdsState = AdsState.MISSING;
            else
                RewardAdsState = AdsState.READY;
        };
        //Invoked when the video ad starts playing. 
        IronSourceEvents.onRewardedVideoAdStartedEvent += () =>
        {
            Debug.Log("onRewardedVideoAdStartedEvent");
        };
        //Invoked when the video ad finishes playing. 
        IronSourceEvents.onRewardedVideoAdEndedEvent += () =>
        {
            Debug.Log("onRewardedVideoAdEndedEvent");
        };
        IronSourceEvents.onRewardedVideoAdRewardedEvent += (IronSourcePlacement) =>
        {
            Debug.Log("onRewardedVideoAdRewardedEvent");
            isReward = true;
        };
        IronSourceEvents.onRewardedVideoAdShowFailedEvent += (IronSourceError) =>
        {
            Debug.Log("onRewardedVideoAdShowFailedEvent");
            m_RewardFail?.Invoke();
            RewardAdsState = AdsState.MISSING;
        };
    }

    private float time = 0;
    private void Update()
    {
        if(IsAutoLoadAds)
        {
            if (InterAdsState == AdsState.MISSING)
                LoadInter();
            if (RewardAdsState == AdsState.MISSING)
                LoadReward();


            time += Time.deltaTime;
            if (time > 5f)
            {
                time = 0;
                if (!IronSource.Agent.isInterstitialReady())
                    InterAdsState = AdsState.MISSING;
                if (!IronSource.Agent.isRewardedVideoAvailable())
                    RewardAdsState = AdsState.MISSING;
            }
        }
    }    

    private void LoadBanner()
    {
        Debug.Log("Load Banner");
        //For Banners
        IronSource.Agent.init(ironsource_your_app_key, IronSourceAdUnits.BANNER);
        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
        IronSourceEvents.onBannerAdLoadFailedEvent += (IronSourceError) =>
        {
            CoroutineUtils.PlayCoroutine(() =>
            {
                //For Banners
                IronSource.Agent.init(ironsource_your_app_key, IronSourceAdUnits.BANNER);
                IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
                BannerShow();
            }, 1f);
        };

        BannerShow();
    }

    private void LoadInter()
    {
        //For Interstitial
        InterAdsState = AdsState.LOADING;
        IronSource.Agent.init(ironsource_your_app_key, IronSourceAdUnits.INTERSTITIAL);
        IronSource.Agent.loadInterstitial();
    }

    private void LoadReward()
    {
        RewardAdsState = AdsState.LOADING;
        //For Rewarded Video
        IronSource.Agent.init(ironsource_your_app_key, IronSourceAdUnits.REWARDED_VIDEO);
    }

    public void BannerShow()
    {
        if (RuntimeStorageData.PLAYER.isAds)
        {
            Debug.Log("show banner");
            IronSource.Agent.displayBanner();
            FirebaseManager.Instance.ShowBanner();
        }
        else
        {
            BannerHide();
        }
    }

    public void BannerHide()
    {
        Debug.Log("hide banner");
        IronSource.Agent.hideBanner();
    }

    public bool m_SetAction = true;

    public void ShowInter(Action success)
    {
        //show ads inter show level 3;
        if (RuntimeStorageData.PLAYER.isAds)
        {
            FirebaseManager.Instance.ShowInter();

#if UNITY_EDITOR
            if (m_SetAction)
            {
                success?.Invoke();
            }
            else
            {
                success?.Invoke();
            }
#elif UNITY_IOS
                Time.timeScale = 0;
                AudioListener.pause = true;
                IronSource.Agent.onApplicationPause(true);

                IronSource.Agent.showInterstitial();
                m_InterSuccess = success;
                m_InterFail = () =>
                {
                    fail?.Invoke();
                    Time.timeScale = 1;
                    AudioListener.pause = false;
                    IronSource.Agent.onApplicationPause(false);
                };
#elif UNITY_ANDROID
            IronSource.Agent.showInterstitial();
            m_InterSuccess = success;
            m_InterFail = success;
#endif
        }
        else
        {
            success?.Invoke();
        }
    }

    public void ShowReward(Action success, Action fail)
    {
        isReward = false;
        FirebaseManager.Instance.ShowReward();
#if UNITY_EDITOR
        if (m_SetAction)
        {
            success?.Invoke();
        }
        else
        {
            fail?.Invoke();
        }
#elif UNITY_IOS
            Time.timeScale = 0;
            AudioListener.pause = true;
            IronSource.Agent.onApplicationPause(true);

            IronSource.Agent.showRewardedVideo();
            m_RewardSuccess = success;
            m_RewardFail = () =>
            {
                fail?.Invoke();
                Time.timeScale = 1;
                AudioListener.pause = false;
                IronSource.Agent.onApplicationPause(false);
            };
#elif UNITY_ANDROID
            IronSource.Agent.showRewardedVideo();
            m_RewardSuccess = success;
            m_RewardFail = fail;
#endif
    }
}
