using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Emotiv;

public class Game : MonoBehaviour
{
    //- the grid Hieght - Wiedth ---> Bourder
    static int gridWiedth = 10;//x
    static int gridHieght = 19;//y
    public static Transform[,] grid = new Transform[gridWiedth, gridHieght];
    //public TutorialMenuController mainScript;
    //- control the pause resume through the engine
    const float lastPauseDelay = .5f;
    float lastPause = 0;
    public static bool isPused = false;

    //for old highest score
    int highestScore1;
    int highestScore2;
    int highestScore3;
    int highestScore4;
    int highestScore5;
    int highestScore6;
    int highestScore7;
    int highestScore8;
    int highestScore9;
    int highestScore10;

    //control game speed and defculty
    public static float fallSpeed = 1; // Tetromino fall time
    public int currentLevel = 0;
    public int numLineCleared = 0;

    //the score variables
    int scoreOneLine = 10;
    int scoreTwoLine = 25;
    int scoreThreeLine = 80;
    int scoreFourLine = 200;
    int numberOfRowsThisTurn = 0;
    public static int currentScore = 0;

    //- UI
    [Header("UI")]
    public Canvas myCanvas;
    public Image pauseMenu;
    Image pauseMenuControl;
    //public GameObject pauseMenu;
    public Text HUDScore;
    public Text HUDLevel;
    public Text HUDClearedLine;
    public Text HUDTime;
    public Text HUDDeltaTime;

    //sound variables
    static AudioSource audioSource;
    public AudioClip ClearOneLine;
    public AudioClip ClearTwoLine;
    public AudioClip ClearThreeLine;
    public AudioClip ClearFourLine;

    //craete and previw next tetrminos
    GameObject nextTetromino;
    GameObject previewNextTetromino;
    public static bool gameStarted = false;
    Vector2 previewNextTetrominoposition = new Vector2(-9f, 1f);

    //the game start and start level
    public static bool isStartAtLevelZero;
    public static int startLevel;

    //- for version two of the game
    public int tetrominoLevel = 0;

    //- active the engine and sure the game is at start
    private void Awake()
    {
        //- Active the engine Facial Expression Sensing through code
        //- Add the reactions happen when a mental comand event happen // MEG - moves the Tetromino 
        if (TutorialMenuController.engine != null)
        { 
            TutorialMenuController.engine.FacialExpressionEmoStateUpdated += new EmoEngine.FacialExpressionEmoStateUpdatedEventHandler(moveAkword);
        }
        

    }

    // Start the game
    void Start()
    {
        //- some check before start the game
        isPused = false;
        Time.timeScale = 1;
        gameStarted = false;
        currentScore = 0;
        currentLevel = startLevel;
        audioSource = GetComponent<AudioSource>();
        Cursor.visible = false;
        lastPause = 0;

        //solve the problem of pause menu
        if (myCanvas == null)
        {
            myCanvas = GameObject.Find("HUDCanvas").GetComponent<Canvas>();
        }
        pauseMenuControl =(Image)Instantiate(pauseMenu);
        pauseMenuControl.transform.SetParent( myCanvas.gameObject.transform,false);
        pauseMenuControl.gameObject.SetActive(false);

        //- the game start here
        initializeHighestScore();
        spwanNextTetromino();

        //- for version two of the game
        tetrominoLevel = 1;

        

    }

    //- update scores and lists and check for pause or resume 
    void initializeHighestScore()
    {
        highestScore1 = PlayerPrefs.GetInt("HighScore1");
        highestScore2 = PlayerPrefs.GetInt("HighScore2");
        highestScore3 = PlayerPrefs.GetInt("HighScore3");
        highestScore4 = PlayerPrefs.GetInt("HighScore4");
        highestScore5 = PlayerPrefs.GetInt("HighScore5");
        highestScore6 = PlayerPrefs.GetInt("HighScore6");
        highestScore7 = PlayerPrefs.GetInt("HighScore7");
        highestScore8 = PlayerPrefs.GetInt("HighScore8");
        highestScore9 = PlayerPrefs.GetInt("HighScore9");
        highestScore10 = PlayerPrefs.GetInt("HighScore10");
    }

    void updateHighestScoreList()
    {
        if (highestScore1 <= currentScore)
        {
            PlayerPrefs.SetInt("HighScore10", highestScore9);
            PlayerPrefs.SetInt("HighScore9", highestScore8);
            PlayerPrefs.SetInt("HighScore8", highestScore7);
            PlayerPrefs.SetInt("HighScore7", highestScore6);
            PlayerPrefs.SetInt("HighScore6", highestScore5);
            PlayerPrefs.SetInt("HighScore5", highestScore4);
            PlayerPrefs.SetInt("HighScore4", highestScore3);
            PlayerPrefs.SetInt("HighScore3", highestScore2);
            PlayerPrefs.SetInt("HighScore2", highestScore1);
            PlayerPrefs.SetInt("HighScore1", currentScore);
        }
        else if (highestScore2 < currentScore)
        {
            PlayerPrefs.SetInt("HighScore10", highestScore9);
            PlayerPrefs.SetInt("HighScore9", highestScore8);
            PlayerPrefs.SetInt("HighScore8", highestScore7);
            PlayerPrefs.SetInt("HighScore7", highestScore6);
            PlayerPrefs.SetInt("HighScore6", highestScore5);
            PlayerPrefs.SetInt("HighScore5", highestScore4);
            PlayerPrefs.SetInt("HighScore4", highestScore3);
            PlayerPrefs.SetInt("HighScore3", highestScore2);
            PlayerPrefs.SetInt("HighScore2", currentScore);
        }
        else if (highestScore3 < currentScore)
        {
            PlayerPrefs.SetInt("HighScore10", highestScore9);
            PlayerPrefs.SetInt("HighScore9", highestScore8);
            PlayerPrefs.SetInt("HighScore8", highestScore7);
            PlayerPrefs.SetInt("HighScore7", highestScore6);
            PlayerPrefs.SetInt("HighScore6", highestScore5);
            PlayerPrefs.SetInt("HighScore5", highestScore4);
            PlayerPrefs.SetInt("HighScore4", highestScore3);
            PlayerPrefs.SetInt("HighScore3", currentScore);
        }
        else if (highestScore4 < currentScore)
        {
            PlayerPrefs.SetInt("HighScore10", highestScore9);
            PlayerPrefs.SetInt("HighScore9", highestScore8);
            PlayerPrefs.SetInt("HighScore8", highestScore7);
            PlayerPrefs.SetInt("HighScore7", highestScore6);
            PlayerPrefs.SetInt("HighScore6", highestScore5);
            PlayerPrefs.SetInt("HighScore5", highestScore4);
            PlayerPrefs.SetInt("HighScore4", currentScore);
        }
        else if (highestScore5 < currentScore)
        {
            PlayerPrefs.SetInt("HighScore10", highestScore9);
            PlayerPrefs.SetInt("HighScore9", highestScore8);
            PlayerPrefs.SetInt("HighScore8", highestScore7);
            PlayerPrefs.SetInt("HighScore7", highestScore6);
            PlayerPrefs.SetInt("HighScore6", highestScore5);
            PlayerPrefs.SetInt("HighScore5", currentScore);
        }
        else if (highestScore6 < currentScore)
        {
            PlayerPrefs.SetInt("HighScore10", highestScore9);
            PlayerPrefs.SetInt("HighScore9", highestScore8);
            PlayerPrefs.SetInt("HighScore8", highestScore7);
            PlayerPrefs.SetInt("HighScore7", highestScore6);
            PlayerPrefs.SetInt("HighScore6", currentScore);
        }
        else if (highestScore7 < currentScore)
        {
            PlayerPrefs.SetInt("HighScore10", highestScore9);
            PlayerPrefs.SetInt("HighScore9", highestScore8);
            PlayerPrefs.SetInt("HighScore8", highestScore7);
            PlayerPrefs.SetInt("HighScore7", currentScore);
        }
        else if (highestScore8 < currentScore)
        {
            PlayerPrefs.SetInt("HighScore10", highestScore9);
            PlayerPrefs.SetInt("HighScore9", highestScore8);
            PlayerPrefs.SetInt("HighScore8", currentScore);
        }
        else if (highestScore9 < currentScore)
        {
            PlayerPrefs.SetInt("HighScore10", highestScore9);
            PlayerPrefs.SetInt("HighScore9", currentScore);
        }
        else
        {
            PlayerPrefs.SetInt("HighScore10", currentScore);
        }
    }

    void updateResultList()
    {
        PlayerPrefs.SetInt("LastScore",currentScore);
        PlayerPrefs.SetInt("numberOfLineCleared", numLineCleared);
        PlayerPrefs.SetInt("lastLevelAcheved", currentLevel);
        PlayerPrefs.SetInt("maxTimeAcheved", (int)Time.timeSinceLevelLoad);
    }

    //- update scoring and other data and also wait for pause
    private void Update()
    {
        updateScore();
        updateUI();
        updateLevel();
        updateSpeed();
        checkUserInput();

    }

    //- resume or pause the game
    public void checkUserInput()
    {
        //- end game //- add for testing porpse
        if (Input.GetKeyUp(KeyCode.O))
        {
            if (Time.timeScale == 1)
            {
                gameOver();
            }
        }


        if (Input.GetKeyUp(KeyCode.P))
        {
            if (Time.timeScale == 1)
            {
                pauseGame();
            }
            else
            {
                resumeGame();
            }
        }
    }

    public void pauseGame()
    {
        if (pauseMenuControl == null)
        {
            if (myCanvas == null)
            {
                myCanvas = GameObject.Find("HUDCanvas").GetComponent<Canvas>();
            }
            pauseMenuControl = (Image)Instantiate(pauseMenu);
            pauseMenuControl.transform.SetParent(myCanvas.gameObject.transform, false);
        }
        Time.timeScale = 0;
        isPused = true;
        audioSource.Pause();
        pauseMenuControl.gameObject.SetActive(true);
        Cursor.visible = true;
    }

    public void resumeGame()
    {
        if (pauseMenuControl == null)
        {
            if (myCanvas == null)
            { 
                myCanvas = GameObject.Find("HUDCanvas").GetComponent<Canvas>();
            }
            pauseMenuControl = (Image)Instantiate(pauseMenu);
            pauseMenuControl.transform.SetParent(myCanvas.gameObject.transform, false);
        }
        Time.timeScale = 1;
        isPused = false;
        audioSource.Play();
        pauseMenuControl.gameObject.SetActive(false);
        Cursor.visible = false;

    }

    //- gurantee the user never out of the grid
    public bool checkIsInInsideGrid(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < gridWiedth && (int)pos.y >= 0);
    }

    //- to ensure that Tetromino  always moves in an integer destance in the game
    public Vector2 Round (Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    //spwan anext tetromino
    public void spwanNextTetromino()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            //this way is used to instantiate a random object that is not add to the scene yet
            //the Load method requaiered the "path" (passed by getRandomTetromino) for the resource as "relative" to the "Resources" folder in the assets
            GameObject nextTetromino = (GameObject)Instantiate(Resources.Load(getRandomTetromino(), typeof(GameObject)) , new Vector2(5.0f, 20f), Quaternion.identity);
            previewNextTetromino = (GameObject)Instantiate(Resources.Load(getRandomTetromino(), typeof(GameObject)) , previewNextTetrominoposition, Quaternion.identity);
            previewNextTetromino.GetComponent<TetrominoController>().enabled = false;

        }
        else
        {
            //- the preview tetromino become the active tetromino and Instantiate new preview tetromino
            previewNextTetromino.transform.localPosition = new Vector2(5.0f, 20f);
            nextTetromino = previewNextTetromino;
            nextTetromino.GetComponent<TetrominoController>().enabled = true;

            previewNextTetromino = (GameObject)Instantiate(Resources.Load(getRandomTetromino(), typeof(GameObject)) , previewNextTetrominoposition, Quaternion.identity);
            previewNextTetromino.GetComponent<TetrominoController>().enabled = false;
        }
    }

    string getRandomTetromino()
    {
        //- for version two of the game
        int randomTetromino;
        if (tetrominoLevel < 8) { 
            randomTetromino = Random.Range(1,tetrominoLevel+1);
        }
        else
        {
            //- for version one just keep this line and remove the entaire if statment
            randomTetromino = Random.Range(1, 8);
        }

        //path
        string randomTetrominoName = "";
        switch (randomTetromino)
        {
            case 1:
                randomTetrominoName = "SquareTetromino";
                break;
            case 2:
                randomTetrominoName = "ITetromino";
                break;
            case 3:
                randomTetrominoName = "LTetromino";
                break;
            case 4:
                randomTetrominoName = "JTetromino";
                break;
            case 5:
                randomTetrominoName = "STetromino";
                break;
            case 6:
                randomTetrominoName = "ZTetromino";
                break;
            case 7:
                randomTetrominoName = "TTetromino";
                break;

        }
        return randomTetrominoName;

    }

    //- update the grid state as tetriminos moveed , deleted and spwaned
    public void updateGrid(TetrominoController tetromino)
    {
        for (int i = 0; i < gridWiedth; i++)
        {
            for (int j = 0; j < gridHieght; j++)
            {
                if(grid[i,j] != null)
                {
                    if(grid[i,j].parent == tetromino.transform)
                    {
                        grid[i, j] = null;
                    }
                }
            }
        }
        foreach (Transform mino in tetromino.transform)
        {
            Vector2 pos = Round(mino.position);
            if (pos.y < gridHieght)
            {
                grid[(int)pos.x, (int)pos.y] = mino;
            }
        }
    }

    public Transform getTransformAtGridPosition(Vector2 pos)
    {
        if(pos.y > gridHieght - 1)
        {
            return null;
        }
        else
        {
            return grid[(int)pos.x, (int)pos.y];
        }
    }

    //- delete full rows and move all other rows one row down
    public bool isFullRowAt(int y)
    {
        for (int x = 0; x < gridWiedth; x++)
        {
            if(grid[x,y] == null)
            {
                return false;
            }
        }
        numberOfRowsThisTurn++;
        return true;
    }

    public void deleteMinoAt(int y)
    {
        for (int x = 0; x < gridWiedth; x++)
        {
                Destroy(grid[x,y].gameObject);
                grid[x, y] = null;
        }
    }

    public void moveOneRowDown(int y)
    {
        for (int x = 0; x < gridWiedth; x++)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += Vector3.down;
            }
        }
    }

    public void moveAllRowsDown(int y)
    {
        for(int i = y; i < gridHieght; i++)
        {
            moveOneRowDown(i);
        }
    }

    //- this method continusly check if there is a full rows //- the start of the previos section of methods
    public void deleteFullRows()
    {
        for (int i = 0; i < gridHieght; i++)
        {
            if (isFullRowAt(i))
            {
                deleteMinoAt(i);
                moveAllRowsDown(i+1);
                i--;
            }
        }

    }

    //- call to chick Game Over - player losser
    public bool checkIsAboveGrid(TetrominoController obj)
    {
        for (int x = 0; x < gridWiedth; x++)
        {
            foreach (Transform mino in obj.transform)
            {
                Vector2 pos = Round(mino.position);
                if (pos.y > gridHieght - 1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    //- when this object is destroyed delete the subsecribation
    private void OnDestroy()
    {
        if (TutorialMenuController.engine != null)
        {
            TutorialMenuController.engine.FacialExpressionEmoStateUpdated -= new EmoEngine.FacialExpressionEmoStateUpdatedEventHandler(moveAkword);
        }
    }

    public void gameOver() {
        updateHighestScoreList();
        updateResultList();
        Cursor.visible = true;
        SceneManager.LoadScene("NewGameOverMenu");
    }
    
    //- score and sound methods //- in the modifid version of the game the next methods change(expands) the types of fallings Tetromino
    void updateScore()
    {
        switch (numberOfRowsThisTurn)
        {
            case 1:
                oneLineCleared();
                break;
            case 2:
                twoLineCleared();
                break;
            case 3:
                threeLineCleared();
                break;
            case 4:
                fourLineCleared();
                break;
            default:
                numberOfRowsThisTurn = 0;
                break;
        }
        numberOfRowsThisTurn = 0;
    }


    void oneLineCleared()
    {
        currentScore += scoreOneLine;
        audioSource.PlayOneShot(ClearOneLine);
        numLineCleared++;
        //- for version two of the game
        tetrominoLevel++;
    }

    void twoLineCleared()
    {
        currentScore += scoreTwoLine;
        audioSource.PlayOneShot(ClearTwoLine);
        numLineCleared += 2;
        //- for version two of the game
        tetrominoLevel++;
    }

    void threeLineCleared()
    {
        currentScore += scoreThreeLine;
        audioSource.PlayOneShot(ClearThreeLine);
        numLineCleared += 3;
        //- for version two of the game
        tetrominoLevel++;
    }

    void fourLineCleared()
    {
        currentScore += scoreFourLine;
        audioSource.PlayOneShot(ClearFourLine);
        numLineCleared += 4;
        //- for version two of the game
        tetrominoLevel++;
    }

    //- update UI during the game
    void updateUI()
    {
        HUDScore.text = currentScore.ToString();
        HUDLevel.text = currentLevel.ToString();
        HUDClearedLine.text = numLineCleared.ToString();
        HUDTime.text = Time.timeSinceLevelLoad.ToString();
        HUDDeltaTime.text = Time.deltaTime.ToString();
    }

    //incresing game defculty and level
    void updateLevel()
    {
        if (isStartAtLevelZero || (!isStartAtLevelZero && (numLineCleared / 10) > startLevel))
        {
            currentLevel = numLineCleared / 10;
        }
    }

    void updateSpeed()
    {
        fallSpeed = 1.0f - ((float)currentLevel * 0.1f);
    }

    //- control using the engine
    //- pause and resume
    void checkUserBlink()
    {
        if (lastPause < lastPauseDelay)
        {
            lastPause += Time.timeSinceLevelLoad;
            return;
        }

        if (Time.timeScale == 1)
        {
            pauseGame();
            lastPause = 0;
        }
        else
        {
            resumeGame();
            lastPause = 0;
        }
    }

    //- the two methods called by TetrominoController to move the Tetromino left and right
    private void moveLeftMentally()
    {
        GameObject currentTetromino = findObj();
        currentTetromino.GetComponent<TetrominoController>().moveLeftMentally();
    }

    private void moveRightMentally()
    {
        GameObject currentTetromino = findObj();
        currentTetromino.GetComponent<TetrominoController>().moveRightMentally();
    }

    //- the reaction method add to the engine in this script
    void moveAkword(object sender, EmoStateUpdatedEventArgs e)
    {
        EmoState es = e.emoState;

        if (es.FacialExpressionIsBlink())
        {
            checkUserBlink();
        }

        if (es.FacialExpressionGetLowerFaceAction() == EdkDll.IEE_FacialExpressionAlgo_t.FE_SMIRK_RIGHT || es.FacialExpressionGetUpperFaceAction() == EdkDll.IEE_FacialExpressionAlgo_t.FE_FROWN)
        {
            moveRightMentally();
        }
        else if (es.FacialExpressionGetLowerFaceAction() == EdkDll.IEE_FacialExpressionAlgo_t.FE_SMIRK_LEFT || es.FacialExpressionGetUpperFaceAction() == EdkDll.IEE_FacialExpressionAlgo_t.FE_SUPRISE)
        {
            moveLeftMentally();
        }
    }

    //- called by moveRightMentally() and moveLeftMentally() in this script
    GameObject findObj()
    {
        GameObject[] activeTetrominos = GameObject.FindGameObjectsWithTag("Player");
        GameObject currentTetromino=null;
        foreach (GameObject ex in activeTetrominos)
        {
            if (ex.GetComponent<TetrominoController>().isActiveAndEnabled)
            {
                currentTetromino = ex;
            }
        }
        return currentTetromino;
    }

}
