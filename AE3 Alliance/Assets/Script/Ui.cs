using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ui : MonoBehaviour
{
    public void Game()
    {
        SceneManager.LoadScene("AE3");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
