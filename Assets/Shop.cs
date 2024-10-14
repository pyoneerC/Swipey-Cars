using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILogic : MonoBehaviour
{
    private const string CoinsKey = "Coins";
    private const string VehicleMapKey = "VehicleMap";
    private const int MaxVehicles = 5; // 5 vehicles (0-4)

    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI vehicleText;

    private void Start()
    {
        InitializePlayerPrefs();
        UpdateCoinsDisplay();
        UpdateVehicleDisplay();
    }

    public void GoToFirstLevel()
    {
        var randomLevel = Random.Range(1, 5);
        SceneManager.LoadScene($"LVL{randomLevel}");
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
        PlayerPrefs.SetInt(CoinsKey, 0); // Start with 0 coins
        PlayerPrefs.SetInt(VehicleMapKey, 1); // Unlock the first vehicle
        PlayerPrefs.Save();
    }

    private void UpdateCoinsDisplay()
    {
        var coins = PlayerPrefs.GetInt(CoinsKey);
        coinsText.text = coins.ToString();
    }

    private void UpdateVehicleDisplay()
    {
        var vehicles = PlayerPrefs.GetInt(VehicleMapKey);
        var unlockedVehicleCount = CountUnlockedVehicles(vehicles);
        vehicleText.text = unlockedVehicleCount.ToString();
    }

    private static int CountUnlockedVehicles(int vehicleMap)
    {
        var count = 0;
        for (var i = 0; i < MaxVehicles; i++)
        {
            if ((vehicleMap & (1 << i)) != 0)
            {
                count++;
            }
        }
        return count;
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

    // Unlocks a vehicle in the shop (if it's in the valid range)
    public void UnlockVehicle(int vehicleIndex)
    {
        if (vehicleIndex is < 0 or >= MaxVehicles)
        {
            Debug.LogWarning("Invalid vehicle index");
            return;
        }

        var vehicleMap = PlayerPrefs.GetInt(VehicleMapKey);
        vehicleMap |= (1 << vehicleIndex); // Set bit to unlock the vehicle
        PlayerPrefs.SetInt(VehicleMapKey, vehicleMap);
        PlayerPrefs.Save();
    }

    // Checks if a vehicle is unlocked
    public bool IsVehicleUnlocked(int vehicleIndex)
    {
        if (vehicleIndex is < 0 or >= MaxVehicles)
        {
            Debug.LogWarning("Invalid vehicle index");
            return false;
        }

        var vehicleMap = PlayerPrefs.GetInt(VehicleMapKey);
        return (vehicleMap & (1 << vehicleIndex)) != 0; // Check if the bit is set
    }
}
