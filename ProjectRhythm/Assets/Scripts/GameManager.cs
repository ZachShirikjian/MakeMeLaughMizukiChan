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
    public bool noAnswers = false;

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

    public GameObject[] optionPrefabs = new GameObject[4];

    private GameObject canvas;

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        optionsMenu.SetActive(false);
        laughMeter.gameObject.SetActive(false);
        timerUI.SetActive(false);
        musicSource.clip = null;
        noAnswers = false;
        countdownText.text = "";
        speakerText.text = dialogueList[0].dialogueText.ToString();
        mizukiSprite.sprite = dialogueList[0].characterSprite;
        dialogueBox.SetActive(true);
        curPlace = 0;
        timeRemaining = 3;
        sfxSource.PlayOneShot(dialogueList[0].soundEffect);
        StartCoroutine("Countdown");
    }

    // Update is called once per frame
    void Update()
    {

    }
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
                sfxSource.PlayOneShot(audioManager.gameStart);
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
        if(noAnswers == true)
        {
            noAnswers = false;
        }
        else if(noAnswers == false)
        {
            Debug.Log("BUTTON WAS PREVIOUSLY CHOSEN");
            curPlace++;
        }

        //16 is last line of dialogue 
        if (curPlace >= 16)
        {
            Debug.Log("GAME FINISH!");
            musicSource.Stop(); //stops the music 
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
          //  Invoke("AppearOptions", 3f);
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
        optionsMenu.SetActive(true);
        GameObject option1 = Instantiate(dialogueList[curPlace].correctButton, optionsMenu.transform);
        GameObject option2 = Instantiate(dialogueList[curPlace].incorrectButton, optionsMenu.transform);

        EventSystem.current.SetSelectedGameObject(option1);

      //  option1.transform.SetParent(optionsMenu.transform);
       // option2.transform.SetParent(optionsMenu.transform);

        //IF no buttons are pressed in 5 seconds, close the options menu, then move onto the next piece of dialogue.
        Invoke("NoOptionsSelected", 5f);
      //  //if(!performed)
      //  {
       //     CloseOptions();
      //  }
    }

    //CLOSE OPTIONS MENU AUTOMATICALLY AFTER 3 SECONDS IF NO BUTTON IS PRESSED
    //PLAY THE WRONG NOISE SFX 
    //RESETS APPEAR OPTIONS AND CONTINUE TO THE NEXT OPTION
    public void NoOptionsSelected()
    {
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

    //CALLED ON THE SUBMIT BUTTON
    public void ContinueDialogue()
    {
        if(dialogueList[curPlace].correctDialogue == true)
        {
            Debug.Log("CORRECT ANSWER");
        }
        else if(dialogueList[curPlace].correctDialogue == false)
        {
            Debug.Log("WRONG ANSWER");
        }
    }


}
