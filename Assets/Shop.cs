using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILogic : MonoBehaviour
{
    private const string CoinsKey = "Coins";
    private const string VehicleMapKey = "VehicleMap";

    public TextMeshProUGUI coinsText;

    private void Start()
    {
        InitializePlayerPrefs();
        UpdateCoinsDisplay();
    }

    public void GoToFirstLevel()
    {
        SceneManager.LoadScene("LVL1");
    }

    public void GoToShop()
    {
        SceneManager.LoadScene("Shop");
    }

    public void GoToInventory()
    {
        SceneManager.LoadScene("Inventory");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    private static void InitializePlayerPrefs()
    {
        if (!PlayerPrefs.HasKey(CoinsKey) || !PlayerPrefs.HasKey(VehicleMapKey))
        {
            ResetPlayerPrefs();
        }
    }

    private static void ResetPlayerPrefs()
    {
        PlayerPrefs.SetInt(CoinsKey, 0);
        PlayerPrefs.SetInt(VehicleMapKey, 0);
        PlayerPrefs.Save();
    }

    private void UpdateCoinsDisplay()
    {
        var coins = PlayerPrefs.GetInt(CoinsKey);
        coinsText.text = coins.ToString();
    }

    public void AddCoins(int amount)
    {
        var currentCoins = PlayerPrefs.GetInt(CoinsKey);
        currentCoins += amount;
        PlayerPrefs.SetInt(CoinsKey, currentCoins);
        PlayerPrefs.Save();
        UpdateCoinsDisplay();
    }

    public void RemoveCoins(int amount)
    {
        var currentCoins = PlayerPrefs.GetInt(CoinsKey);
        currentCoins -= amount;
        PlayerPrefs.SetInt(CoinsKey, currentCoins);
        PlayerPrefs.Save();
        UpdateCoinsDisplay();
    }
}