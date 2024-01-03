using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class HandleTimeline : MonoBehaviour
{
    public static PlayableDirector timeline;
    public static bool timelineIsPlaying = false;
    public GameObject phone;
    private GameObject HUD;
    public GameObject objectiveContainer;
    public GameObject objectiveText;
    public GameObject locationContainer;
    public GameObject locationText;
    public GameObject dateTimeContainer;
    public GameObject dateTimeText;
    public Animator playIcon;
    public Animator pauseIcon;
    private bool timelinePaused = true;
    public static bool killedYourself = false;
    public static bool objective6Complete = false;
    public GameObject skipFade;

    private void Start()
    {
        HUD = GameObject.Find("HUD");
        skipFade.SetActive(false);
    }
    private void Update()
    {
        if (HandleProgress.currentChapter == 1 && HandleProgress.currentScene == "Chapter_one_first_dream")
        {
            timeline = GameObject.Find("Timeline Dream").GetComponent<PlayableDirector>();
            timeline.Play();
        }
        else if (HandleProgress.currentChapter == 1 && HandleProgress.currentScene == "Chapter_one_waking_up_from_first_dream")
        {
            timeline = GameObject.Find("Waking up from first Dream TImeline").GetComponent<PlayableDirector>();
            timeline.Play();
            HandleProgress.currentScene = "Chapter_one_getting_up_from_bed";
        }
        else if (HandleProgress.currentChapter == 1 && HandleProgress.currentScene == "Chapter_one_talking_to_mom" && HandleProgress.pickedUpKnife && HandleProgress.currentObjectiveIndex == 4)
        {
            StartCoroutine(setPositionForTimeline());
        }
        else if (HandleProgress.currentChapter == 1 && HandleProgress.currentScene == "Chapter_one_dream_after_killing_yourself" && HandleProgress.currentObjectiveIndex == 4)
        {
            // PlayerPrefs.SetString("currentCharacter", "Takahashi_Summer_school");
            // SceneManagerScript.currentCharacter = PlayerPrefs.GetString("currentCharacter");
            timeline = GameObject.Find("Dream after killing youself Timeline").GetComponent<PlayableDirector>();
            timeline.Play();
            killedYourself = true;
            HandleProgress.currentScene = "Chapter_one_second_dream";
        }
        else if (HandleProgress.currentChapter == 1 && HandleProgress.currentScene == "Chapter_one_going_to_school_after_the_second_dream" && HandleProgress.currentObjectiveIndex == 6)
        {
            timeline = GameObject.Find("Waking up from second Dream TImeline").GetComponent<PlayableDirector>();
            timeline.Play();
            HandleProgress.currentScene = "Chapter_one_second_dream_after_effects";
            objective6Complete = true;
        }
        // timeline = GameObject.Find("Dream after killing youself Timeline").GetComponent<PlayableDirector>();

        if (timeline != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (timelinePaused)
                {
                    PauseTimeline();
                }
                else
                {
                    ResumeTimeline();
                }
            }
            if (Input.GetKeyDown(KeyCode.Tab) && timeline.state == PlayState.Playing)
            {
                StartCoroutine(SkipTimelineFade());
            }
            if (timeline.state == PlayState.Playing)
            {
                Debug.Log("Timeline is playing");
                timelineIsPlaying = true;
                phone.layer = LayerMask.NameToLayer("Default");
                objectiveContainer.SetActive(false);
                objectiveText.SetActive(false);
                locationContainer.SetActive(false);
                locationText.SetActive(false);
                dateTimeContainer.SetActive(false);
                dateTimeText.SetActive(false);
                // HUD.SetActive(false);
            }
            else
            {
                timelineIsPlaying = false;
                Debug.Log("Timeline is not playing");
                phone.layer = LayerMask.NameToLayer("Interactable");
                if (!DialogueManager.dialogueIsPlaying)
                {
                    objectiveContainer.SetActive(true);
                    objectiveText.SetActive(true);
                    if (HandleProgress.tutorialComplete)
                    {
                        locationContainer.SetActive(true);
                        locationText.SetActive(true);
                        dateTimeContainer.SetActive(true);
                        dateTimeText.SetActive(true);
                    }
                }
                // this.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator setPositionForTimeline()
    {
        CharacterController characterController = FindObjectOfType<CharacterController>();
        Transform TakahashisMom = GameObject.Find("Takahashi's Mom").transform;
        TakahashisMom.position = new Vector3(115.903999f, 2.16299963f, -1.47899997f);
        TakahashisMom.rotation = Quaternion.Euler(0, 0, 0);
        characterController.enabled = false;
        characterController.gameObject.transform.position = new Vector3(115.4143f, 2.131f, -3.513328f);
        characterController.gameObject.transform.rotation = Quaternion.Euler(0, -0.776f, 0);
        characterController.enabled = true;
        yield return new WaitForSeconds(0.2f);
        timeline = GameObject.Find("Kill Yourself TIimeline").GetComponent<PlayableDirector>();
        timeline.Play();
        HandleProgress.currentScene = "Chapter_one_kill_yourself";
    }

    void PauseTimeline()
    {
        timeline.playableGraph.GetRootPlayable(0).SetSpeed(0.0f);
        timelinePaused = false;
        StartCoroutine(setPauseFalse());
    }

    void ResumeTimeline()
    {
        timeline.playableGraph.GetRootPlayable(0).SetSpeed(1.0f);
        timelinePaused = true;
        StartCoroutine(setPlayFalse());
    }

    void SkipTimeline()
    {
        timeline.time = timeline.duration;
        skipFade.SetActive(false);
    }

    IEnumerator SkipTimelineFade()
    {
        skipFade.SetActive(true);
        skipFade.GetComponent<Animator>().Play("SkipTimelineFade");
        yield return new WaitForSeconds(2f);
        SkipTimeline();
    }

    IEnumerator setPauseFalse()
    {
        pauseIcon.SetBool("isPause", true);
        yield return new WaitForSeconds(0.4f);
        pauseIcon.SetBool("isPause", false);
    }
    IEnumerator setPlayFalse()
    {
        playIcon.SetBool("isPlay", true);
        yield return new WaitForSeconds(0.4f);
        playIcon.SetBool("isPlay", false);
    }

}
