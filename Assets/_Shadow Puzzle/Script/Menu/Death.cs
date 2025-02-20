using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Death : MonoBehaviour
{
    // public AudioSource onClick;
    string curScene;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void PlayGame()
    {
        //onClick.Play();
        Time.timeScale = 1;
        curScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(curScene);
    }
    
    public void BackToMenu()
    {
        //onClick.Play();
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }
}
