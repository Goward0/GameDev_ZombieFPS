using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game State UI")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI killText;

    [Header("Scoreboard UI")]
    public GameObject scoreboardPanel;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI wavesText;
    public TextMeshProUGUI scoreText;

    [Header("Game Over Stats")]
    public TextMeshProUGUI gameOverKillsText;
    public TextMeshProUGUI gameOverWavesText;
    public TextMeshProUGUI gameOverScoreText;
    public TextMeshProUGUI gameOverHighScoreText;

    private int waveNumber = 1;
    private int killCount = 0;
    private int totalScore = 0;
    private int highScore = 0;
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        Application.targetFrameRate = 60; 
        highScore = PlayerPrefs.GetInt("HighScore", 0);  // Load saved high score
        UpdateWaveText();
        UpdateKillText();
        UpdateScoreboard();
        gameOverPanel.SetActive(false);
        if (scoreboardPanel != null) scoreboardPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isGameOver && scoreboardPanel != null)
        {
            scoreboardPanel.SetActive(!scoreboardPanel.activeSelf);
            UpdateScoreboard();
        }
    }

    public void AddKill()
    {
        killCount++;
        totalScore += 5;
        UpdateKillText();
        UpdateScoreboard();
    }

public void NextWave(int wave)
{
    waveNumber = wave;
    totalScore += 50;
    UpdateWaveText();
    UpdateScoreboard();

    // Remove all ragdolls at the start of a wave
    GameObject[] ragdolls = GameObject.FindGameObjectsWithTag("DeadRagdoll");
    foreach (GameObject r in ragdolls)
    {
        Destroy(r);
    }
}
    public int CurrentWave => waveNumber;


    void UpdateWaveText()
    {
        if (waveText != null)
            waveText.text = "Wave: " + waveNumber;
    }

    void UpdateKillText()
    {
        if (killText != null)
            killText.text = "Kills: " + killCount;
    }

    void UpdateScoreboard()
    {
        if (killsText != null) killsText.text = "Kills: " + killCount;
        if (wavesText != null) wavesText.text = "Waves: " + waveNumber;
        if (scoreText != null) scoreText.text = "Score: " + totalScore;
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        Time.timeScale = 0f;

        if (gameOverPanel != null) gameOverPanel.SetActive(true);

        if (gameOverKillsText != null) gameOverKillsText.text = "Kills: " + killCount;
        if (gameOverWavesText != null) gameOverWavesText.text = "Waves: " + waveNumber;
        if (gameOverScoreText != null) gameOverScoreText.text = "Score: " + totalScore;

        // Update and save high score
        if (totalScore > highScore)
        {
            highScore = totalScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        if (gameOverHighScoreText != null)
            gameOverHighScoreText.text = "High Score: " + highScore;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

GameObject player = GameObject.FindGameObjectWithTag("Player");
if (player != null)
{
    PlayerMovementScript movement = player.GetComponent<PlayerMovementScript>();
    if (movement != null)
        movement.enabled = false;
}

    }

    public void RestartGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
