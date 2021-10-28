//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Purchasing;

//public class IAPService : MonoSingletonGlobal<IAPService>, IStoreListener
//{

//    private Action Success, Failure;

//    private static IStoreController m_StoreController;          // The Unity Purchasing system.
//    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
//    public static string remove_ads_id = "remove_ads";
//    public static string all_item_id = "all_item_id";
//    public static string all_themes_id = "all_theme_id";
//    public static string all_bottle_id = "all_bottle_id";

//    private string[] m_ProductNames;

//    void Start()
//    {
//        m_ProductNames = new string[1];
//        m_ProductNames[0] = remove_ads_id;

//        if (m_StoreController == null)
//        {
//            InitializePurchasing();
//        }
//    }

//    public void InitializePurchasing()
//    {
//        if (IsInitialized())
//        {
//            return;
//        }

//        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
//        builder.AddProduct(remove_ads_id, ProductType.NonConsumable);
//        builder.AddProduct(all_item_id, ProductType.NonConsumable);
//        builder.AddProduct(all_themes_id, ProductType.NonConsumable);
//        builder.AddProduct(all_bottle_id, ProductType.NonConsumable);

//        UnityPurchasing.Initialize(this, builder);
//    }


//    private bool IsInitialized()
//    {
//        return m_StoreController != null && m_StoreExtensionProvider != null;
//    }

//    public void BuyProductID(string productId, Action _success)
//    {
//#if UNITY_EDITOR
//        _success?.Invoke();
//#else
//        BuyProductID(productId);
//        Success = _success;
//#endif
//    }

//    public void RestoreProduct(Action CALLBACK_MISSING, Action CALLBACK_SUCCESS, Action CALLBACK_FAIL)
//    {
//        if (IsInitialized())
//        {
//            bool isSuccess = false;
//            for (int i = 0; i < m_ProductNames.Length; i++)
//            {
//                string id = m_ProductNames[i];
//                Product product = m_StoreController.products.WithID(id);
//                if (product != null && product.hasReceipt)
//                {
//                    isSuccess = true;
//                    switch (id)
//                    {
//                        case "iap_01":
//                            RuntimeStorageData.PLAYER.isAds = false;
//                            IronSourceManager.Instance.BannerHide();
//                            break;
//                        case "all_theme_id":
//                            ThemeManager.Instance.BuyAllTheme();
//                            break;
//                        case "all_bottle_id":
//                            BottleManager.Instance.BuyAllBottle();
//                            break;
//                        case "all_item_id":
//                            ThemeManager.Instance.BuyAllTheme();
//                            BottleManager.Instance.BuyAllBottle();
//                            break;
//                    }
//                }
//            }

//            if (isSuccess) CALLBACK_SUCCESS?.Invoke();
//            else CALLBACK_FAIL?.Invoke();
//        }
//        else
//        {
//            Debug.Log("RestoreProductID FAIL. Not initialized.");
//            InitializePurchasing();
//            CALLBACK_MISSING?.Invoke();
//        }
//    }

//    private void RestoreProductID(string id)
//    {
//        if (IsInitialized())
//        {
//            Product product = m_StoreController.products.WithID(id);
//            if (product != null && product.hasReceipt)
//            {
//                switch(id)
//                {
//                    case "iap_01":
//                        RuntimeStorageData.PLAYER.isAds = false;
//                        IronSourceManager.Instance.BannerHide();
//                        break;
//                    case "all_theme_id":
//                        ThemeManager.Instance.BuyAllTheme();
//                        break;
//                    case "all_bottle_id":
//                        BottleManager.Instance.BuyAllBottle();
//                        break;
//                    case "all_item_id":
//                        ThemeManager.Instance.BuyAllTheme();
//                        BottleManager.Instance.BuyAllBottle();
//                        break;
//                }
//            }
//        }
//        else
//        {
//            Debug.Log("RestoreProductID FAIL. Not initialized.");
//            InitializePurchasing();
//        }
//    }

//    void BuyProductID(string productId)
//    {
//        if (IsInitialized())
//        {
//            Product product = m_StoreController.products.WithID(productId);
//            if (product != null && product.availableToPurchase)
//            {
//                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
//                m_StoreController.InitiatePurchase(product);
//            }
//            else
//            {
//                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
//            }
//        }
//        else
//        {
//            Debug.Log("BuyProductID FAIL. Not initialized.");
//            InitializePurchasing();
//        }
//    }

//    public void RestorePurchases()
//    {
//        if (!IsInitialized())
//        {
//            Debug.Log("RestorePurchases FAIL. Not initialized.");
//            return;
//        }

//        if (Application.platform == RuntimePlatform.IPhonePlayer ||
//            Application.platform == RuntimePlatform.OSXPlayer)
//        {
//            Debug.Log("RestorePurchases started ...");

//            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
//            apple.RestoreTransactions((result) => {
//                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
//                if(result)
//                {
//                    for (int i = 0; i < m_ProductNames.Length; i++)
//                    {
//                        Debug.Log("Checking store " + m_ProductNames[i]);
//                        RestoreProductID(m_ProductNames[i]);
//                    }
//                }    
//            });
//        }
//        else
//        {
//            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
//        }
//    }

//    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
//    {
//        Debug.Log("OnInitialized: PASS");
//        m_StoreController = controller;
//        m_StoreExtensionProvider = extensions;  
//    }


//    public void OnInitializeFailed(InitializationFailureReason error)
//    {
//        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
//    }


//    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
//    {
//        Success?.Invoke();
//        return PurchaseProcessingResult.Complete;
//    }


//    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
//    {
//        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
//    }
//}
