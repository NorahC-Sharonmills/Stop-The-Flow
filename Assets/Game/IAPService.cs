using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPService : MonoSingletonGlobal<IAPService>, IStoreListener
{

    private Action Success, Failure;

    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
    public static string remove_ads_id = "remove_ads";
    IEnumerator Start()
    {
        yield return WaitForSecondCache.WAIT_TIME_ONE;
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(remove_ads_id, ProductType.NonConsumable);
        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuyProductID(string productId, Action _success)
    {
#if UNITY_EDITOR
        _success?.Invoke();
#else
        BuyProductID(productId);
        Success = _success;
#endif
    }

    public void RestoreProduct(Action CALLBACK_MISSING, Action CALLBACK_SUCCESS, Action CALLBACK_FAIL)
    {
        if (IsInitialized())
        {
            bool isSuccess = false;
            Product product = m_StoreController.products.WithID(remove_ads_id);
            if (product != null && product.hasReceipt)
            {
                isSuccess = true;
                RuntimeStorageData.PLAYER.isAds = false;
                IronSourceManager.Instance.BannerHide();
                if (Game.UIManager.Instance != null)
                    Game.UIManager.Instance.HideRemoveAds();
            }

            if (isSuccess) CALLBACK_SUCCESS?.Invoke();
            else CALLBACK_FAIL?.Invoke();
        }
        else
        {
            Debug.Log("RestoreProductID FAIL. Not initialized.");
            InitializePurchasing();
            CALLBACK_MISSING?.Invoke();
        }
    }

    private void RestoreProductID(string id)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(id);
            if (product != null && product.hasReceipt)
            {
                if(id == remove_ads_id)
                {
                    RuntimeStorageData.PLAYER.isAds = false;
                    IronSourceManager.Instance.BannerHide();
                    if (Game.UIManager.Instance != null)
                        Game.UIManager.Instance.HideRemoveAds();
                }
            }
        }
        else
        {
            Debug.Log("RestoreProductID FAIL. Not initialized.");
            InitializePurchasing();
        }
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
            InitializePurchasing();
        }
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) =>
            {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                if (result)
                {
                    Debug.Log("Checking store " + remove_ads_id);
                    RestoreProductID(remove_ads_id);
                }
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Success?.Invoke();
        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}
