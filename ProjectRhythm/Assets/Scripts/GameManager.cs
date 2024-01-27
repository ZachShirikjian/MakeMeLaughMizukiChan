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

    void Start()
    {
        optionsMenu.SetActive(false);
        laughMeter.gameObject.SetActive(false);
        timerUI.SetActive(false);
        musicSource.clip = null;
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
        yield return new WaitForSeconds(3f);
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

    public void LoadDialogue()
    {
        dialogueBox.SetActive(true);
        curPlace++;
        speakerText.text = dialogueList[1].dialogueText.ToString();
        Invoke("AppearOptions", 3f);
    }

    //OPEN OPTIONS MENU 
    public void AppearOptions()
    {
        curPlace++;

        speakerText.text = dialogueList[curPlace].dialogueText.ToString();
        sfxSource.PlayOneShot(dialogueList[curPlace].soundEffect);
        mizukiSprite.sprite = dialogueList[curPlace].characterSprite;

        optionsMenu.SetActive(true);
        musicSource.Pause();

        //RANDOMLY SELECT A BUTTON ON THE LIST//
        GameObject randomButton = optionsMenu.transform.GetChild(Random.Range(0,1)).gameObject;
        EventSystem.current.SetSelectedGameObject(randomButton);


        //IF no buttons are pressed in 3 seconds, close the options menu, then move onto the next piece of dialogue.
        Invoke("NoOptionsSelected", 3f);
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
        optionsMenu.SetActive(false);
        musicSource.UnPause();

        //MAKE THIS THE LAST PIECE OF DIALOGUE FOR NOW//
        speakerText.text = dialogueList[7].dialogueText.ToString();
        sfxSource.PlayOneShot(dialogueList[7].soundEffect);
        mizukiSprite.sprite = dialogueList[7].characterSprite;
        Invoke("AppearOptions", 1f);
    }


}
