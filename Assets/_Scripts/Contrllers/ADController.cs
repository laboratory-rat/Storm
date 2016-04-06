using UnityEngine;
using System.Collections;
using Reign;

public class ADController : MonoBehaviour
{
    #region Instance

    private static ADController _instance = null;

    public static ADController Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Init()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(this);
        }
    }

    #endregion Instance

    private static InterstitialAd ad;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        var desc = new InterstitialAdDesc();

        // Global
        desc.Testing = false;
        desc.EventCallback = eventCallback;

        // Android
        desc.Android_AdAPI = InterstitialAdAPIs.AdMob;
        desc.Android_AdMob_UnitID = "pub-9869209397937230";// NOTE: Must set event for testing

        // create ad
        ad = InterstitialAdManager.CreateAd(desc, createdCallback);
    }

    private void createdCallback(bool success)
    {
        Debug.Log(success);
        if (!success) Debug.LogError("Failed to create InterstitialAd!");
    }

    private void eventCallback(InterstitialAdEvents adEvent, string eventMessage)
    {
        Debug.Log(adEvent);
        if (adEvent == InterstitialAdEvents.Error) Debug.LogError(eventMessage);
        if (adEvent == InterstitialAdEvents.Cached) ad.Show();
    }

    void OnGUI()
    {
        GUI.matrix = Matrix4x4.identity;
        GUI.color = Color.white;

        if (GUI.Button(new Rect(0, 0, 128, 64), "Show Ad"))
        {
            ad.Cache();
        }
    }
}