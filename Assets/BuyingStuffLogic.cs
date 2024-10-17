using System.Collections;
using TMPro;
using UnityEngine;

public class BuyingStuffLogic : MonoBehaviour
{
    public TextMeshProUGUI statusText;

    private int _coins;
    private int _carsUnlocked;

    private const int PurpleCarPrice = 420;
    private const int TruckPrice = 999;
    private const int GreenCarPrice = 4200;
    private const int RaceCarProPrice = 6969;

    public enum CarType
    {
        StartingCar,
        PurpleCar1,
        Truck,
        GreenCar,
        RaceCarPro
    }

    private void Start()
    {
        _coins = PlayerPrefs.GetInt("Coins");
        _carsUnlocked = PlayerPrefs.GetInt("VehicleMap");
    }

    public void Add100Coins() => AddCoins(100);
    public void Add300Coins() => AddCoins(300);
    public void Add1000Coins() => AddCoins(1000);
    public void Add2500Coins() => AddCoins(2500);
    public void Add5000Coins() => AddCoins(5000);
    public void AddRandomBoxCoins() => AddCoins(GenerateRandomBoxAmount());

    private void AddCoins(int amount)
    {
        _coins = PlayerPrefs.GetInt("Coins");
        _coins += amount;
        SavePlayerData(_coins, _carsUnlocked);
        UpdateStatusText($"Added {amount} coins!", Color.green);
    }

    private int GenerateRandomBoxAmount()
    {
        float randomValue = Random.Range(0f, 1f);
        if (randomValue < 0.6f)
            return Random.Range(5, 15);
        else if (randomValue < 0.9f)
            return Random.Range(15, 50);
        else
            return Random.Range(50, 101);
    }

    public void PurchasePurpleCar1()
    {
        PurchaseCar(CarType.PurpleCar1);
    }

    public void PurchaseTruck()
    {
        PurchaseCar(CarType.Truck);
    }

    public void PurchaseGreenCar()
    {
        PurchaseCar(CarType.GreenCar);
    }

    public void PurchaseRaceCarPro()
    {
        PurchaseCar(CarType.RaceCarPro);
    }

    private void PurchaseCar(CarType carType)
    {
        _coins = PlayerPrefs.GetInt("Coins");
        _carsUnlocked = PlayerPrefs.GetInt("VehicleMap");

        int carPrice = GetCarPrice(carType);

        if (IsCarOwned(carType))
        {
            UpdateStatusText($"You already own the {carType}.", Color.red);
            return;
        }

        if (_coins < carPrice)
        {
            UpdateStatusText($"Not enough coins to purchase the {carType}.", Color.red);
            return;
        }

        _coins -= carPrice;
        _carsUnlocked |= (1 << (int)carType);
        SavePlayerData(_coins, _carsUnlocked);

        UpdateStatusText($"Purchased {carType} successfully!", Color.green);
    }

    private bool IsCarOwned(CarType carType)
    {
        return (_carsUnlocked & (1 << (int)carType)) != 0;
    }

    private int GetCarPrice(CarType carType)
    {
        switch (carType)
        {
            case CarType.PurpleCar1:
                return PurpleCarPrice;
            case CarType.Truck:
                return TruckPrice;
            case CarType.GreenCar:
                return GreenCarPrice;
            case CarType.RaceCarPro:
                return RaceCarProPrice;
            default:
                UpdateStatusText("Invalid car type.", Color.red);
                return 0;
        }
    }

    private void SavePlayerData(int coins, int carsUnlocked)
    {
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.SetInt("VehicleMap", carsUnlocked);
        PlayerPrefs.Save();
    }

    private void UpdateStatusText(string message, Color color)
    {
        statusText.text = message;
        statusText.color = color;
    }
}
