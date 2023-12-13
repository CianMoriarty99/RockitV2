using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackgroundObject : MonoBehaviour
{
    public float timeAlive, killAfter;
    private float rotation, speed;
    // Start is called before the first frame update
    void Start()
    {
        rotation = Random.Range(10f, 30.0f);
        speed = Random.Range(0.3f, 0.7f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate ( Vector3.forward * ( rotation * Time.deltaTime ) );

        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y + speed * Time.deltaTime, transform.position.z);

        if (timeAlive > killAfter)
        {
            Destroy(this.gameObject);
        }

        timeAlive += Time.deltaTime;
    }
}
