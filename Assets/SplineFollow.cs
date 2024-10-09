using UnityEngine;
using UnityEngine.Splines; // Assuming you're using Unity's spline system

public class SplineFollow : MonoBehaviour
{
    public SplineContainer spline; // Reference to the spline container
    private float progress = 0f; // Track position along the spline (0 to 1)
    private bool swiped = false; // To check if the swipe is made
    private float swipeForce; // Force of the swipe

    // Swipe parameters
    private Vector2 swipeStart;
    private Vector2 swipeEnd;
    private bool swipeDetected = false;
    private float maxSwipeForce = 1.0f; // Max swipe force
    private float movementSpeed = 2f; // Speed at which to move along the spline
    private float winThresholdMin = 0.05f; // Minimum threshold for winning
    private float winThresholdMax = 1f; // Maximum threshold for winning
    private float loseThreshold = 1.0f; // Progress threshold for losing
    private float interpolationSpeed = 5f; // Speed for lerping

    private bool gameEnded = false; // Track if the game has ended

    void Start()
    {
        if (spline == null)
        {
            Debug.LogError("No SplineContainer found on this GameObject!");
        }
    }

    void Update()
    {
        if (!gameEnded)
        {
            // Detect swipe
            if (!swiped)
            {
                DetectSwipe();
            }
            else
            {
                // Move along the spline based on the swipe force
                MoveAlongSpline();
            }
        }
    }

    // Function to detect swipe input
    void DetectSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                swipeStart = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                swipeEnd = touch.position;
                swipeDetected = true;
                CalculateSwipeForce();
            }
        }
    }

    // Calculate the force based on swipe distance and direction
    void CalculateSwipeForce()
    {
        Vector2 swipeDelta = swipeEnd - swipeStart;
        float swipeDistance = swipeDelta.magnitude;

        // Normalize swipe distance to a value between 0 and maxSwipeForce
        swipeForce = Mathf.Clamp(swipeDistance / Screen.height, 0, maxSwipeForce);
        Debug.Log($"Swipe force: {swipeForce}");

        // Trigger movement after swipe detection
        swiped = true;
    }

    // Move the object along the spline
    void MoveAlongSpline()
    {
        // Calculate target progress based on swipe force
        float targetProgress = progress + (swipeForce * Time.deltaTime * movementSpeed);
        targetProgress = Mathf.Clamp(targetProgress, 0f, 1f); // Clamp to spline limits

        // Calculate new position and tangent
        Vector3 newPosition = spline.EvaluatePosition(targetProgress);
        Vector3 newTangent = spline.EvaluateTangent(targetProgress); // Get the tangent to align the object

        // Smoothly interpolate position
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * interpolationSpeed);
        transform.rotation = Quaternion.LookRotation(newTangent) * Quaternion.Euler(0, 180, 0); // Rotate the object to align with the tangent

        // Update progress
        progress = targetProgress;

        // Check win/lose conditions based on swipe force
        if (swipeForce >= winThresholdMin && swipeForce <= winThresholdMax && progress >= 1f)
        {
            EndGame(true); // Player wins
        }
        else if (progress >= loseThreshold)
        {
            EndGame(false); // Player loses
        }
    }

    // End game when player wins or loses
    void EndGame(bool success)
    {
        gameEnded = true;
        if (success)
        {
            Debug.Log("You won!");
        }
        else
        {
            Debug.Log("You lost!");
        }
    }
}
