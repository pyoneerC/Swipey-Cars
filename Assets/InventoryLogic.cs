using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryLogic : MonoBehaviour
{
    // Visual references to car GameObjects (add their respective cross/check images later)
    public GameObject PurpleCarGameobjectVisual;
    public GameObject Truck;
    public GameObject GreenCar;
    public GameObject RaceCarPro;

    // Cars unlocked by beating levels (these require level milestones to unlock)
    public GameObject RedRaceCar;
    public GameObject WhiteRaceCar;

    // Cross and checkmark icons for owned/unowned cars
    public Texture2D icon_cross; // Cross icon for unowned cars
    public Texture2D icon_checkmark; // Checkmark icon for owned cars

    private void Start()
    {
        // Logic to initialize the inventory UI will go here
        UpdateInventory();
    }

    // Function to update the inventory UI based on what cars are owned
    private void UpdateInventory()
    {
        // Example of how to handle each car (this should be extended for every car)
        UpdateCarVisual(PurpleCarGameobjectVisual, CarType.PurpleCar1);
        UpdateCarVisual(Truck, CarType.Truck);
        UpdateCarVisual(GreenCar, CarType.GreenCar);
        UpdateCarVisual(RaceCarPro, CarType.RaceCarPro);

        // Handle special cars that require level completion to unlock
        UpdateLevelBasedCar(RedRaceCar, 20);  // 20 levels for RedRaceCar
        UpdateLevelBasedCar(WhiteRaceCar, 50); // 50 levels for WhiteRaceCar
    }

    // Function to update the car visual (cross or check) based on ownership
    private void UpdateCarVisual(GameObject carGameObject, CarType carType)
    {
        bool isOwned = CheckIfCarOwned(carType);
        RawImage carIcon = carGameObject.GetComponentInChildren<RawImage>();

        if (isOwned)
        {
            carIcon.texture = icon_checkmark;
        }
        else
        {
            carIcon.texture = icon_cross;
        }
    }

    // Function to update level-based unlocks (for extra cars like RedRaceCar and WhiteRaceCar)
    private void UpdateLevelBasedCar(GameObject carGameObject, int requiredLevels)
    {
        int currentLevel = GetPlayerLevel();
        RawImage carIcon = carGameObject.GetComponentInChildren<RawImage>();

        if (currentLevel >= requiredLevels)
        {
            carIcon.texture = icon_checkmark;
        }
        else
        {
            carIcon.texture = icon_cross;
        }
    }

    // Placeholder: Check if the player owns a car based on their progress (replace with actual logic)
    private bool CheckIfCarOwned(CarType carType)
    {
        // Retrieve this from PlayerPrefs or your game's save system
        int carsUnlocked = PlayerPrefs.GetInt("VehicleMap", 1); // Assuming 1 means StartingCar
        return (carsUnlocked & (1 << (int)carType)) != 0; // Bitwise check for ownership
    }

    // Placeholder: Get the player's current level (replace with actual logic)
    private int GetPlayerLevel()
    {
        // Assuming PlayerPrefs stores the player's level
        return PlayerPrefs.GetInt("PlayerLevel", 1); // Default to level 1
    }

    // Enum for car types
    public enum CarType
    {
        StartingCar,   // Default car every player has
        PurpleCar1,
        Truck,
        GreenCar,
        RaceCarPro
    }
}
