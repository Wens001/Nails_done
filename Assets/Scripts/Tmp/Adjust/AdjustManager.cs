using System.Collections;
using System.Collections.Generic;
using com.adjust.sdk;
using UnityEngine;

public class AdjustManager : MonoBehaviour
{
    public static AdjustManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
#if UNITY_IOS
        var appToken = "mtty37b6r3sw";
#elif UNITY_ANDROID
        var appToken = "nrlb299bxfy8";
#endif
        
        var environment = AdjustEnvironment.Production;

        AdjustConfig config = new AdjustConfig(appToken, environment, true);
        config.setLogLevel(AdjustLogLevel.Info);
        config.setSendInBackground(true);

        new GameObject("Adjust").AddComponent<Adjust>();

        Adjust.start(config);
    }

    public void LogPurchaseEvent()
    {
        AdjustEvent adjustEvent = new AdjustEvent("af_purchase");
        Adjust.trackEvent(adjustEvent);
    }
}
