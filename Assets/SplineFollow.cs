using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Splines;
using UnityEngine.UI;

public class SwipeFollow : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI swipeForceText;
    public TextMeshProUGUI endTextHint;
    public GameObject pauseButton;
    public Texture2D pauseButtonImageA;
    public Texture2D pauseButtonImageB;
    public GameObject muteButton;
    public Texture2D muteButtonImageA;
    public Texture2D muteButtonImageB;

    public Transform carTransform; // The car object
    public SplineContainer spline; // Your spline path
    public TextMeshProUGUI coinsText;
    public AudioSource swipeFeedbackSource;
    public AudioClip engineSoundStart;
    public AudioClip successSound;
    public AudioClip failSound;

    // Reference to all car prefabs
    public GameObject[] carPrefabs; // Array to hold car prefabs

    private int _swipeForce;
    private bool _swiped;
    private Vector2 _swipeStart;
    private Vector2 _swipeEnd;
    private bool _swipeDetected = false;
    private float _maxSwipeForce = 1.0f;
    private const int WinThresholdMin = 3;
    private const int WinThresholdMax = 5;
    private bool _gameEnded;
    private int _coins;
    private int _carsUnlocked;
    private int _levelsbeaten;

    private void Start()
    {
        Time.timeScale = 1;
        pauseButton.SetActive(true);
        _coins = PlayerPrefs.GetInt("Coins");
        _carsUnlocked = PlayerPrefs.GetInt("VehicleMap");
        coinsText.text = _coins.ToString();
        _levelsbeaten = PlayerPrefs.GetInt("LevelsBeaten");

        InitializeCarSelection(); // Initialize the car selection
    }

    private void InitializeCarSelection()
    {
        // Get the selected car index from PlayerPrefs
        int selectedCarIndex = PlayerPrefs.GetInt("SelectedVehicle");
        //clamp from 0 to carPrefabs.Length
        selectedCarIndex = Mathf.Clamp(selectedCarIndex, 0, carPrefabs.Length - 1);

        // Disable all cars first
        foreach (var carPrefab in carPrefabs)
        {
            carPrefab.SetActive(false);
        }

        // Check if selected index is valid
        if (selectedCarIndex >= 0 && selectedCarIndex < carPrefabs.Length)
        {
            // Enable the selected car
            carPrefabs[selectedCarIndex].SetActive(true);
            carTransform = carPrefabs[selectedCarIndex].transform; // Assign the transform
        }
        else
        {
            Debug.LogWarning("Selected car index is out of range: " + selectedCarIndex);
            carPrefabs[0].SetActive(true); // Fallback to default car
            carTransform = carPrefabs[0].transform;
        }
    }

    private void Update()
    {
        if (_coins != PlayerPrefs.GetInt("Coins"))
        {
            _coins = PlayerPrefs.GetInt("Coins");
            coinsText.text = _coins.ToString();
        }

        if (_gameEnded) return;
        if (!_swiped)
        {
            DetectSwipe();
        }
    }

    private void DetectSwipe()
    {
        if (Input.touchCount <= 0 || EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return;
        var touch = Input.GetTouch(0);
        switch (touch.phase)
        {
            case TouchPhase.Began:
                _swipeStart = touch.position;
                break;
            case TouchPhase.Ended:
                _swipeEnd = touch.position;
                PlayEngineSound();
                CalculateSwipeForce();
                break;
            case TouchPhase.Moved:
                break;
            case TouchPhase.Stationary:
                break;
            case TouchPhase.Canceled:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private IEnumerator PlayEngineSoundForDuration(AudioSource swipeFeedbackSource, AudioClip engineSoundStart, float duration)
    {
        // Play the sound
        swipeFeedbackSource.PlayOneShot(engineSoundStart);

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Stop the sound after the duration (if still playing)
        swipeFeedbackSource.Stop();
    }

// Call this method to play the sound for 5 seconds
    private void PlayEngineSound()
    {
        StartCoroutine(PlayEngineSoundForDuration(swipeFeedbackSource, engineSoundStart, 3.5f));
    }

    private void CalculateSwipeForce()
    {
        var swipeDelta = _swipeEnd - _swipeStart;
        var swipeDistance = swipeDelta.magnitude;

        var normalizedSwipe = swipeDistance / Screen.height;
        _swipeForce = Mathf.Clamp(Mathf.RoundToInt(normalizedSwipe * 10), 0, 10);

        swipeForceText.text = _swipeForce <= 0 ? "Swipe faster!" : $"Swipe force: {_swipeForce}";
        swipeForceText.color = _swipeForce <= 0 ? Color.red : Color.black;

        if (_swipeForce <= 0)
        {
            return;
        }

        EndGame(_swipeForce is >= WinThresholdMin and <= WinThresholdMax);

        _swiped = true;
    }

    private void EndGame(bool success)
    {
        _gameEnded = true;
        if (success)
        {
            MoveCarToEnd();
        }
        else
        {
            MoveCarToRandomLocation();
        }
    }

    private void MoveCarToEnd()
    {
        StartCoroutine(MoveCarOnSpline(1.0f, true));
    }

    private void MoveCarToRandomLocation()
    {
        var randomPosition = UnityEngine.Random.Range(0.1f, 0.9f);
        StartCoroutine(MoveCarOnSpline(randomPosition, false));
    }

    private System.Collections.IEnumerator MoveCarOnSpline(float targetPosition, bool success)
    {
        var currentTime = 0f;
        const float startPosition = 0f;
        var duration = UnityEngine.Random.Range(2f, 4f);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            var t = currentTime / duration;
            var newPosition = Mathf.Lerp(startPosition, targetPosition, t);

            carTransform.position = spline.EvaluatePosition(newPosition);

            carTransform.rotation = Quaternion.LookRotation(spline.EvaluateTangent(newPosition));
            carTransform.rotation *= Quaternion.Euler(0, 180, 0);

            yield return null;
        }

        resultText.text = success ? "You win!" : "You lost!";
        resultText.color = success ? Color.green : Color.red;

        if (success)
        {
            endTextHint.text = "Nice!";
        }
        else
            endTextHint.text = _swipeForce switch
            {
                < WinThresholdMin => "Too slow!",
                > WinThresholdMax => "Too fast!",
                _ => endTextHint.text
            };

        switch (success)
        {
            case false:
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    Invoke(nameof(ShowInterstitialAd), 1.0f);
                }
                break;
            case true:
                _coins += UnityEngine.Random.Range(10, 20);
                PlayerPrefs.SetInt("Coins", _coins);
                _levelsbeaten++;
                PlayerPrefs.SetInt("LevelsBeaten", _levelsbeaten);
                PlayerPrefs.Save();
                Invoke(nameof(GoToNextLevel), 2.0f);
                break;
        }

        swipeFeedbackSource.PlayOneShot(success ? successSound : failSound);
    }

    public void GoToNextLevel()
    {
        var randomLevel = UnityEngine.Random.Range(2, 5);
        SceneManager.LoadScene($"LVL{randomLevel}");
    }

    public void ShowInterstitialAd()
    {
        FindObjectOfType<InterstitialAdExample>().ShowAd();
    }

    public void RestartGame()
    {
        _gameEnded = false;
        _swiped = false;
        resultText.text = "";
        swipeForceText.text = "";
        FindObjectOfType<RewardedAdsButton>().ShowAd();

        Invoke(nameof(RestartLevel), 2.0f);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RedirectToShop()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void TogglePause()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;

        var rawImage = pauseButton.GetComponent<RawImage>();

        rawImage.texture = Time.timeScale == 0 ? pauseButtonImageB : pauseButtonImageA;
    }

    public void ToggleMute()
    {
        Camera.main.GetComponent<AudioListener>().enabled = !Camera.main.GetComponent<AudioListener>().enabled;

        var rawImage = muteButton.GetComponent<RawImage>();

        rawImage.texture = Camera.main.GetComponent<AudioListener>().enabled ? muteButtonImageA : muteButtonImageB;
    }
}
