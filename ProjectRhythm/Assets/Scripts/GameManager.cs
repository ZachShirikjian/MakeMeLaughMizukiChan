using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour
{
    //VARS//
    public int timeRemaining = 3; //for every line of dialogue
    public List<Dialogue> dialogueList;
    public int curPlace = 0; //current place in dialogue, starts at 
    public int laughAmount = 0; //goes up every time correct answer chosen
    public bool noAnswers = true;

    //REFS//
    public GameObject optionsMenu;
    public GameObject dialogueBox;
    public TextMeshProUGUI speakerText;
    public GameObject timerUI;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI timerText;
    public AudioManager audioManager;
    public AudioSource sfxSource;
    public AudioSource musicSource;
    public Image mizukiSprite;
    public Slider laughMeter;

    private GameObject canvas;
    //public ButtonCheck buttonCheckScript;

    public InputActionReference chooseOption; //SUBMIT button for choosing option
    public InputActionAsset controls; //control layout

    public GameObject correctPrefab;
    public GameObject incorrectPrefab;

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        optionsMenu.SetActive(false);
        laughMeter.gameObject.SetActive(false);
        timerUI.SetActive(false);
        musicSource.clip = null;
        noAnswers = true;
        countdownText.text = "";
        speakerText.text = dialogueList[0].dialogueText.ToString();
        mizukiSprite.sprite = dialogueList[0].characterSprite;
        dialogueBox.SetActive(true);
        curPlace = 0;
        timeRemaining = 3;
        sfxSource.PlayOneShot(dialogueList[0].soundEffect);
        StartCoroutine("Countdown");

        //DISABLE ENTER INPUT BEFORE GAME STARTS
        OnDisable();
    }

    // Update is called once per frame
    void Update()
    {

    }


    //WHEN OPTIONS APPEAR, PRESS SUBMIT 
    private void OnEnable()
    {
        chooseOption.action.performed += ContinueDialogue;
        chooseOption.action.Enable();
    }

    //WHEN OPTIONS DISAPPEAR, PREVENT SUBMIT BUTTON FROM BEING PRESSED
    private void OnDisable()
    {
        // -= ContinueDialogue();
        chooseOption.action.performed -= ContinueDialogue;
        chooseOption.action.Disable();
    }
    //submit.performed += ContinueDialogue()

    //After intro dialogue plays for a few seconds, call this method to start game 
    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(10f);
        dialogueBox.SetActive(false);
        for (int i = 3; i >= 0; i--)
        {
            yield return new WaitForSeconds(1f);
            if (i > 0)
            {
                sfxSource.PlayOneShot(audioManager.countdownBeep);
                countdownText.text = i.ToString();
            }
            else if (i <= 0)
            {
                Debug.Log("START");
                countdownText.text = "START!";
               // sfxSource.PlayOneShot(audioManager.gameStart);
            }
        }
        yield return new WaitForSeconds(1f);
        countdownText.text = "";
        StartGame();
    }

    //STARTS THE GAME after Countdown ends 
    public void StartGame()
    {
        musicSource.clip = audioManager.BGM;
        musicSource.Play();
        timerUI.SetActive(true);
        laughMeter.gameObject.SetActive(true);
        laughMeter.value = 0;
        LoadDialogue();
    }

    //Loads next dialogue choice. After 3 seconds, appear the options. 
    public void LoadDialogue()
    {
        Debug.Log("LOAD NEW DIALOGUE");
        dialogueBox.SetActive(true); 
        if(noAnswers == false)
        {
            noAnswers = true;
        }
        else if(noAnswers == true)
        {
            curPlace++;
        }

        //16 is last line of dialogue 
        if (curPlace >= 16)
        {
            Debug.Log("GAME FINISH!");
            optionsMenu.SetActive(false);
            //musicSource.Stop(); //stops the music 
            sfxSource.PlayOneShot(audioManager.gameFinish);
            Invoke("FinishGame", 3f);
        }
        else if(curPlace < 16)
        {
            speakerText.text = dialogueList[curPlace].dialogueText.ToString();

            //IF A DIALOGUE OPTION HAS A SOUND ON IT PLAY SOUND
            if (dialogueList[curPlace].soundEffect != null)
            {
                sfxSource.PlayOneShot(dialogueList[curPlace].soundEffect);
            }
            mizukiSprite.sprite = dialogueList[curPlace].characterSprite;
            StopAllCoroutines(); //Ensures that only 1 instance of AnswerTimer coroutine runs at a time
            StartCoroutine("AnswerTimer");
        }

    }

    IEnumerator AnswerTimer()
    {
        timerUI.SetActive(true);
        timeRemaining = 3;
        timerText.text = timeRemaining.ToString();
        for (int i = 3; i >= 0; i--)
        {
            yield return new WaitForSeconds(1f);
            timeRemaining--;
            if(timeRemaining > 0)
            {
                timerText.text = timeRemaining.ToString();
            }
            else if(timeRemaining <= 0)
            {
                timerUI.SetActive(false);
                AppearOptions(); //Make options menu appear after 3 seconds
            }
        }
    }


    //Checks # of correct answers given based on laugh value
    public void FinishGame()
    {
        if(laughAmount >= 4)
        {
            Debug.Log("GAME WIN!");
            speakerText.text = dialogueList[18].dialogueText.ToString();
            sfxSource.PlayOneShot(dialogueList[18].soundEffect);
            mizukiSprite.sprite = dialogueList[18].characterSprite;
        }

        //PLAY LOSING SFX
        else if (laughAmount < 4)
        {
            Debug.Log("GAME LOSE");
            speakerText.text = dialogueList[17].dialogueText.ToString();
            sfxSource.PlayOneShot(dialogueList[17].soundEffect);
            mizukiSprite.sprite = dialogueList[17].characterSprite; 
        }
    }

    //OPEN OPTIONS MENU 
    public void AppearOptions()
    {
        Debug.Log("OPEN MENU");
        optionsMenu.SetActive(true);
        correctPrefab = Instantiate(dialogueList[curPlace].correctButton, optionsMenu.transform);
        incorrectPrefab = Instantiate(dialogueList[curPlace].incorrectButton, optionsMenu.transform);
        incorrectPrefab.transform.position = new Vector3(incorrectPrefab.transform.position.x + 100, incorrectPrefab.transform.position.y);

        EventSystem.current.SetSelectedGameObject(correctPrefab);
        //correctPrefab = option1;
       // incorrectPrefab = option2;
        OnEnable();

        //If Enter Button does not get pressed in 5 seconds
       // Invoke("NoOptionsSelected", 5f);

    }

    //CLOSE OPTIONS MENU AUTOMATICALLY AFTER 3 SECONDS IF NO BUTTON IS PRESSED
    //PLAY THE WRONG NOISE SFX 
    //RESETS APPEAR OPTIONS AND CONTINUE TO THE NEXT OPTION
    public void NoOptionsSelected()
    {
        //IF NO BUTTONS WERE PRESSED
        //LOAD NO ANSWERS GIVEN DIALOGUE 
        if(noAnswers == true && !chooseOption.action.triggered)
        {
            OnDisable();
            Debug.Log("NO OPTIONS SELECTED");
            optionsMenu.SetActive(false);

            //MAKE THIS THE LAST PIECE OF DIALOGUE FOR NOW//
            speakerText.text = dialogueList[16].dialogueText.ToString();
            mizukiSprite.sprite = dialogueList[16].characterSprite;
            sfxSource.PlayOneShot(audioManager.noAnswer, 0.35f);
            curPlace += 3;
            noAnswers = true;
            Invoke("LoadDialogue", 3f);
        }


        //IF CORRECT BUTTON WAS PRESSED
       // else if(buttonCheckScript.correctButPressed == true)
       // {
       //     ContinueDialogue(
       // }

      //  else if(buttonCheckScript.correctButPressed == false)
      //  {
       //     ContinueDialogue(false);
      //  }

    }

    //CALLED ON THE SUBMIT BUTTON
    public void ContinueDialogue(InputAction.CallbackContext context)
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject);

        //IF correctPrefab selected and pressed, jump to correct dialogue option
        if(EventSystem.current.currentSelectedGameObject == correctPrefab)
        {
            Debug.Log("CORRECT ANSWER");
            curPlace += 2;
            speakerText.text = dialogueList[curPlace].dialogueText.ToString();
            mizukiSprite.sprite = dialogueList[curPlace].characterSprite;
            sfxSource.PlayOneShot(dialogueList[curPlace].soundEffect);
            laughAmount++;
            laughMeter.value += 1;

            //DESTROY COPIES OF CORRECT/INCORRECT PREFABS IN THE UI! 
            Destroy(correctPrefab.gameObject);
            Destroy(incorrectPrefab.gameObject);

        }

        //IF incorrectPrefab selected and pressed, jump to incorrect dialogue option 
        else if(EventSystem.current.currentSelectedGameObject == incorrectPrefab)
        {
            Debug.Log("WRONG ANSWER");
            //MAKE THIS THE LAST PIECE OF DIALOGUE FOR NOW//
            curPlace += 1;
            speakerText.text = dialogueList[curPlace].dialogueText.ToString();
            mizukiSprite.sprite = dialogueList[curPlace].characterSprite;
            sfxSource.PlayOneShot(dialogueList[curPlace].soundEffect);

            //DESTROY COPIES OF CORRECT/INCORRECT PREFABS IN THE UI! 
            Destroy(correctPrefab.gameObject);
            Destroy(incorrectPrefab.gameObject);
        }

        noAnswers = false;
        OnDisable();
        optionsMenu.SetActive(false);
        Invoke("LoadDialogue", 3f);

        //  if(dialogueList[curPlace].correctDialogue == true)
        //  {
        //      Debug.Log("CORRECT ANSWER");
        // }
        // else if(dialogueList[curPlace].correctDialogue == false)
        //  {
        //      Debug.Log("WRONG ANSWER");
        //  }
    }


}
