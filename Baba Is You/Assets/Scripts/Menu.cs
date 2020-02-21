using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{

    public void Play()
    {
        PlayerPrefs.SetInt("Level", 0);
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }


}
