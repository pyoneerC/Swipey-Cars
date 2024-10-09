using System;
using TMPro;
using UnityEngine;

public class SwipeFollow : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI swipeForceText;

    private int _swipeForce;
    private bool _swiped;

    private Vector2 _swipeStart;
    private Vector2 _swipeEnd;
    private bool _swipeDetected = false;
    private float _maxSwipeForce = 1.0f;
    private const int WinThresholdMin = 3;
    private const int WinThresholdMax = 5;
    private bool _gameEnded;

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
        
        swipeForceText.text = $"Swipe force: {_swipeForce}";

        if (_swipeForce >= WinThresholdMin && _swipeForce <= WinThresholdMax)
        {
            EndGame(true);
        }
        else
        {
            EndGame(false);
        }

        _swiped = true;
    }

    private void EndGame(bool success)
    {
        _gameEnded = true;
        resultText.text = success ? "You won!" : "You lost!";
    }
}