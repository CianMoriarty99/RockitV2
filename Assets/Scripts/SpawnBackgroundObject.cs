using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBackgroundObject : MonoBehaviour
{

    public GameObject backgroundPrefab;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        time = 0f;

        for (int i = 0; i < 20; i++)
        {
            Vector3 pos = Random.onUnitSphere * 5f;
            Vector3 normalisedPos = new Vector3(pos.x, pos.y, 20f);
            Instantiate(backgroundPrefab, normalisedPos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //every so often spawn a prefab somewhere with a radius

        if(time > 0.2f)
        {
            time = 0f;
            Vector3 pos = Random.onUnitSphere * 2f;
            Vector3 normalisedPos = new Vector3(pos.x -4f, pos.y -4f, 20f);
            Instantiate(backgroundPrefab, normalisedPos, Quaternion.identity);
        }

        time += 0.01f;
    }
}
