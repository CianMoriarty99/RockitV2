using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAudioPlayer : MonoBehaviour
{
    public AudioSource src;

    public AudioClip laser;
    private static LaserAudioPlayer _instance;
    public static LaserAudioPlayer Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    public void playLaserClip()
    {
        src.clip = laser;
        src.Play();
    }

    public void stopClip()
    {
        src.Stop();
    }

}
