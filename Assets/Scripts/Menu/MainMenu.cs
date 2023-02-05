using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    bool isPaused = false;
    public void PlayGame()
    {
        SceneManager.LoadScene("Environment 1107");
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Quit()
    {
        Application.Quit();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        }
        
        if (Input.GetKeyDown((KeyCode.P))) isPaused = !isPaused;
        if(isPaused)
        {
            Debug.Log("Pausa");
            isPaused = true;
            Time.timeScale = 0f;
        }
        else
        {
            Debug.Log("Nie ma pausy");
            isPaused = false;
            Time.timeScale = 1f;
        }
            

    }
    
    
}
    

