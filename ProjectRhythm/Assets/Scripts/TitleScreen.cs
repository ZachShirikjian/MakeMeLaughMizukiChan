using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class TitleScreen : MonoBehaviour
{
    //VARIABLES//
    public GameObject playButton;
    public GameObject controlsButton;
    public GameObject controlsPanel;
    public GameObject closeControls;
    public GameObject creditsPanel;
    public GameObject creditsButton;
    public GameObject closeCredits;

    //REFERENCES//

    // Start is called before the first frame update
    void Start()
    {
        creditsPanel.SetActive(false);
        controlsPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(playButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //LOADS UP GAME SCENE ON PLAY
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    //OPENS UP CREDITS//
    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(closeCredits);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(creditsButton);
    }

    //OPENS UP CONTROLS MENU//
    public void OpenControls()
    {
        controlsPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(closeControls);
    }

    public void CloseControls()
    {
        controlsPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(controlsButton);
    }

    //QUITS OUT OF UNITY (IN-ENGINE) OR EXITS THE GAME (ON DESKTOP)
    public void QuitGame()
    {
        //Quit game if in editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        
        //Close exe if in build
        #endif
        Application.Quit();
    }
}
