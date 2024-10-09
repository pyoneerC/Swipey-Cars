using UnityEngine;
using UnityEngine.Splines; // Assuming you're using Unity's spline system

public class SplineFollow : MonoBehaviour
{
    public SplineContainer spline; // Reference to the spline container
    private float progress = 0f; // To track the position along the spline (0 to 1)
    private bool swiped = false; // To check if the swipe is made
    private float swipeForce; // Force of the swipe

    // Swipe parameters
    private Vector2 swipeStart;
    private Vector2 swipeEnd;
    private bool swipeDetected = false;
    private float maxSwipeForce = 1.0f; // Max swipe force
    private float movementSpeed = 2f; // Base speed at which to move along the spline
    private float winThresholdMin = 0.3f; // Minimum threshold for winning
    private float winThresholdMax = 0.4f; // Maximum threshold for winning
    private float loseThreshold = 1.0f; // Progress threshold for losing
    private float interpolationSpeed = 5f; // Speed for lerping

    private bool gameEnded = false; // To track if the game has ended

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
                // Move along the spline after the swipe
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
        if (swipeForce > 0 && !gameEnded)
        {
            // Interpolate position along the spline based on swipe force
            float targetProgress = progress + (swipeForce * Time.deltaTime * movementSpeed);
            targetProgress = Mathf.Clamp(targetProgress, 0f, 1f);

            // Calculate current spline position
            Vector3 newPosition = spline.EvaluatePosition(targetProgress);
            Vector3 newTangent = spline.EvaluateTangent(targetProgress); // Get the tangent to align the object

            // Lerp to the new position for smooth movement
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * interpolationSpeed);
            transform.rotation = Quaternion.LookRotation(newTangent); // Rotate object along spline's direction

            // Update progress
            progress = targetProgress;

            // Check win/lose conditions based on progress
            if (progress >= winThresholdMax)
            {
                EndGame(true); // Player wins
            }
            else if (progress >= winThresholdMin)
            {
                // If between thresholds, still win but slightly away from the spline
                transform.position += newTangent * (swipeForce * 0.5f); // Move slightly away from the spline
            }
            else if (progress >= loseThreshold)
            {
                EndGame(false); // Player loses
            }
        }
        else if (swipeForce <= 0 && !gameEnded)
        {
            // If no swipe force, keep lerping to the last position on the spline
            Vector3 newPosition = spline.EvaluatePosition(progress);
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * interpolationSpeed);
        }
    }

    private void OnDrawGizmos()
    {
        if (spline != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(spline.EvaluatePosition(progress), 0.1f);
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
