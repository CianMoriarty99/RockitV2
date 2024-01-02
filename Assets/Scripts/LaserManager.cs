using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    public Sprite[] animationFrames;

    private SpriteRenderer sprr;
    private BoxCollider2D bc2d;
    private int currentAnimationFrame;
    public float currentTime;

    private bool chargingUp, chargingDown, laserOn, startCounting;
    private IEnumerator laserOnCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        sprr = GetComponent<SpriteRenderer>();
        bc2d = GetComponent<BoxCollider2D>();
        sprr.material.color = new Color(1f, 1f, 1f, 0f); //Don't show it
        bc2d.enabled = false;
        currentAnimationFrame = 0;
        currentTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.countdownClock && (GameManager.Instance.levelSelected.y == 1 || GameManager.Instance.levelSelected.y == 4))
        {
            currentTime += Time.deltaTime;

            if(currentTime > 6f)
            {
                currentAnimationFrame = 0;
                currentTime = 0f;
                chargingUp = false;
                laserOn = false;
                chargingDown = false;
            }

            if(currentTime > 1f && !chargingUp)
            {
                chargingUp = true;
                bc2d.enabled = false;
                StartCoroutine(ChargeUp());
            }

            if(currentTime > 2f && !laserOn)
            {
                bc2d.enabled = true;
                laserOn = true;
                StartCoroutine(LaserOn());
            }

            if(currentTime > 2.8f && !chargingDown)
            {
                bc2d.enabled = false;
                StartCoroutine(ChargeDown());
            }

            sprr.sprite = animationFrames[currentAnimationFrame];
        } 
        else 
        {
            bc2d.enabled = false;
            currentAnimationFrame = 0;
            currentTime = 0f;
            chargingUp = false;
            laserOn = false;
            chargingDown = false;
            sprr.material.color = new Color(1f, 1f, 1f, 0f);
        }

    }

    IEnumerator ChargeUp()
    {
        sprr.material.color = new Color(1f, 1f, 1f, 1f);
        chargingUp = true;
        while(currentAnimationFrame < 3)
        {
            yield return new WaitForSeconds(0.25f);

            currentAnimationFrame += 1;
        }

        yield break;

    }

    IEnumerator LaserOn()
    {
        LaserAudioPlayer.Instance.playLaserClip();
        currentAnimationFrame = 3;
        yield return new WaitForSeconds(0.2f);
        currentAnimationFrame = 4;
        yield return new WaitForSeconds(0.2f);
        currentAnimationFrame = 3;
        yield return new WaitForSeconds(0.2f);
        currentAnimationFrame = 4;
        yield return new WaitForSeconds(0.2f);
        currentAnimationFrame = 3;
        LaserAudioPlayer.Instance.stopClip();
    }

    IEnumerator ChargeDown()
    {
        chargingDown = true;
        while(currentAnimationFrame > 0)
        {
            yield return new WaitForSeconds(0.1f);
            currentAnimationFrame -= 1;
        }

        sprr.material.color = new Color(1f, 1f, 1f, 0f);
        yield break;

    }
}
