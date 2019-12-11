using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Emotiv;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class TutorialMenuController : MonoBehaviour
{
    //- profiling
    [Header("Profiling")]
    public GameObject profiling;
    public Text successOrNot;
    public InputField userName;
    public InputField password;
    public InputField profileName;
    bool isRegestered = false;
    
    //- MovingCurserTutorial
    [Header("MovingCurserTutorial")]
    public GameObject MovingCurserTutorial;

    //- NeutralTutorial
    [Header("NeutralTutorial")]
    public GameObject NeutralTutorial;
    public GameObject NeutralTraining;
    public Text ResetNeutralTrainingText;
    public static bool isNeutralTrained = false;

    //- DownTutorial
    [Header("DownTutorial")]
    public GameObject DownTutorial;
    public GameObject DownTraining;
    public Text ResetDownTrainingText;
    public static bool isDownTrained = false;

    //- RotateTutorial
    [Header("RotateTutorial")]
    public GameObject RotateTutorial;
    public GameObject RotateTraining;
    public Text ResetRotateTrainingText;
    public static bool isRotateTrained = false;

    //- FinshedTraining
    [Header("FinshedTraining")]
    public GameObject FinshedTraining;
    public Button finshedTrainingNext;

    //- accept or reject training
    public GameObject messageBoxModal;

    //- emoengine controller
    public static EmoEngine engine;
    public static int engineUserID = -1;
    public static int userCloudID = 0;
    public static int version = -1;

    //- setup the engine and activate
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (engine != null) { 
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        engine = EmoEngine.Instance;
        engine.UserAdded += new EmoEngine.UserAddedEventHandler(UserAddedEvent);
        engine.UserRemoved += new EmoEngine.UserRemovedEventHandler(UserRemovedEvent);
        engine.EmoEngineConnected += new EmoEngine.EmoEngineConnectedEventHandler(EmotivConnected);
        engine.EmoEngineDisconnected += new EmoEngine.EmoEngineDisconnectedEventHandler(EmotivDisconnected);
        engine.MentalCommandTrainingStarted += new EmoEngine.MentalCommandTrainingStartedEventEventHandler(TrainingStarted);
        engine.MentalCommandTrainingSucceeded += new EmoEngine.MentalCommandTrainingSucceededEventHandler(TrainingSucceeded);
        engine.MentalCommandTrainingCompleted += new EmoEngine.MentalCommandTrainingCompletedEventHandler(TrainingCompleted);
        engine.MentalCommandTrainingRejected += new EmoEngine.MentalCommandTrainingRejectedEventHandler(TrainingRejected);
        engine.MentalCommandTrainingDataErased += new EmoEngine.MentalCommandTrainingDataErasedEventHandler(TrainingErased);
        engine.MentalCommandTrainingReset += new EmoEngine.MentalCommandTrainingResetEventHandler(TrainingReset);
        engine.Connect();

        successOrNot.text = "";
        userName.text = "1536699";
        password.text = "1536698*Hy";
        profileName.text = "";
        ResetDownTrainingText.text = "";
        ResetRotateTrainingText.text = "";
        ResetNeutralTrainingText.text = "";

    }

    //- contenuas excute for the engine event if happen
    void Update()
    {
        //- this line make the engine Behaves exact like the update method Behavior but with more contenuas sensing as a device behavior
        engine.ProcessEvents();
    }

    //- the engine events and reactoins 
    void UserAddedEvent(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("User Added");
        engineUserID = (int)e.userId;
    }

    void UserRemovedEvent(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("User Removed");
    }

    void EmotivConnected(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("Connected!!");
    }

    void EmotivDisconnected(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("Disconnected :(");
    }

    public void TrainingStarted(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("Trainig started");
    }

    public void TrainingCompleted(object sender, EmoEngineEventArgs e)
    {
        
        Debug.Log("Training completed!!");

    }

    public void TrainingRejected(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("Trainig rejected");

    }

    public void TrainingErased(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("Commands Erased");
    }

    public void TrainingReset(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("Command Reset");
    }

    public void TrainingSucceeded(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("Training Succeeded!!");
        messageBoxModal.GetComponent<MessageBox>().init("Training Succeeded!!", "Do you want to use this session?", new Decision(AceptTrainig));

    }

    public void AceptTrainig(bool accept)
    {
        if (accept)
        {
            engine.MentalCommandSetTrainingControl((uint)engineUserID, EdkDll.IEE_MentalCommandTrainingControl_t.MC_ACCEPT);
            endTraining();
        }
        else
        {
            engine.MentalCommandSetTrainingControl((uint)engineUserID, EdkDll.IEE_MentalCommandTrainingControl_t.MC_REJECT);
            endTraining();
        }
    }

    //- Load - Save profile buttons // connect to the cloud // check for profile existence
    public bool CloudConnected()
    {
        if (EmotivCloudClient.EC_Connect() == Emotiv.EdkDll.EDK_OK)
        {
            Debug.Log("Connection to server OK");
            if (EmotivCloudClient.EC_Login(userName.text, password.text) == Emotiv.EdkDll.EDK_OK)
            {
                successOrNot.text = "Login as " + userName.text;
                if (EmotivCloudClient.EC_GetUserDetail(ref userCloudID) == Emotiv.EdkDll.EDK_OK)
                {
                    Debug.Log("CloudID: " + userCloudID.ToString());
                    return true;
                }
            }
            else
            {
                successOrNot.text = "Cant login as " + userName.text + ", check password is correct";
            }
        }
        else
        {
            successOrNot.text = "Cant connect to server";
        }
        return false;
    }

    public void SaveProfile()
    {
        if (CloudConnected())
        {
            int profileId = -1;
            EmotivCloudClient.EC_GetProfileId(userCloudID, profileName.text, ref profileId);
            if (profileId >= 0)
            {
                if (EmotivCloudClient.EC_UpdateUserProfile(userCloudID, (int)engineUserID, profileId) == Emotiv.EdkDll.EDK_OK)
                {
                    successOrNot.text = "Profile updated";
                    isRegestered = true;
                }
                else
                {
                    successOrNot.text = "Error saving profile, aborting";
                }
            }
            else
            {
                if (EmotivCloudClient.EC_SaveUserProfile(userCloudID, engineUserID, profileName.text,EmotivCloudClient.profileFileType.TRAINING) == Emotiv.EdkDll.EDK_OK)
                {
                    successOrNot.text = "Profiled saved successfully";
                    isRegestered = true;
                }
                else
                {
                    successOrNot.text = "Error saving profile, aborting";
                }
            }
        }

    }

    public void LoadProfile()
    {
        if (CloudConnected())
        {
            int profileId = -1;
            EmotivCloudClient.EC_GetProfileId(userCloudID, profileName.text, ref profileId);
            if (EmotivCloudClient.EC_LoadUserProfile(userCloudID, (int)engineUserID, profileId, (int)version) == Emotiv.EdkDll.EDK_OK)
            {
                successOrNot.text = "Load finished successfully";
                isRegestered = true;
            }
            else
            {
                successOrNot.text = "Problem loading";
            }
        }
    }

    //- Start Training button in all tutorial dialogs
    public void TrainNeutral()
    {
        NeutralTraining.SetActive(true);
        engine.MentalCommandSetActiveActions((uint)engineUserID, (uint)EdkDll.IEE_MentalCommandAction_t.MC_NEUTRAL);
        engine.MentalCommandSetTrainingAction((uint)engineUserID, EdkDll.IEE_MentalCommandAction_t.MC_NEUTRAL);
        engine.MentalCommandSetTrainingControl((uint)engineUserID, EdkDll.IEE_MentalCommandTrainingControl_t.MC_START);
        isNeutralTrained = true;
    }

    public void TrainDown()
    {
        DownTraining.SetActive(true);
        engine.MentalCommandSetActiveActions((uint)engineUserID, (uint)EdkDll.IEE_MentalCommandAction_t.MC_PUSH);
        engine.MentalCommandSetTrainingAction((uint)engineUserID, EdkDll.IEE_MentalCommandAction_t.MC_PUSH);
        engine.MentalCommandSetTrainingControl((uint)engineUserID, EdkDll.IEE_MentalCommandTrainingControl_t.MC_START);
        isDownTrained = true;

    }

    public void TrainRotate()
    {
        RotateTraining.SetActive(true);
        engine.MentalCommandSetActiveActions((uint)engineUserID, (uint)EdkDll.IEE_MentalCommandAction_t.MC_PULL);
        engine.MentalCommandSetTrainingAction((uint)engineUserID, EdkDll.IEE_MentalCommandAction_t.MC_PULL);
        engine.MentalCommandSetTrainingControl((uint)engineUserID, EdkDll.IEE_MentalCommandTrainingControl_t.MC_START);
        isRotateTrained = true;

    }

    //- Reset Training button in all tutorial dialogs
    public void TrainNeutralReset()
    {
        engine.MentalCommandSetActiveActions((uint)engineUserID, (uint)EdkDll.IEE_MentalCommandAction_t.MC_NEUTRAL);
        engine.MentalCommandSetTrainingAction((uint)engineUserID, EdkDll.IEE_MentalCommandAction_t.MC_NEUTRAL);
        engine.MentalCommandSetTrainingControl((uint)engineUserID, EdkDll.IEE_MentalCommandTrainingControl_t.MC_RESET);
        isNeutralTrained = false;
    }

    public void TrainDownReset()
    {
        engine.MentalCommandSetActiveActions((uint)engineUserID, (uint)EdkDll.IEE_MentalCommandAction_t.MC_PUSH);
        engine.MentalCommandSetTrainingAction((uint)engineUserID, EdkDll.IEE_MentalCommandAction_t.MC_PUSH);
        engine.MentalCommandSetTrainingControl((uint)engineUserID, EdkDll.IEE_MentalCommandTrainingControl_t.MC_RESET);
        isDownTrained = false;
    }

    public void TrainRotateReset()
    {
        engine.MentalCommandSetActiveActions((uint)engineUserID, (uint)EdkDll.IEE_MentalCommandAction_t.MC_PULL);
        engine.MentalCommandSetTrainingAction((uint)engineUserID, EdkDll.IEE_MentalCommandAction_t.MC_PULL);
        engine.MentalCommandSetTrainingControl((uint)engineUserID, EdkDll.IEE_MentalCommandTrainingControl_t.MC_RESET);
        isRotateTrained = false;
    }

    //- Next button in all tutorial dialogs
    public void endProfiling()
    {
        if (isRegestered)
        {
            profiling.SetActive(false);
            MovingCurserTutorial.SetActive(true);
            engine.MentalCommandSetTrainingControl((uint)engineUserID, EdkDll.IEE_MentalCommandTrainingControl_t.MC_ERASE);
        }
        else
        {
            successOrNot.text = "Pls enter your profile to continue";
        }
    }

    public void endCurserTut()
    {
        MovingCurserTutorial.SetActive(false);
        NeutralTutorial.SetActive(true);
        ResetNeutralTrainingText.text = "Pls Train Neutral each time you are here";
    }

    public void endNeutralTut()
    {
        NeutralTutorial.SetActive(false);
        DownTutorial.SetActive(true);
        ResetDownTrainingText.text = "Pls Train PUSH each time you are here";
    }
    
    public void endDownlTut()
    {
        DownTutorial.SetActive(false);
        RotateTutorial.SetActive(true);
        ResetRotateTrainingText.text = "Pls Train Rotate each time you are here";
    }

    public void endRotateTut()
    {
        RotateTutorial.SetActive(false);
        FinshedTraining.SetActive(true);
    }
    
    public void endTut()
    {
        SceneManager.LoadScene("NewGameMenu");
    }

    //- deactivate any finished dialogs
    void endTraining()
    {
            DownTraining.SetActive(false);
            RotateTraining.SetActive(false);
            NeutralTraining.SetActive(false);
    }

    //- when this object is destroyed delete the subsecribation
    private void OnDestroy()
    {
        engine.UserAdded -= new EmoEngine.UserAddedEventHandler(UserAddedEvent);
        engine.UserRemoved -= new EmoEngine.UserRemovedEventHandler(UserRemovedEvent);
        engine.EmoEngineConnected -= new EmoEngine.EmoEngineConnectedEventHandler(EmotivConnected);
        engine.EmoEngineDisconnected -= new EmoEngine.EmoEngineDisconnectedEventHandler(EmotivDisconnected);
        engine.MentalCommandTrainingStarted -= new EmoEngine.MentalCommandTrainingStartedEventEventHandler(TrainingStarted);
        engine.MentalCommandTrainingSucceeded -= new EmoEngine.MentalCommandTrainingSucceededEventHandler(TrainingSucceeded);
        engine.MentalCommandTrainingCompleted -= new EmoEngine.MentalCommandTrainingCompletedEventHandler(TrainingCompleted);
        engine.MentalCommandTrainingRejected -= new EmoEngine.MentalCommandTrainingRejectedEventHandler(TrainingRejected);
        engine.MentalCommandTrainingDataErased -= new EmoEngine.MentalCommandTrainingDataErasedEventHandler(TrainingErased);
        engine.MentalCommandTrainingReset -= new EmoEngine.MentalCommandTrainingResetEventHandler(TrainingReset);
        engine.Disconnect();
    }

    //- quit and disconnect the device
    private void OnApplicationQuit()
    {
        if (engine != null)
        {
            engine.Disconnect();
            engine = null;
        }
    }

    public void quit()
    {
        Application.Quit();
    }
}
