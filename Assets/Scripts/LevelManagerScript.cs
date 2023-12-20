using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManagerScript : MonoBehaviour
{
    public int Level {get; set;} = 1;
    private static LevelManagerScript _instance = null;

    void CheckIfSingleton() {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        } 
        else {
            _instance = this;
        }
    }

    void Awake() {
        CheckIfSingleton();
        if (Level < 6) {
            DontDestroyOnLoad(this);
        }
    }
}
