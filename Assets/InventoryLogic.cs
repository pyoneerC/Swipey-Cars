using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Import TextMeshPro namespace

public class InventoryLogic : MonoBehaviour
{
    public GameObject PurpleCarGameobjectVisual;
    public GameObject Truck;
    public GameObject GreenCar;
    public GameObject RaceCarPro;
    public GameObject RedRaceCar;
    public GameObject WhiteRaceCar;

    public GameObject RedRaceCarText;  // Keep as GameObject
    public GameObject WhiteRaceCarText;  // Keep as GameObject

    public Texture2D icon_cross;
    public Texture2D icon_checkmark;

    private void Start()
    {
        UpdateInventory();
    }

    private void UpdateInventory()
    {
        UpdateCarVisual(PurpleCarGameobjectVisual, CarType.PurpleCar1);
        UpdateCarVisual(Truck, CarType.Truck);
        UpdateCarVisual(GreenCar, CarType.GreenCar);
        UpdateCarVisual(RaceCarPro, CarType.RaceCarPro);

        UpdateLevelBasedCar(RedRaceCar, 20, RedRaceCarText);
        UpdateLevelBasedCar(WhiteRaceCar, 50, WhiteRaceCarText);
    }

    private void UpdateCarVisual(GameObject carGameObject, CarType carType)
    {
        bool isOwned = CheckIfCarOwned(carType);
        RawImage carIcon = carGameObject.GetComponentInChildren<RawImage>();
        carIcon.texture = isOwned ? icon_checkmark : icon_cross;
    }

    private void UpdateLevelBasedCar(GameObject carGameObject, int requiredLevels, GameObject levelTextObject)
    {
        int levelsBeaten = PlayerPrefs.GetInt("LevelsBeaten");
        RawImage carIcon = carGameObject.GetComponentInChildren<RawImage>();
        int remainingLevels = requiredLevels - levelsBeaten;

        if (levelsBeaten >= requiredLevels)
        {
            carIcon.texture = icon_checkmark;
            remainingLevels = 0;
        }
        else
        {
            carIcon.texture = icon_cross;
        }

        // Get the TextMeshProUGUI component from the GameObject
        TextMeshProUGUI levelText = levelTextObject.GetComponent<TextMeshProUGUI>();
        remainingLevels = Mathf.Max(0, remainingLevels);
        levelText.text = remainingLevels.ToString();
        levelText.color = GetLevelColor(remainingLevels, requiredLevels);
    }

    private Color GetLevelColor(int remainingLevels, int requiredLevels)
    {
        if (remainingLevels <= 0) return Color.green;
        float ratio = Mathf.Clamp01((float)remainingLevels / requiredLevels);
        return Color.Lerp(Color.green, Color.red, ratio);
    }

    private bool CheckIfCarOwned(CarType carType)
    {
        int carsUnlocked = PlayerPrefs.GetInt("VehicleMap", 1);
        return (carsUnlocked & (1 << (int)carType)) != 0;
    }

    public enum CarType
    {
        StartingCar,
        PurpleCar1,
        Truck,
        GreenCar,
        RaceCarPro
    }
}
