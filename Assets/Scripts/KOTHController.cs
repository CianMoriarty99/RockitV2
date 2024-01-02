using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KOTHController : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite highlightSprite, defaultSprite;
    private SpriteRenderer currentSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
       currentSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "PLAYER")
        {
            SoundEffectPlayer.Instance.playKoTHClip();
            currentSpriteRenderer.sprite = highlightSprite;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag == "PLAYER")
        {
            currentSpriteRenderer.sprite = defaultSprite;
        }
    }
}
