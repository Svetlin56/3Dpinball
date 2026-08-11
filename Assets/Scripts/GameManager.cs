using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Ball")]
    [SerializeField] 
    private BallController ball;
    [SerializeField] 
    private Transform ballSpawnPoint;
    [SerializeField] 
    private float respawnDelay = 1f;

    [Header("Game")]
    [SerializeField] 
    private int startingLives = 3;
    [SerializeField] 
    private KeyCode restartKey = KeyCode.R;

    [Header("Display")]
    [SerializeField] 
    private bool showGameStatusOnScreen = true;
    [SerializeField] 
    private int fontSize = 28;

    public int Lives { get; private set; }
    public bool IsGameOver { get; private set; }

    private bool isRespawning;
    private GUIStyle statusStyle;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        Lives = Mathf.Max(1, startingLives);

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
        }

        RespawnBallImmediately();
    }

    private void Update()
    {
        if (IsGameOver && Input.GetKeyDown(restartKey))
        {
            RestartGame();
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void HandleBallLost(BallController lostBall)
    {
        if (IsGameOver || isRespawning)
        {
            return;
        }

        if (lostBall != ball)
        {
            return;
        }

        StartCoroutine(HandleLostBallRoutine());
    }

    private IEnumerator HandleLostBallRoutine()
    {
        isRespawning = true;
        Lives--;

        if (Lives <= 0)
        {
            EndGame();
            yield break;
        }

        yield return new WaitForSeconds(respawnDelay);

        RespawnBallImmediately();
        isRespawning = false;
    }

    private void EndGame()
    {
        IsGameOver = true;
        isRespawning = false;

        if (ball == null)
        {
            return;
        }

        ball.Rigidbody.linearVelocity = Vector3.zero;
        ball.Rigidbody.angularVelocity = Vector3.zero;
        ball.Rigidbody.Sleep();
    }

    private void RespawnBallImmediately()
    {
        if (ball == null)
        {
            Debug.LogError("Ball is not assigned in GameManager.");
            return;
        }

        if (ballSpawnPoint == null)
        {
            Debug.LogError("Ball Spawn Point is not assigned in GameManager.");
            return;
        }

        ball.ResetBall(
            ballSpawnPoint.position,
            ballSpawnPoint.rotation);
    }

    public void RestartGame()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.buildIndex);
    }

    private void OnGUI()
    {
        if (!showGameStatusOnScreen)
        {
            return;
        }

        if (statusStyle == null)
        {
            statusStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = fontSize,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.UpperRight
            };
        }

        GUI.Label(
            new Rect(
                Screen.width - 320f,
                20f,
                300f,
                50f),
            $"Lives: {Mathf.Max(0, Lives)}",
            statusStyle);

        if (!IsGameOver)
        {
            return;
        }

        GUIStyle gameOverStyle = new GUIStyle(statusStyle)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = fontSize + 12
        };

        GUI.Label(
            new Rect(
                0f,
                Screen.height * 0.4f,
                Screen.width,
                120f),
            $"GAME OVER\nPress {restartKey} to restart",
            gameOverStyle);
    }
}