using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(this.transform.position, target.position, 0.6f);
    }
}
