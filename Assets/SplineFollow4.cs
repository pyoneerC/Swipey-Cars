using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Splines;
using UnityEngine.UI;

public class SwipeFollow4 : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI swipeForceText;
    public TextMeshProUGUI endTextHint;
    public GameObject pauseButton;
    public Texture2D pauseButtonImageA;
    public Texture2D pauseButtonImageB;

    public Transform carTransform; // The car object
    public SplineContainer spline; // Your spline path, assuming you have a spline component

    private int _swipeForce;
    private bool _swiped;

    private Vector2 _swipeStart;
    private Vector2 _swipeEnd;
    private bool _swipeDetected = false;
    private float _maxSwipeForce = 1.0f;
    private const int WinThresholdMin = 1;
    private const int WinThresholdMax = 1;
    private bool _gameEnded;

    private void Start()
    {
        Time.timeScale = 1;
        pauseButton.SetActive(true);
    }

    private void Update()
    {
        if (_gameEnded) return;
        if (!_swiped)
        {
            DetectSwipe();
        }
    }

    private void DetectSwipe()
    {
        if (Input.touchCount <= 0) return;
        var touch = Input.GetTouch(0);
        switch (touch.phase)
        {
            case TouchPhase.Began:
                _swipeStart = touch.position;
                break;
            case TouchPhase.Ended:
                _swipeEnd = touch.position;
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
        var duration = UnityEngine.Random.Range(10f, 20f);

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
                Invoke(nameof(GoToNextLevel), 2.0f);
                break;
        }
    }

    public void GoToNextLevel()
    {
        var randomLevel = UnityEngine.Random.Range(1, 4);
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

}
