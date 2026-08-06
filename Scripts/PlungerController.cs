using UnityEngine;

public class PlungerController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private KeyCode launchKey = KeyCode.Space;

    [Header("Launch Direction")]
    [SerializeField] private Transform launchDirection;

    [Header("Launch Force")]
    [SerializeField] private float minimumLaunchForce = 4f;
    [SerializeField] private float maximumLaunchForce = 18f;
    [SerializeField] private float maximumChargeTime = 1.5f;

    private Rigidbody currentBall;
    private float chargeTime;
    private bool isCharging;

    private void Update()
    {
        if (currentBall == null)
        {
            ResetCharge();
            return;
        }

        if (Input.GetKeyDown(launchKey))
        {
            isCharging = true;
            chargeTime = 0f;
        }

        if (isCharging && Input.GetKey(launchKey))
        {
            chargeTime += Time.deltaTime;
            chargeTime = Mathf.Min(chargeTime, maximumChargeTime);
        }

        if (isCharging && Input.GetKeyUp(launchKey))
        {
            LaunchBall();
        }
    }

    private void LaunchBall()
    {
        float chargePercentage;

        if (maximumChargeTime <= 0f)
        {
            chargePercentage = 1f;
        }
        else
        {
            chargePercentage =
                Mathf.Clamp01(chargeTime / maximumChargeTime);
        }

        float launchForce = Mathf.Lerp(
            minimumLaunchForce,
            maximumLaunchForce,
            chargePercentage);

        Vector3 direction = launchDirection != null
            ? launchDirection.forward
            : transform.forward;

        currentBall.WakeUp();

        currentBall.AddForce(
            direction.normalized * launchForce,
            ForceMode.Impulse);

        ResetCharge();
    }

    private void ResetCharge()
    {
        chargeTime = 0f;
        isCharging = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        BallController ball =
            other.GetComponentInParent<BallController>();

        if (ball == null)
        {
            return;
        }

        currentBall = ball.Rigidbody;
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentBall == null)
        {
            return;
        }

        if (other.attachedRigidbody != currentBall)
        {
            return;
        }

        currentBall = null;
        ResetCharge();
    }
}