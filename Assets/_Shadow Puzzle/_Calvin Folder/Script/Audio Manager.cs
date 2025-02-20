using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource BGM;
    public AudioSource SFX;

    public AudioClip bgm;
    public AudioClip buttonClick;
    public AudioClip buttonHover;

    private void Start()
    {
        BGM.clip = bgm;
        BGM.clip = bgm;
    }
}