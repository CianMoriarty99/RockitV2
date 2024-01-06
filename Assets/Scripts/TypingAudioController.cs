using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypingAudioController : MonoBehaviour
{
    public AudioSource src;
    public AudioClip typing1, typing2;

    public bool sound1;

    private static TypingAudioController _instance;
    public static TypingAudioController Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
        sound1 = true;
    }

    public void playTypingClip()
    {
        if(sound1)
        {
            src.clip = typing1;
        } else
        {
            src.clip = typing2;
        }

        sound1 = !sound1;

        src.Play();
    }

    public void stopClip()
    {
        src.Stop();
    }

}
