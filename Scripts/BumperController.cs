using UnityEngine;

public class BumperController : MonoBehaviour
{
    [Header("Bumper")]
    [SerializeField] private float impulseForce = 10f;
    [SerializeField] private int scoreValue = 100;

    [Header("Optional Audio")]
    [SerializeField] private AudioSource hitSound;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody == null)
        {
            return;
        }

        BallController ball =
            collision.rigidbody.GetComponent<BallController>();

        if (ball == null)
        {
            return;
        }

        Vector3 pushDirection =
            ball.transform.position - transform.position;

        if (pushDirection.sqrMagnitude < 0.001f)
        {
            if (collision.contactCount == 0)
            {
                return;
            }

            pushDirection = collision.GetContact(0).normal;
        }

        pushDirection.Normalize();

        ball.Rigidbody.AddForce(
            pushDirection * impulseForce,
            ForceMode.Impulse);

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(scoreValue);
        }

        if (hitSound != null)
        {
            hitSound.Play();
        }
    }
}