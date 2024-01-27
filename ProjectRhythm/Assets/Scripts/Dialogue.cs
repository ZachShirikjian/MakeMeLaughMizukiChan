using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//ALLOWS DIALOGUE TO BE CREATED IN UNITY UI//
[CreateAssetMenu]
//SCRIPTABLE OBJECT TO ALLOW FOR EASY DIALOGUE CREATION
public class Dialogue : ScriptableObject
{
    //VARIABLES//
    public bool correctDialogue; //checked w/ dialogue manager to see if this was correct part of the song or not 

    //REFERENCES//
    public AudioClip soundEffect; //sound effect associated w/ this dialogue

    [TextArea]
    public string dialogueText; //text of dialogue being spoken

    //portrait of mizuki for this piece of dialogue 
    public Sprite characterSprite;

    //THE CORRECT/INCORRECT OPTION PREFABS TO SPAWN
    public GameObject correctButton;
    public GameObject incorrectButton;
}
