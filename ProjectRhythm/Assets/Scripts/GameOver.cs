using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
public class GameOver : MonoBehaviour
{

    //USED FOR RETRY/QUIT BUTTONS IN GAME
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Restarts game on clicking restart button 
    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    //Quits out of the game and returns to the title screen 
    public void QuitGame()
    {
        SceneManager.LoadScene("TitleScreen");
    }

}
