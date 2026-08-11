using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
public class FlipperController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] 
    private KeyCode controlKey = KeyCode.LeftArrow;

    [Header("Angles")]
    [SerializeField] 
    private float releasedAngle = 0f;
    [SerializeField] 
    private float pressedAngle = 45f;

    [Header("Spring")]
    [SerializeField] 
    private float springForce = 12000f;
    [SerializeField] 
    private float springDamper = 300f;

    private HingeJoint hingeJoint;
    private bool isPressed;

    private void Awake()
    {
        hingeJoint = GetComponent<HingeJoint>();
        hingeJoint.useSpring = true;

        SetTargetAngle(releasedAngle);
    }

    private void Update()
    {
        isPressed = Input.GetKey(controlKey);
    }

    private void FixedUpdate()
    {
        float targetAngle = isPressed
            ? pressedAngle
            : releasedAngle;

        SetTargetAngle(targetAngle);
    }

    private void SetTargetAngle(float targetAngle)
    {
        JointSpring spring = hingeJoint.spring;

        spring.spring = springForce;
        spring.damper = springDamper;
        spring.targetPosition = targetAngle;

        hingeJoint.spring = spring;
    }
}