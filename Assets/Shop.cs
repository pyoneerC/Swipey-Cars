using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILogic : MonoBehaviour
{
    private const string CoinsKey = "Coins";
    private const string VehicleMapKey = "VehicleMap";
    private const int MaxVehicles = 7; // 7 vehicles (0-6)

    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI vehicleText;
    public TextMeshProUGUI levelsBeatenText;

    private int _coins;
    private int _carsUnlocked;
    private int _levelsBeaten;
    private int _selectedVehicle;

    private void Start()
    {
        InitializePlayerPrefs();
        _coins = PlayerPrefs.GetInt(CoinsKey);
        _carsUnlocked = PlayerPrefs.GetInt(VehicleMapKey);
        _levelsBeaten = PlayerPrefs.GetInt("LevelsBeaten");
        _selectedVehicle = PlayerPrefs.GetInt("SelectedVehicle");

        ResetPlayerPrefs();

        UpdateCoinsDisplay();
        UpdateVehicleDisplay();
        UpdateLevelsBeatenDisplay();
    }

    // Update coins, vehicle, and levels beaten texts if changed
    private void Update()
    {
        if (_coins != PlayerPrefs.GetInt(CoinsKey))
        {
            _coins = PlayerPrefs.GetInt(CoinsKey);
            UpdateCoinsDisplay();
        }

        if (_carsUnlocked != PlayerPrefs.GetInt(VehicleMapKey))
        {
            _carsUnlocked = PlayerPrefs.GetInt(VehicleMapKey);
            UpdateVehicleDisplay();
        }

        if (_levelsBeaten == PlayerPrefs.GetInt("LevelsBeaten")) return;
        _levelsBeaten = PlayerPrefs.GetInt("LevelsBeaten");
        UpdateLevelsBeatenDisplay();
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
        if (!PlayerPrefs.HasKey(CoinsKey) || !PlayerPrefs.HasKey(VehicleMapKey) || !PlayerPrefs.HasKey("LevelsBeaten") || !PlayerPrefs.HasKey("SelectedVehicle"))
        {
            ResetPlayerPrefs();
        }
    }

    private static void ResetPlayerPrefs()
    {
        PlayerPrefs.SetInt(CoinsKey, 0); // Start with 0 coins
        PlayerPrefs.SetInt(VehicleMapKey, 1); // Unlock the first vehicle
        PlayerPrefs.SetInt("LevelsBeaten", 0); // Start with 0 levels beaten
        PlayerPrefs.SetInt("SelectedVehicle", 0); // Select the first vehicle
        PlayerPrefs.Save();
    }

    private void UpdateCoinsDisplay()
    {
        coinsText.text = _coins.ToString();
    }

    private void UpdateVehicleDisplay()
    {
        var unlockedVehicleCount = CountUnlockedVehicles(_carsUnlocked);
        vehicleText.text = unlockedVehicleCount.ToString();
    }

    private void UpdateLevelsBeatenDisplay()
    {
        levelsBeatenText.text = _levelsBeaten.ToString();
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
        _coins += amount;
        PlayerPrefs.SetInt(CoinsKey, _coins);
        PlayerPrefs.Save();
        UpdateCoinsDisplay();
    }

    public void RemoveCoins(int amount)
    {
        _coins -= amount;
        PlayerPrefs.SetInt(CoinsKey, _coins);
        PlayerPrefs.Save();
        UpdateCoinsDisplay();
    }

    public void UnlockVehicle(int vehicleIndex)
    {
        if (vehicleIndex < 0 || vehicleIndex >= MaxVehicles)
        {
            Debug.LogWarning("Invalid vehicle index");
            return;
        }

        _carsUnlocked |= (1 << vehicleIndex); // Set bit to unlock the vehicle
        PlayerPrefs.SetInt(VehicleMapKey, _carsUnlocked);
        PlayerPrefs.Save();
        UpdateVehicleDisplay();
    }

    public bool IsVehicleUnlocked(int vehicleIndex)
    {
        if (vehicleIndex is >= 0 and < MaxVehicles)
            return (_carsUnlocked & (1 << vehicleIndex)) != 0; // Check if the bit is set
        Debug.LogWarning("Invalid vehicle index");
        return false;

    }
}
