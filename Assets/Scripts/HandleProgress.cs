using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class HandleProgress : MonoBehaviour
{
    private PhoneManager phoneManager;

    public class Objective
    {
        public string description;
        public bool isCompleted;
    }

    public Objective[] objectives = new Objective[]
    {
        new Objective { description = "Press W, A, S, D to move around", isCompleted = false},
        new Objective { description = "Pick up your phone from the table", isCompleted = false},
        new Objective { description = "Press TAB to take out phone", isCompleted = false },
        new Objective { description = "", isCompleted = false },
        new Objective { description = "Find a Knife", isCompleted = false },
        new Objective { description = "Find out what place this is", isCompleted = false },
        new Objective { description = "Find the source of that voice", isCompleted = false },
        new Objective { description = "Pick up your phone and change for school", isCompleted = false },
        new Objective { description = "Go to School", isCompleted = false },
    };

    public static bool firstPlaythrough;
    public static int currentChapter = 1;
    public static string currentChapterName;
    public static string currentScene;
    public static bool tutorialComplete = false;
    public static int currentObjectiveIndex;
    private bool pressW = false;
    private bool pressA = false;
    private bool pressS = false;
    private bool pressD = false;
    public static bool pickedUpPhone = false;
    public static bool pickedUpKnife = false;
    public static bool readyForSchool = false;

    private float duration = 0.8f;

    [Header("Objective")]
    public static TextMeshProUGUI objective;
    public Animator objectiveContainerAnimator;
    public Animator objectiveTextAnimator;
    [Header("Location")]
    public static TextMeshProUGUI location;
    public Animator locationContainerAnimator;
    public Animator locationTextAnimator;

    [Header("Date Time")]
    public static TextMeshProUGUI dateTime;
    public Animator dateTimeContainerAnimator;
    public Animator dateTimeTextAnimator;


    [Header("Dialogue Files")]
    private DialogueManager dialogueManager;

    private void Awake()
    {
        objective = GameObject.Find("Objective Text").GetComponent<TextMeshProUGUI>();
        location = GameObject.Find("Location Text").GetComponent<TextMeshProUGUI>();
        dateTime = GameObject.Find("Date TIme Text").GetComponent<TextMeshProUGUI>();
        phoneManager = GameObject.Find("PhoneManager").GetComponent<PhoneManager>();
        dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
    }

    private void Start()
    {
        // if (currentObjectiveIndex == 0)
        // {
        //     currentChapter = 1;
        //     currentScene = "Chapter_one_second_dream_after_effects";
        //     currentObjectiveIndex = 7;
        //     StartCoroutine(UpdateObjective());
        //     tutorialComplete = true;
        //     SceneManagerScript.currentCharacter = "Takahashi_Summer_home";
        // }

        objectiveContainerAnimator.Play("SlideInFromRightContainer");
        objectiveTextAnimator.Play("SlideInFromRightText");
        if (tutorialComplete)
        {
            location.text = PlayerPrefs.GetString("currentLocation");
            dateTime.text = PlayerPrefs.GetString("currentDateTime");
            locationContainerAnimator.gameObject.SetActive(true);
            locationContainerAnimator.Play("SlideInFromRightContainer");
            locationTextAnimator.gameObject.SetActive(true);
            locationTextAnimator.Play("SlideInFromRightText");
            dateTimeContainerAnimator.gameObject.SetActive(true);
            dateTimeContainerAnimator.Play("SlideInFromLeftContainer");
            dateTimeTextAnimator.gameObject.SetActive(true);
            dateTimeTextAnimator.Play("SlideInFromLeftText");
        }
        StartCoroutine(UpdateObjective());
    }

    private void Update()
    {
        if (!PauseMenuScript.gameIsPaused || currentObjectiveIndex != 0)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                pressW = true;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                pressA = true;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                pressS = true;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                pressD = true;
            }
        }
        switch (currentObjectiveIndex)
        {
            case 0: // Objective: Press W, A, S, D to move around
                firstPlaythrough = true;
                if (pressW && pressA && pressS && pressD)
                {
                    objectives[currentObjectiveIndex].isCompleted = true;
                }
                break;
            case 1: // Objective: Pick up your phone from the table
                if (pickedUpPhone)
                {
                    objectives[currentObjectiveIndex].isCompleted = true;
                }
                break;
            case 2: // Objective: Press TAB to take out phone
                if (phoneManager.phoneOutFirstTime)
                {
                    objectives[currentObjectiveIndex].isCompleted = true;
                }
                break;
            case 3: //
                bool objective3Complete = ((Ink.Runtime.BoolValue)dialogueManager.GetVariableState("objective3Complete")).value;
                string locationText = ((Ink.Runtime.StringValue)dialogueManager.GetVariableState("location")).value;
                string time = ((Ink.Runtime.StringValue)dialogueManager.GetVariableState("time")).value;
                if (objective3Complete)
                {
                    location.text = locationText;
                    dateTime.text = time;
                    PlayerPrefs.SetString("currentLocation", locationText);
                    PlayerPrefs.SetString("currentDateTime", time);
                    locationContainerAnimator.gameObject.SetActive(true);
                    locationTextAnimator.gameObject.SetActive(true);
                    locationContainerAnimator.Play("SlideInFromRightContainer");
                    locationTextAnimator.Play("SlideInFromRightText");
                    dateTimeContainerAnimator.gameObject.SetActive(true);
                    dateTimeTextAnimator.gameObject.SetActive(true);
                    dateTimeContainerAnimator.Play("SlideInFromLeftContainer");
                    dateTimeTextAnimator.Play("SlideInFromLeftText");
                    tutorialComplete = true;
                    objectives[currentObjectiveIndex].isCompleted = true;
                }
                break;
            case 4: // Find a Knife
                Debug.Log("Case 4");
                if (HandleTimeline.killedYourself)
                {
                    objectives[currentObjectiveIndex].isCompleted = true;
                }
                break;
            case 5: // Find out what place this is
                Debug.Log("Case 5");
                location.text = "?????";
                dateTime.text = "??-??-????";
                bool objective5Complete = ((Ink.Runtime.BoolValue)dialogueManager.GetVariableState("objective5Complete")).value;
                locationText = ((Ink.Runtime.StringValue)dialogueManager.GetVariableState("location")).value;
                Debug.Log("Objective 5 Complete" + objective5Complete);
                if (objective5Complete)
                {
                    location.text = locationText;
                    objectives[currentObjectiveIndex].isCompleted = true;
                    Transform sachi = GameObject.Find("Sachi - NOT FINAL").transform;
                    sachi.position = new Vector3(0, 0, -18.06f);
                }
                break;
            case 6:
                Debug.Log("Case 6");
                if (HandleTimeline.objective6Complete)
                {
                    pickedUpKnife = false;
                    pickedUpPhone = false;
                    objectives[currentObjectiveIndex].isCompleted = true;
                }
                break;
            case 7:
                Debug.Log("Case 7");
                Debug.Log(currentObjectiveIndex);
                Debug.Log(SceneManagerScript.currentCharacter);
                if (pickedUpPhone && SceneManagerScript.currentCharacter == "Takahashi_Summer_school")
                {
                    readyForSchool = true;
                    objectives[currentObjectiveIndex].isCompleted = true;
                }
                break;
            case 8:
                Debug.Log("Case 8");
                break;
        }

        if (objectives[currentObjectiveIndex].isCompleted)
        {
            currentObjectiveIndex++;
            StartCoroutine(UpdateObjective());
        }

        Debug.Log("currentScene " + HandleProgress.currentScene);
        Debug.Log("currentObjectiveIndex " + HandleProgress.currentObjectiveIndex);
    }

    private IEnumerator UpdateObjective()
    {
        objectiveTextAnimator.Play("SlideOutFromRightText");
        yield return new WaitForSeconds(1.1f);
        objective.text = objectives[currentObjectiveIndex].description;
        objectiveTextAnimator.Play("SlideInFromRightText");
    }
}
