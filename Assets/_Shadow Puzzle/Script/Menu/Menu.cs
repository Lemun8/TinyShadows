using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject menuObj, settingsObj;
    public AudioSource onClick;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void PlayGame()
    {
        onClick.Play();
        SceneManager.LoadScene("Game");
    }
    public void OpenSettings()
    {
        onClick.Play();
        settingsObj.SetActive(true);
        menuObj.SetActive(false);
    }
    public void BackToMenu()
    {
        onClick.Play();
        settingsObj.SetActive(false);
        menuObj.SetActive(true);
    }
    public void QuitGame()
    {
        onClick.Play();
        Application.Quit();
        Debug.Log("This will quit the game");
    }
}