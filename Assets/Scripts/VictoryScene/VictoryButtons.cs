using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryButtons : MonoBehaviour
{
    [SerializeField] private string MainMenuSceneName;

    public void GoToMainMenu() {
        SceneManager.LoadScene(MainMenuSceneName, LoadSceneMode.Single);
    }
    public void QuitGame() {
        Application.Quit();
    }
}
