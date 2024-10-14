using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyingStuffLogic : MonoBehaviour
{
    private int _coins;
    private int _carsUnlocked;

    // Prices for each car
    private const int PurpleCarPrice = 420;
    private const int TruckPrice = 999;
    private const int GreenCarPrice = 4200;
    private const int RaceCarProPrice = 6969;

    // Enum for car types
    public enum CarType
    {
        PurpleCar1,
        Truck,
        GreenCar,
        RaceCarPro
    }

    // Set my coins to 10000 for debugging purposes
    private void Start()
    {
        PlayerPrefs.SetInt("Coins", 10000);
        PlayerPrefs.SetInt("VehicleMap", 1); // Ensure vehicle map is initialized
        PlayerPrefs.Save();
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

        // Check if the player already owns the car
        if (IsCarOwned(carType))
        {
            Debug.Log($"You already own the {carType}."); // Log message for already owned car
            return; // Exit the function if car is already owned
        }

        Debug.Log($"Attempting to purchase {carType}. Coins: {_coins}, Price: {carPrice}");

        // Check if the player has enough coins
        if (_coins < carPrice)
        {
            Debug.Log($"Not enough coins to purchase the {carType}.");
            return; // Not enough coins
        }

        // Proceed with the purchase
        _coins -= carPrice;
        _carsUnlocked = (int)carType + 1;  // Set the car unlocked state (assuming each car corresponds to a unique int value)
        SavePlayerData(_coins, _carsUnlocked);
        Debug.Log($"Purchased {carType} successfully."); // Log successful purchase
    }

    private bool IsCarOwned(CarType carType)
    {
        // Check if the car is already unlocked
        return _carsUnlocked >= (int)carType + 1; // Check if the current unlocked state includes this car
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
                Debug.LogError("Invalid car type.");
                return 0; // Default price if the car type is invalid
        }
    }

    private void SavePlayerData(int coins, int carsUnlocked)
    {
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.SetInt("VehicleMap", carsUnlocked);
        PlayerPrefs.Save();

        // Debug the saved values
        Debug.Log($"Saved Coins: {coins}, Cars Unlocked: {carsUnlocked}");
    }
}
