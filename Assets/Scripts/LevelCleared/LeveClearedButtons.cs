using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelClearedButtons : MonoBehaviour
{
    [SerializeField] private string NextLevelSceneName;

    public void NextLevelChange() {
        SceneManager.LoadScene(NextLevelSceneName, LoadSceneMode.Single);
    }
    public void QuitGame() {
        Application.Quit();
    }
}
