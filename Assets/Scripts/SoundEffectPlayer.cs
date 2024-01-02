using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{   
    public AudioSource src;
    public AudioClip koth, snake, uiSelect, switchLevel, playerDeathSound;

    private static SoundEffectPlayer _instance;
    public static SoundEffectPlayer Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    public void playKoTHClip()
    {
        src.clip = koth;
        src.Play();
    }

    public void playDeathSound()
    {
        src.clip = playerDeathSound;
        src.Play();
    }

    public void playSnakeClip()
    {
        src.clip = snake;
        src.Play();
    }

    public void stopClip()
    {
        src.Stop();
    }

    public void playUISelectClip()
    {
        src.clip = uiSelect;
        src.Play();
    }

    public void playSwitchLevelClip()
    {
        src.clip = switchLevel;
        src.Play();
    }


}
