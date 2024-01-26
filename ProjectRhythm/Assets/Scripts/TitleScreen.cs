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

    //REFERENCES//

    // Start is called before the first frame update
    void Start()
    {
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
