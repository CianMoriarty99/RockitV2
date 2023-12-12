using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateDoorScript : MonoBehaviour
{
    public Sprite[] animationSprites; //
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

    public void AnimateOpen()
    {
        StartCoroutine(SwitchSprite());
    }

    private IEnumerator SwitchSprite(){
        int index = 0;

        while(index < animationSprites.Length)
        {
            yield return new WaitForSeconds(0.2f);
            currentSpriteRenderer.sprite = animationSprites[index];
            index++;
        }

    }
}
