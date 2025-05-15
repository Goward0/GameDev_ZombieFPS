using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    public TMP_Dropdown mapDropdown;

    [Tooltip("Scene names must match those in Build Settings")]
    public List<string> mapSceneNames = new List<string>();

    void Start()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore;

        if (mapDropdown != null && mapSceneNames.Count > 0)
        {
            mapDropdown.ClearOptions();
            mapDropdown.AddOptions(mapSceneNames);
        }
    }

    public void StartGame()
    {
        if (mapSceneNames.Count > 0 && mapDropdown != null)
        {
            int selectedIndex = mapDropdown.value;
            string selectedScene = mapSceneNames[selectedIndex];
            Debug.Log("Loading scene: " + selectedScene);
            SceneManager.LoadScene(selectedScene);
        }
        else
        {
            Debug.LogWarning("No map selected or dropdown not set.");
        }
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
