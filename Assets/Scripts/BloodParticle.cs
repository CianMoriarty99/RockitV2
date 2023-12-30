using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodParticle : MonoBehaviour
{

    void Start()
    {
        float distanceToCentre = Vector3.Distance(new Vector3(0,0,0), transform.position);
        transform.localScale = new Vector3 ( (distanceToCentre/4f), (distanceToCentre/4f), 1);
    }

}
