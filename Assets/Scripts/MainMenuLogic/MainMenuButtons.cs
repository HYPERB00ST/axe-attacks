using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private string PlayGameSceneName;
    public void StartGame() {
        Debug.Log("Clicked");
        SceneManager.LoadScene(PlayGameSceneName, LoadSceneMode.Single);
    }
    public void QuitGame() {
        Application.Quit();
    }
}
