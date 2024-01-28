using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class GameOver : MonoBehaviour
{

    //USED FOR RETRY/QUIT BUTTONS IN GAME
    // Start is called before the first frame update
    public InputActionReference submit; //SUBMIT button for choosing option
    public InputActionAsset controls; //control layout

    public GameObject restartButton;
    public GameObject quitButton;
    private GameManager gm;

    private void Awake()
    {
        OnEnable();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public void OnEnable()
    {
        submit.action.performed += ChooseOption;
        submit.action.Enable();
    }

    public void OnDisable()
    {
        submit.action.performed -= ChooseOption;
        submit.action.Disable();
    }

    public void ChooseOption(InputAction.CallbackContext context)
    {
        if(EventSystem.current.currentSelectedGameObject == restartButton)
        {
            RestartGame();
        }

        else if(EventSystem.current.currentSelectedGameObject == quitButton)
        {
            QuitGame();
        }
    }
    //Restarts game on clicking restart button 
    public void RestartGame()
    {
        //prevents button from being selected again
        EventSystem.current.SetSelectedGameObject(null);
        gm.transition.SetActive(true);
        Invoke("ResetGame", 2f);
        gm.transition.GetComponent<Animator>().Play("Slide");

    }

    public void ResetGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    //Quits out of the game and returns to the title screen 
    public void QuitGame()
    {
        EventSystem.current.SetSelectedGameObject(null);
        gm.transition.SetActive(true);
        Invoke("ReturnToTitle", 2.5f);
        gm.transition.GetComponent<Animator>().Play("Slide");
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene("TitleScreen");
    }

}
