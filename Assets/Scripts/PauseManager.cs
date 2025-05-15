using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    public Button resumeButton;
    public Button quitButton;

    private bool isPaused = false;
    private PlayerMovementScript movementScript;


    public void Start()
    {
        // Disable pause panel initially
        if (pausePanel != null)
            pausePanel.SetActive(false);

        // Link buttons
        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);

        // Get reference to the player's movement script
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            movementScript = player.GetComponent<PlayerMovementScript>();
        }
        else
        {
            Debug.LogWarning("PauseManager: No GameObject with tag 'Player' found.");
        }
    }

   public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
{
    isPaused = true;

    if (pausePanel != null)
        pausePanel.SetActive(true);

    Time.timeScale = 0f;

    if (movementScript != null)
        movementScript.enabled = false;

    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;

    AudioListener.pause = true;
}


    public void ResumeGame()
{
    isPaused = false;

    if (pausePanel != null)
        pausePanel.SetActive(false);

    Time.timeScale = 1f;

    if (movementScript != null)
        movementScript.enabled = true;

    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;

    // Unmute audio
    AudioListener.pause = false;
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
