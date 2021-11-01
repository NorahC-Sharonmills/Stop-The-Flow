using Firebase;
using Firebase.Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseManager : MonoSingletonGlobal<FirebaseManager>
{
    private bool IsFirebaseInitialized = false;

    IEnumerator Start()
    {
        yield return WaitForSecondCache.WAIT_TIME_ONE;
        yield return InitializedFirebase();
        yield return IronSourceManager.Instance.InitializedIronsource();
        yield return InitializedFirebaseMessaging();

        OpenApplication();
    }

    IEnumerator InitializedFirebase()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Initializer maybe
                var app = FirebaseApp.DefaultInstance;
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

                // Finish initializer
                Debug.Log("Firebase initialized");
                IsFirebaseInitialized = true;
            }
            else
            {
                IsFirebaseInitialized = true;
                Debug.LogError(string.Format("Dependency error: {0}", dependencyStatus)); // Firebase Unity SDK is not safe to use here.
            }
        });
        yield return null;
    }

    IEnumerator InitializedFirebaseMessaging()
    {
        yield return new WaitUntil(() => IsFirebaseInitialized);
        Debug.Log("Firebase Messaging initialized");
        Firebase.Messaging.FirebaseMessaging.TokenReceived += FirebaseMessaging_TokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += FirebaseMessaging_MessageReceived;
    }

    private void FirebaseMessaging_MessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
    }

    private void FirebaseMessaging_TokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs e)
    {
        UnityEngine.Debug.Log("Received Registration Token: " + e.Token);
    }

    public void OpenApplication()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("app_open");
    }

    public void RemoveAdsClick()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("remove_ads_click");
    }

    public void ShowBanner()
    {
        if (IsFirebaseInitialized)
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent("ads_banner");
        }
        else
        {
            actions.Enqueue(() =>
            {
                Firebase.Analytics.FirebaseAnalytics.LogEvent("ads_banner");
            });
        }
    }

    public void ShowInter()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("ads_inter");
    }

    public void ShowInterReplay()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("ads_inter_replay");
    }

    public void ShowInterNext()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("ads_inter_next");
    }

    public void ShowInterBackHome()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("ads_inter_backhome");
    }

    public void ShowReward()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("ads_reward");
    }

    public void ShowRewardUnlock()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("ads_reward_unlock");
    }

    public void ShowRewardSkip()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("ads_reward_skip");
    }

    public void ClickSkip()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("skip_click");
    }

    public void ClickShop()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("shop_click");
    }

    public void LevelPlay()
    {
        int RealLevel = RuntimeStorageData.PLAYER.level + 1;
        string StringRealLevel = string.Format("level_{0}", RealLevel);
        if (IsFirebaseInitialized)
        {
            if (RealLevel > 20)
            {
                if (RealLevel % 10 == 0)
                    Firebase.Analytics.FirebaseAnalytics.LogEvent(StringRealLevel);
            }
            else
            {
                Firebase.Analytics.FirebaseAnalytics.LogEvent(StringRealLevel);
            }
        }
        else
        {
            actions.Enqueue(() =>
            {
                if (RealLevel > 20)
                {
                    if (RealLevel % 10 == 0)
                        Firebase.Analytics.FirebaseAnalytics.LogEvent(StringRealLevel);
                }
                else
                {
                    Firebase.Analytics.FirebaseAnalytics.LogEvent(StringRealLevel);
                }
            });
        }
    }

    private Queue<System.Action> actions = new Queue<System.Action>();
    private void Update()
    {
        if(IsFirebaseInitialized)
        {
            if(actions.Count > 0)
            {
                RunAction();
            }
        }
    }

    private float time = 0f;
    private void RunAction()
    {
        if(time >= 1f)
        {
            time = 0;
            System.Action action = actions.Dequeue();
            action?.Invoke();
        }
        else
        {
            time += Time.deltaTime;
        }
    }
}
