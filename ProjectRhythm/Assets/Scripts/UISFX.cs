using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISFX : MonoBehaviour
{

    public AudioSource sfxSource;
    public AudioManager audioScript;
    public void HoverSound()
    {
        sfxSource.PlayOneShot(audioScript.hoverBut);
    }

    public void SubmitSound()
    {
        sfxSource.PlayOneShot(audioScript.clickBut);
    }

}
