using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenuController : MonoBehaviour
{

    //- Highest Score List UI
    [Header("HighScores")]
    public Text firstHighScore;
    public Text secandHighScore;
    public Text thiredHighScore;
    public Text fourthHighScore;
    public Text fifthHighScore;
    public Text sixthHighScore;
    public Text seventhHighScore;
    public Text eagthHighScore;
    public Text ninthHighScore;
    public Text tenthHighScore;

    //- Last Score List UI
    [Header("LastScores")]
    public Text lastScoreAcheved;
    public Text numberOfLineCleared;
    public Text lastLevelAcheved;
    public Text maxTimeAcheved;

    //- update at the level start
    void Start()
    {
        updateHighestScoreList();
        updateResultList();
    }

    //- Play Again Button
    public void playAgain()
    {
        SceneManager.LoadScene("Level");
    }

    //- Main Menu Button
    public void backToMainMenu()
    {
        SceneManager.LoadScene("NewGameMenu");
    }

    //- Exit Game Button
    public void QuitGame()
    {
        Application.Quit();
    }

    void OnApplicationQuit()
    {
        if(TutorialMenuController.engine != null)
        { 
            TutorialMenuController.engine.Disconnect();
            TutorialMenuController.engine = null;
        }
    }

    //- Highest Score List
    void updateHighestScoreList()
    {
        firstHighScore.text = PlayerPrefs.GetInt("HighScore1").ToString();
        secandHighScore.text = PlayerPrefs.GetInt("HighScore2").ToString();
        thiredHighScore.text = PlayerPrefs.GetInt("HighScore3").ToString();
        fourthHighScore.text = PlayerPrefs.GetInt("HighScore4").ToString();
        fifthHighScore.text = PlayerPrefs.GetInt("HighScore5").ToString();
        sixthHighScore.text = PlayerPrefs.GetInt("HighScore6").ToString();
        seventhHighScore.text = PlayerPrefs.GetInt("HighScore7").ToString();
        eagthHighScore.text = PlayerPrefs.GetInt("HighScore8").ToString();
        ninthHighScore.text = PlayerPrefs.GetInt("HighScore9").ToString();
        tenthHighScore.text = PlayerPrefs.GetInt("HighScore10").ToString();
    }

    //- Result List
    void updateResultList()
    {
        lastScoreAcheved.text = PlayerPrefs.GetInt("LastScore").ToString();
        numberOfLineCleared.text = PlayerPrefs.GetInt("numberOfLineCleared").ToString();
        lastLevelAcheved.text = PlayerPrefs.GetInt("lastLevelAcheved").ToString();
        maxTimeAcheved.text = PlayerPrefs.GetInt("maxTimeAcheved").ToString();
    }

}
