using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryLogic : MonoBehaviour
{
    public GameObject PurpleCarGameobjectVisual;
    public GameObject Truck;
    public GameObject GreenCar;
    public GameObject RaceCarPro;
    public GameObject RedRaceCar;
    public GameObject WhiteRaceCar;

    public GameObject RedRaceCarText;
    public GameObject WhiteRaceCarText;

    public Texture2D icon_cross;
    public Texture2D icon_checkmark;
    public Texture2D icon_green_circle;  // Icon for active car

    public AudioSource CarSelectionFeedbackSource;
    public AudioClip CarSelectionFeedbackClipError;
    public AudioClip CarSelectionFeedbackClipSuccess;

    private GameObject activeCar = null;  // Reference to the currently active car

    private void Start()
    {
        UpdateInventory();
    }

    private void UpdateInventory()
    {
        UpdateCarVisual(PurpleCarGameobjectVisual, BuyingStuffLogic.CarType.PurpleCar1);
        UpdateCarVisual(Truck, BuyingStuffLogic.CarType.Truck);
        UpdateCarVisual(GreenCar, BuyingStuffLogic.CarType.GreenCar);
        UpdateCarVisual(RaceCarPro, BuyingStuffLogic.CarType.RaceCarPro);

        UpdateLevelBasedCar(RedRaceCar, 20, RedRaceCarText);
        UpdateLevelBasedCar(WhiteRaceCar, 50, WhiteRaceCarText);
    }

    private void UpdateCarVisual(GameObject carGameObject, BuyingStuffLogic.CarType carType)
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

    private bool CheckIfCarOwned(BuyingStuffLogic.CarType carType)
    {
        int carsUnlocked = PlayerPrefs.GetInt("VehicleMap", 1);
        return (carsUnlocked & (1 << (int)carType)) != 0;
    }

    // Callback method for selecting a car
    private void SelectCar(GameObject carGameObject, BuyingStuffLogic.CarType carType)
    {
        if (!CheckIfCarOwned(carType))
        {
            CarSelectionFeedbackSource.PlayOneShot(CarSelectionFeedbackClipError);
            return;
        }

        if (activeCar != null)
        {
            // Reset the previously active car icon back to checkmark
            RawImage prevCarIcon = activeCar.GetComponentInChildren<RawImage>();
            prevCarIcon.texture = icon_checkmark;
        }

        // Set the new active car and update its icon to the green circle
        activeCar = carGameObject;
        RawImage carIcon = activeCar.GetComponentInChildren<RawImage>();
        carIcon.texture = icon_green_circle;
        PlayerPrefs.SetInt("SelectedVehicle", (int)carType);
        PlayerPrefs.Save();
        CarSelectionFeedbackSource.PlayOneShot(CarSelectionFeedbackClipSuccess);
    }

    // Callback methods for each car
    public void OnPurpleCarSelected() => SelectCar(PurpleCarGameobjectVisual, BuyingStuffLogic.CarType.PurpleCar1);
    public void OnTruckSelected() => SelectCar(Truck, BuyingStuffLogic.CarType.Truck);
    public void OnGreenCarSelected() => SelectCar(GreenCar, BuyingStuffLogic.CarType.GreenCar);
    public void OnRaceCarProSelected() => SelectCar(RaceCarPro, BuyingStuffLogic.CarType.RaceCarPro);
    public void OnRedRaceCarSelected() => SelectCar(RedRaceCar, BuyingStuffLogic.CarType.StartingCar); // Assuming RedRaceCar is StartingCar
    public void OnWhiteRaceCarSelected() => SelectCar(WhiteRaceCar, BuyingStuffLogic.CarType.StartingCar); // Adjust CarType if needed
}
