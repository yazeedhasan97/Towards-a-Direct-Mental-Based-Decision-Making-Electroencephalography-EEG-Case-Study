using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Emotiv;

public class PauseMenuController : MonoBehaviour
{
    public Image neuralComplete;
    public Image DownComplete;
    public Image RotateComplete;
    public Image neuralNotComplete;
    public Image DownNotComplete;
    public Image RotateNotComplete;

    private void Start()
    {
        checkForTrainingComplete();
    }


    //- Main Menu Button
    public void backToMainMenu()
    {
        SceneManager.LoadScene("NewGameMenu");
    }

    //- Exit Button and desconnect the device
    public void QuitGame()
    {
        Application.Quit();
    }


    void OnApplicationQuit()
    {
        if (TutorialMenuController.engine != null)
        {
            TutorialMenuController.engine.Disconnect();
            TutorialMenuController.engine = null;
        }
    }

    //- updata UI
    public void checkForTrainingComplete()
    {
        if (TutorialMenuController.isNeutralTrained)
        {
            neuralComplete.gameObject.SetActive(true);
            neuralNotComplete.gameObject.SetActive(false);
        }
        else
        {
            neuralComplete.gameObject.SetActive(false);
            neuralNotComplete.gameObject.SetActive(true);
        }


        if (TutorialMenuController.isDownTrained)
        {
            DownComplete.gameObject.SetActive(true);
            DownNotComplete.gameObject.SetActive(false);
        }
        else
        {
            DownComplete.gameObject.SetActive(false);
            DownNotComplete.gameObject.SetActive(true);
        }


        if (TutorialMenuController.isRotateTrained)
        {
            RotateComplete.gameObject.SetActive(true);
            RotateNotComplete.gameObject.SetActive(false);
        }
        else
        {
            RotateComplete.gameObject.SetActive(false);
            RotateNotComplete.gameObject.SetActive(true);
        }

    }

}
