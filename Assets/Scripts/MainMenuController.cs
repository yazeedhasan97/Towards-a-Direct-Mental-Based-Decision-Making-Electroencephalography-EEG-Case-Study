using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenuController : MonoBehaviour
{
    //- The Start Level Text
    public Text startLevelText;

    //- open and close the tutorial scroll view
    public GameObject container;

    //- PlayButton
    public void playNewGame()
    {
        //- chick if start from 0 or not to applay changes on the game
        if (Game.startLevel == 0) 
            Game.isStartAtLevelZero = true;
        else
            Game.isStartAtLevelZero = false;

        //- start the game
        SceneManager.LoadScene("Level");
    }

    public void restartTraining()
    {
        Destroy(GameObject.Find("TutorialMenuControllerScript"));
        SceneManager.LoadScene("TutorialMenu");
    }


    //- change the value of the StartLevelNumber while the slider value change
    public void changeStartLevelNumber(float Value)
    {
        Game.startLevel = (int)Value;
        startLevelText.text = Value.ToString();
    }

    //- ExitButton
    public void exitTheGame()
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

    //- open and close the tutorial scroll view
    public void openTut()
    {
        container.gameObject.SetActive(true);
    }

    public void closeTut()
    {
        container.gameObject.SetActive(false);
    }

}
