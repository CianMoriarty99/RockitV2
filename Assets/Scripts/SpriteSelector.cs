using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSelector : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite[] sprites;
    SpriteRenderer m_SpriteRenderer;
    void Start()
    {
        int rand = Random.Range(0, sprites.Length);
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.sprite = sprites[rand];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
