using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Emotiv;

public class TetrominoController : MonoBehaviour
{
    //- game speed variables
    float fall = 0;
    float fallSpeed ;

    float continuasVerticalSpeed = .3f;
    float continuasHorizntalSpeed = .1f;

    float verticalSpeedTimer = 0;
    float horizntalSpeedTimer = 0;

    float buttenDownMaxDelay = .15f;
    float buttenDownMaxTimerHorezntal = 0;
    float buttenDownMaxTimerVertical = 0;

    bool moveImmediateHorizantal = false;
    bool moveImmediateVertical = false;

    //- game allowed move variables
    public bool allowedRotation = true;
    public bool limitRotation = false;

    //score variables
    int tetrominoFastLandScore = 50;
    float tetrominoLandTime = 1;

    //sound variables
    AudioSource audioSource;
    public AudioClip moveSound;
    public AudioClip rotateSound;
    public AudioClip landSound;

    //- control moves Mentally - game speed varibles
    float lastVerticalMoveDelay = .005f;
    float lastVerticalMove = 0;

    float lastHorizontalMoveDelay = .005f;
    float lastHorizontalMove = 0;

    float lastRotateDelay = .005f;
    float lastRotate = 0;


    //- active the engine and audio
    void Awake()
    {
        //- Active the engine Mental Command Sensing through code
        //- Add the reactions happen when a mental comand event happen // EEG - moves the Tetromino 
        if (TutorialMenuController.engine != null)
        {
            TutorialMenuController.engine.MentalCommandEmoStateUpdated += new EmoEngine.MentalCommandEmoStateUpdatedEventHandler(MovingObjectByMind);
        }

    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    void updateFallSpeed()
    {
        fallSpeed = Game.fallSpeed;
    }

    //- contenuas check for user input
    void Update()
    {
        if(!Game.isPused)
        {
            ChickUserInput();
            updateFastLandScore();
            updateFallSpeed();
        }
    }

    //game movement methods
    void ChickUserInput()
    {
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            horizntalSpeedTimer = 0;
            
            buttenDownMaxTimerHorezntal = 0;
       
            moveImmediateHorizantal = false;
            
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            verticalSpeedTimer = 0;

            moveImmediateVertical = false;

            buttenDownMaxTimerVertical = 0;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveRight();
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveLeft();

        }
        if (Input.GetKey(KeyCode.DownArrow) || Time.time - fall >= fallSpeed)
        {
            moveDown();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            rotate();
        }

    }

    void moveRight()
    {
        if (moveImmediateHorizantal)
        {
            if (buttenDownMaxTimerHorezntal < buttenDownMaxDelay)
            {
                buttenDownMaxTimerHorezntal += Time.deltaTime;
                return;
            }

            if (horizntalSpeedTimer < continuasHorizntalSpeed)
            {
                horizntalSpeedTimer += Time.deltaTime;
                return;
            }
        }

        if (!moveImmediateHorizantal)
        {
            moveImmediateHorizantal = true;
        }
        horizntalSpeedTimer = 0;
        transform.position += Vector3.right;
        if (!checkIsInValidPosition())
        {
            transform.position += Vector3.left;
        }
        else
        {
            FindObjectOfType<Game>().updateGrid(this);
            playMoveSound();
        }
    }

    void moveLeft()
    {
        if (moveImmediateHorizantal)
        {
            if (buttenDownMaxTimerHorezntal < buttenDownMaxDelay)
            {
                buttenDownMaxTimerHorezntal += Time.deltaTime;
                return;
            }

            if (horizntalSpeedTimer < continuasHorizntalSpeed)
            {
                horizntalSpeedTimer += Time.deltaTime;
                return;
            }
        }

        if (!moveImmediateHorizantal)
        {
            moveImmediateHorizantal = true;
        }
        horizntalSpeedTimer = 0;
        transform.position += Vector3.left;
        if (!checkIsInValidPosition())
        {
            transform.position += Vector3.right;
        }
        else
        {
            FindObjectOfType<Game>().updateGrid(this);
            playMoveSound();
        }
    }

    void moveDown()
    {
        if (moveImmediateVertical)
        {
            if (buttenDownMaxTimerVertical < buttenDownMaxDelay)
            {
                buttenDownMaxTimerVertical += Time.deltaTime;
                return;
            }
            if (verticalSpeedTimer < continuasVerticalSpeed)
            {
                verticalSpeedTimer += Time.deltaTime;
                return;
            }
        }
        if (!moveImmediateVertical)
        {
            moveImmediateVertical = true;
        }
        verticalSpeedTimer = 0;
        transform.position += Vector3.down;
        if (!checkIsInValidPosition())
        {
            transform.position += Vector3.up;
            FindObjectOfType<Game>().deleteFullRows();
            if (FindObjectOfType<Game>().checkIsAboveGrid(this))
            {
                FindObjectOfType<Game>().gameOver();
            }
            playLandSound();
            Game.currentScore += tetrominoFastLandScore;
            enabled = false; //here i disable the script not the gameObject
            FindObjectOfType<Game>().spwanNextTetromino();

        }
        else
        {
            FindObjectOfType<Game>().updateGrid(this);
            playMoveSound();
        }
        fall = Time.time;
    }

    void rotate()
    {
        if (allowedRotation)
        {
            if (limitRotation)
            {
                if (transform.rotation.eulerAngles.z >= 90)
                {
                    transform.Rotate(0, 0, -90);
                }
                else
                {
                    transform.Rotate(0, 0, 90);
                }
            }
            else
            {
                transform.Rotate(0, 0, 90);
            }
            if (!checkIsInValidPosition())
            {
                if (limitRotation)
                {
                    if (transform.rotation.eulerAngles.z >= 90)
                    {
                        transform.Rotate(0, 0, -90);
                    }
                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }
                }
                else
                {
                    transform.Rotate(0, 0, -90);
                }
            }
            playRotateSound();
        }
    }

    //- gurantee the user never out of the grid
    bool checkIsInValidPosition()
    {
        foreach(Transform mino in transform)
        {
            Vector2 pos = FindObjectOfType<Game>().Round(mino.position);
            if (FindObjectOfType<Game>().checkIsInInsideGrid(pos) == false)
            {
                return false;
            }
            if (FindObjectOfType<Game>().getTransformAtGridPosition(pos) != null && FindObjectOfType<Game>().getTransformAtGridPosition(pos).parent != transform)
            {
                return false;
            }

        }
        return true;
    }

    //score methods
    void updateFastLandScore()
    {
        if (tetrominoLandTime <= 1)
        {
            tetrominoLandTime += Time.deltaTime;
        }
        else
        {
            tetrominoLandTime = 0;
            tetrominoFastLandScore = Mathf.Max(tetrominoFastLandScore - 5, 0);
        }
    }

    //audio methods
    void playMoveSound()
    {
        audioSource.PlayOneShot(moveSound);
    }

    void playRotateSound()
    {
        audioSource.PlayOneShot(rotateSound);
    }

    void playLandSound()
    {
        audioSource.PlayOneShot(landSound);
    }

    //- EEG control throgh the emotive
    void MovingObjectByMind(object sender, EmoStateUpdatedEventArgs e)
    {
        EmoState es = e.emoState;
        Debug.Log("Corrent action:" + es.MentalCommandGetCurrentAction().ToString());
        if (!Game.isPused)
        {
            Debug.Log(es.MentalCommandGetCurrentActionPower());
            if (es.MentalCommandGetCurrentAction() == EdkDll.IEE_MentalCommandAction_t.MC_PUSH)
            {
                moveDownMentally();
            }
            else if (es.MentalCommandGetCurrentAction() == EdkDll.IEE_MentalCommandAction_t.MC_PULL)
            {
                rotateMentally();
            }
        }
    }

    public void rotateMentally()
    {
        if (enabled)
        {
            if (lastRotate < lastRotateDelay)
            {
                lastRotate += Time.deltaTime;
                return;
            }

            rotate();
            lastRotate = 0;
        }
    }

    public void moveDownMentally()
    {
        if (enabled)
        {
            if (lastVerticalMove < lastVerticalMoveDelay)
            {
                lastVerticalMove += Time.deltaTime;
                return;
            }

            transform.position += Vector3.down;
            if (!checkIsInValidPosition())
            {
                transform.position += Vector3.up;
                FindObjectOfType<Game>().deleteFullRows();
                if (FindObjectOfType<Game>().checkIsAboveGrid(this))
                {
                    FindObjectOfType<Game>().gameOver();
                }
                playLandSound();
                Game.currentScore += tetrominoFastLandScore;
                enabled = false; //here i checked the disabled script on the gameObject
                FindObjectOfType<Game>().spwanNextTetromino();
                
                //- added to solve the panel problem
                TutorialMenuController.engine.MentalCommandEmoStateUpdated -= new EmoEngine.MentalCommandEmoStateUpdatedEventHandler(MovingObjectByMind);
            }
            else
            {
                FindObjectOfType<Game>().updateGrid(this);
                playMoveSound();
            }
            lastVerticalMove = 0;
            fall = Time.time;
        }
    }

    //- MEG control throgh the engine
    //- the next methods call methods from the Game script
    public void moveLeftMentally()
    {
        if (enabled)
        {
            if (lastHorizontalMove < lastHorizontalMoveDelay)
            {
                lastHorizontalMove += Time.deltaTime;
                return;
            }
            transform.position += Vector3.left;
            if (!checkIsInValidPosition())
            {
                transform.position += Vector3.right;
            }
            else
            {
                FindObjectOfType<Game>().updateGrid(this);
                playMoveSound();
            }
            lastHorizontalMove = 0;

        }
    }

    public void moveRightMentally()
    {
        if (enabled)
        {
            if (lastHorizontalMove < lastHorizontalMoveDelay)
            {
                lastHorizontalMove += Time.deltaTime;
                return;
            }
            transform.position += Vector3.right;
            if (!checkIsInValidPosition())
            {
                transform.position += Vector3.left;
            }
            else
            {
                FindObjectOfType<Game>().updateGrid(this);
                playMoveSound();
            }

            lastHorizontalMove = 0;

        }
    }

}
