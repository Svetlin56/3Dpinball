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

    [Header("Visual")]
    [SerializeField] private Transform plungerVisual;
    [SerializeField] private float pullbackDistance = 1f;

    private Rigidbody currentBall;

    private float chargeTime;
    private bool isCharging;

    private Vector3 releasedVisualPosition;

    private void Awake()
    {
        if (plungerVisual != null)
        {
            releasedVisualPosition = plungerVisual.localPosition;
        }
    }

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
            ChargePlunger();
        }

        if (isCharging && Input.GetKeyUp(launchKey))
        {
            LaunchBall();
        }
    }

    private void ChargePlunger()
    {
        chargeTime += Time.deltaTime;

        chargeTime = Mathf.Min(
            chargeTime,
            maximumChargeTime);

        UpdatePlungerVisual();
    }

    private void LaunchBall()
    {
        float chargePercentage = GetChargePercentage();

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

    private float GetChargePercentage()
    {
        if (maximumChargeTime <= 0f)
        {
            return 1f;
        }

        return Mathf.Clamp01(
            chargeTime / maximumChargeTime);
    }

    private void UpdatePlungerVisual()
    {
        if (plungerVisual == null)
        {
            return;
        }

        float chargePercentage = GetChargePercentage();

        Vector3 pulledPosition =
            releasedVisualPosition
            - Vector3.forward * pullbackDistance;

        plungerVisual.localPosition = Vector3.Lerp(
            releasedVisualPosition,
            pulledPosition,
            chargePercentage);
    }

    private void ResetCharge()
    {
        chargeTime = 0f;
        isCharging = false;

        if (plungerVisual != null)
        {
            plungerVisual.localPosition =
                releasedVisualPosition;
        }
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