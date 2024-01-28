using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonCheck : MonoBehaviour
{

    //VARS//
    public bool correctButPressed;
    public bool incorrectButPressed; 

    //REFERENCES//
    //Reference to correctButton prefab spawned in for a dialogue line 
    public GameObject correctPrefab;
    public GameObject incorrectPrefab;

    private GameManager gm; //Reference to the GameManager

    //Reference to incorrectButton prefab spawned in for a dialogue line

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        correctButPressed = false;
        incorrectButPressed = false;

        //correctPrefab.GetComponent<Button>().OnSubmit.AddListener();
    }

    // Update is called once per frame
    void Update()
    {
        correctPrefab = gm.dialogueList[gm.curPlace].correctButton;
        incorrectPrefab = gm.dialogueList[gm.curPlace].incorrectButton;
    }

    //IF SUBMIT performed on correctButton 
    //Call GM CORRECT() method to play correct dialogue and skip no options played method 
    public void NoParameterOnSubmit()
    {
        correctButPressed = false;
    }
    //IF SUBMIT performed on incorrectButton
    //Call GM INCORRECT() method to play incorrect dialogue and skip no options played method
    public void ParameterOnSubmit()
    {
        
    }


}
