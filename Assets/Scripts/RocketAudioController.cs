using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketAudioController : MonoBehaviour
{
    public AudioSource src;

    public AudioClip rocket;

    public bool playing = false;
    private static RocketAudioController _instance;
    public static RocketAudioController Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    public void playRocketClip()
    {
        if(playing == false)
        {
            src.clip = rocket;
            src.Play();
            playing = true;
        }

    }

    public void stopClip()
    {
        playing = false;
        src.Stop();
    }

}
