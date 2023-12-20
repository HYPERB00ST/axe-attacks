using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    private Collider playerCollider;
    private LevelManagerScript LevelManager;
    void Start() {
        playerCollider = GameObject.Find("Player Controller").GetComponent<CharacterController>();
        LevelManager = GameObject.Find("LevelManager").GetComponent<LevelManagerScript>();
    }
    void OnTriggerEnter(Collider collider) {
        if (collider == playerCollider) {
            if (LevelManager.Level < 5) {
                SceneManager.LoadScene("LevelClearedScene");
            }
            else {
                SceneManager.LoadScene("VictoryScene");
            }
        }        
    }
}
