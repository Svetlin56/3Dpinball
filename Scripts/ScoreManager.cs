using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("Score Display")]
    [SerializeField] private bool showScoreOnScreen = true;
    [SerializeField] private Vector2 screenPosition = new Vector2(20f, 20f);
    [SerializeField] private int fontSize = 28;

    public int CurrentScore { get; private set; }

    public event Action<int> ScoreChanged;

    private GUIStyle scoreStyle;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void AddScore(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        CurrentScore += amount;
        ScoreChanged?.Invoke(CurrentScore);
    }

    public void ResetScore()
    {
        CurrentScore = 0;
        ScoreChanged?.Invoke(CurrentScore);
    }

    private void OnGUI()
    {
        if (!showScoreOnScreen)
        {
            return;
        }

        if (scoreStyle == null)
        {
            scoreStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = fontSize,
                fontStyle = FontStyle.Bold
            };
        }

        GUI.Label(
            new Rect(
                screenPosition.x,
                screenPosition.y,
                300f,
                50f),
            $"Score: {CurrentScore}",
            scoreStyle);
    }
}