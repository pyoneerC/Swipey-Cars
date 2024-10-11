using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopCode : MonoBehaviour
{
    public void BackToLevel1()
    {
        SceneManager.LoadScene("LVL1");
    }

    public void ShowRewardedAd()
    {
        FindObjectOfType<RewardedAdsButton>().ShowAd();
    }

}
