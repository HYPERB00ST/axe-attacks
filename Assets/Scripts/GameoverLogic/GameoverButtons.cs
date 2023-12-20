using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverButtons : MonoBehaviour
{
    [SerializeField] private string MainMenuName;
    public void GotoMainMenu() {
        SceneManager.LoadScene(MainMenuName, LoadSceneMode.Single);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
