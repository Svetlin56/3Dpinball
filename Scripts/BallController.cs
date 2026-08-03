using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maximumSpeed = 25f;

    [Header("Lost Ball Detection")]
    [SerializeField] private float lostBallHeight = -5f;

    private Rigidbody ballRigidbody;
    private bool isLost;

    public Rigidbody Rigidbody => ballRigidbody;

    private void Awake()
    {
        ballRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        LimitSpeed();
        CheckForLostBall();
    }

    private void LimitSpeed()
    {
        float maximumSpeedSquared = maximumSpeed * maximumSpeed;

        if (ballRigidbody.linearVelocity.sqrMagnitude <= maximumSpeedSquared)
        {
            return;
        }

        ballRigidbody.linearVelocity =
            ballRigidbody.linearVelocity.normalized * maximumSpeed;
    }

    private void CheckForLostBall()
    {
        if (isLost)
        {
            return;
        }

        if (transform.position.y >= lostBallHeight)
        {
            return;
        }

        isLost = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.HandleBallLost(this);
        }
        else
        {
            Debug.LogWarning("GameManager is missing from the scene.");
        }
    }

    public void ResetBall(Vector3 position, Quaternion rotation)
    {
        transform.SetPositionAndRotation(position, rotation);

        ballRigidbody.linearVelocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;

        ballRigidbody.Sleep();

        isLost = false;
    }

    public void WakeBall()
    {
        ballRigidbody.WakeUp();
    }
}