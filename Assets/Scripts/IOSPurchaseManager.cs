using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
//using UnityEngine.Purchasing;

public class IOSPurchaseManager : MonoBehaviour
{
#if UNITY_IOS

    public const string ShopAds = "com.lioncel.www.ads";

#elif UNITY_ANDROID

    //public const string ShopAds = "com.lioncel.www.ads";
    public const string ShopAds = "com.lioncel.wws.ads";
    
#endif

//    private IStoreController _controller;
//    private IAppleExtensions _appleExtensions;
    
    private bool _isInited;

    private bool _isRestore;
    
    public static IOSPurchaseManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        
        if (!_isInited)
        {
            InitPurchase();
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void InitPurchase()
    {
//        var module = StandardPurchasingModule.Instance();
//        var builder = ConfigurationBuilder.Instance(module);
//
//        builder.AddProduct(ShopAds, ProductType.NonConsumable);
//
//        UnityPurchasing.Initialize(this, builder);
    }
    
//    /// <summary>
//    /// 初始化成功
//    /// </summary>
//    /// <param name="controller">Controller.</param>
//    /// <param name="extensions">Extensions.</param>
//    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
//    {
//        _controller = controller;
//        _appleExtensions = extensions.GetExtension<IAppleExtensions>();
//        _appleExtensions.RegisterPurchaseDeferredListener(OnDeferred);
//
//        _isInited = true;
//        
//        Debug.Log("购买初始化成功");
//    }
    
//    /// <summary>
//    /// iOS 网络延迟错误
//    /// </summary>
//    /// <param name="item">Item.</param>
//    private void OnDeferred(Product item)
//    {
//        Debug.Log("网络连接不稳");
//    }
//
//    /// <summary>
//    /// 初始化失败
//    /// </summary>
//    /// <param name="error">Error.</param>
//    public void OnInitializeFailed(InitializationFailureReason error)
//    {
//        Debug.LogError("购买初始化失败：" + error);
//    }

    /// <summary>
    /// 恢复购买
    /// </summary>
    public void RestorePurchases()
    {
        if (!_isInited)
        {
            Debug.Log("购买未初始化");
            return;
        }

        _isRestore = true;
        
//        _appleExtensions.RestoreTransactions((result) =>
//        {
//            // The first phase of restoration. If no more responses are received on ProcessPurchase then 
//            // no purchases are available to be restored.
//            Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
//            
//            if (_isRestore && SetView.Instance)
//                SetView.Instance.ShowConfirmView();
//
//#if UNITY_IOS
//            if (result)
//            {
//                //产品已经restore，不过官方的解释是恢复过程成功了，并不代表所购买的物品都恢复了
//                //NativeToolkit.ShowAlert("Restore completed", "");
//            }
//            else
//            {
//                // 恢复失败
//                //NativeToolkit.ShowAlert("Restore failed", "Please purchase the software application first.");
//            }
//#endif
//        });
    }


    /// <summary>
    /// 购买产品  购买的第几个    按钮点击
    /// </summary>
    /// <param name="index">Index.</param>
    public void PurchaseShop(string id)
    {
//        if (!_isInited)
//        {
//            Debug.Log("购买未初始化");
//            return;
//        }
        
//        var product = _controller.products.WithID(id);
//        if (product != null && product.availableToPurchase)
//        {
//            _isRestore = false;
//            
//            _controller.InitiatePurchase(product);
//            
//            Debug.Log("购买商品：" + id);
//        }
//        else
//        {
//            Debug.Log("商品不可用：" + id);
//        }
    }

//    /// <summary>
//    /// 购买成功回调
//    /// </summary>
//    /// <returns>The purchase.</returns>
//    /// <param name="e">E.</param>
//    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
//    {
//        //使用id判断是否是当前购买的产品，我这里只有一个产品，所以就是products[0]
//
//        var id = e.purchasedProduct.definition.id;
//        
//        switch (id)
//        {
//            case ShopAds:
//
//                Data.NoAd_isBought = 1;
//                
//                PlayerPrefs.SetInt("NoAd_isBought", 1);
//                
//                UIControl.Instance.UI_NoAD.rectTransform.anchoredPosition = UIControl.Instance.V3_NoAD_Out;
//                
//                if (!_isRestore) AdjustManager.Instance.LogPurchaseEvent();
//                
//                ADSDK.Intance.HideBanner();
//                ADSDK.Intance.HideCrossPromo();
//
//                break;
//            
//            default:
//                Debug.Log("购买回调未知的商品：" + id);
//                break;
//        }
//        
//        Debug.Log("购买成功回调：" + id + ", " + _isRestore);
//
//        if (!_isRestore)
//        {
//            
//        }
//
//        return PurchaseProcessingResult.Complete;
//    }
//
//    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
//    {
//        //购买失败的逻辑
//        Debug.LogError("购买失败：" + i.definition.id + "," + i.definition.storeSpecificId + "," + i.availableToPurchase + "，" + p);
//    }
}