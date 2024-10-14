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
        if (PurchaseCar(CarType.PurpleCar1))
        {
            Debug.Log("Purchased Purple Car 1 successfully.");
        }
    }

    public void PurchaseTruck()
    {
        if (PurchaseCar(CarType.Truck))
        {
            Debug.Log("Purchased Truck successfully.");
        }
    }

    public void PurchaseGreenCar()
    {
        if (PurchaseCar(CarType.GreenCar))
        {
            Debug.Log("Purchased Green Car successfully.");
        }
    }

    public void PurchaseRaceCarPro()
    {
        if (PurchaseCar(CarType.RaceCarPro))
        {
            Debug.Log("Purchased Race Car Pro successfully.");
        }
    }

    private bool PurchaseCar(CarType carType)
    {
        _coins = PlayerPrefs.GetInt("Coins");
        _carsUnlocked = PlayerPrefs.GetInt("VehicleMap");

        int carPrice = GetCarPrice(carType);
        Debug.Log($"Attempting to purchase {carType}. Coins: {_coins}, Price: {carPrice}");

        // Check if the player has enough coins
        if (_coins < carPrice)
        {
            Debug.Log($"Not enough coins to purchase the {carType}.");
            return false; // Not enough coins
        }

        // Proceed with the purchase
        _coins -= carPrice;
        _carsUnlocked = (int)carType + 1;  // Set the car unlocked state (assuming each car corresponds to a unique int value)
        SavePlayerData(_coins, _carsUnlocked);
        return true; // Purchase successful
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
