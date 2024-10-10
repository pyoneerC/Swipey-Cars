using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAdExample : MonoBehaviour
{
    [SerializeField] BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;

    [SerializeField] string _androidAdUnitId = "Banner_Android";
    [SerializeField] string _iOSAdUnitId = "Banner_iOS";
    string _adUnitId = null; // This will remain null for unsupported platforms.

    void Start()
    {
        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif

        // Set the banner position:
        Advertisement.Banner.SetPosition(_bannerPosition);

        // Load the banner right away (or call this from another part of the code when needed)
        LoadBanner();
    }

    // Method to load the banner
    public void LoadBanner()
    {
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        // Load the Ad Unit with banner content:
        Advertisement.Banner.Load(_adUnitId, options);
    }

    // Callback when the banner is successfully loaded
    void OnBannerLoaded()
    {
        Debug.Log("Banner loaded");

        // Show the banner immediately after loading (or call this from another method):
        ShowBannerAd();
    }

    // Callback if the banner loading fails
    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
        // Optionally execute additional code, such as retrying to load the banner.
    }

    // Method to show the banner
    public void ShowBannerAd()
    {
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };

        // Show the loaded banner
        Advertisement.Banner.Show(_adUnitId, options);
    }

    // Method to hide the banner
    public void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }

    // Callbacks for banner events
    void OnBannerClicked() { }
    void OnBannerShown() { }
    void OnBannerHidden() { }
}
