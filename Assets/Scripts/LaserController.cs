using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    BoxCollider2D box;
    bool enabledCacheValue = true;

    float startingFadeAlpha = 0.1f, currentAlpha = 0.1f, endingFadeAlpha = 0.5f, activatedAlpha = 1f;

    public float activateTimer = 5f, currentTime = 0f, endOfLifeTimer = 10f;
    // Start is called before the first frame update
    void Awake()
    {
        ChangeAlpha(startingFadeAlpha);
        box = GetComponent<BoxCollider2D>();
        DisableCollider();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Fade in the laser as it is preparing
        if(currentAlpha < endingFadeAlpha)
        {
            IncreaseAlpha(currentAlpha);
        }


        if(currentTime > activateTimer)
        {
            EnableCollider();
            ChangeAlpha(activatedAlpha);
        }

        if(currentTime > endOfLifeTimer)
        {
            DisableCollider();
            //For now - replace with some cooloff effect
            ChangeAlpha(0f);

            //Temporary
            currentTime = 0f;
        }

        if(currentTime < endOfLifeTimer)
        {
            currentTime += Time.deltaTime;
        }

    }

    void IncreaseAlpha(float currentAlpha)
    {
        ChangeAlpha(currentAlpha + 0.01f);
    }

    void ChangeAlpha(float alpha)
    {
        Color tmp = GetComponent<SpriteRenderer>().color;
        tmp.a = alpha;
        GetComponent<SpriteRenderer>().color = tmp;
        currentAlpha = alpha;
    }

    void EnableCollider()
    {
        if(enabledCacheValue == false)
        {
            enabledCacheValue = true;
            box.enabled = true;
        }

    }

    void DisableCollider()
    {
        if(enabledCacheValue == true)
        {
            enabledCacheValue = false;
            box.enabled = false;
        }

    }
}
