using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodParticle : MonoBehaviour
{

    void Start()
    {
        float distanceToCentre = Vector3.Distance(new Vector3(0,0,0), transform.position);
        float max = Mathf.Max((distanceToCentre/5f), 0.3f);
        transform.localScale = new Vector3 ( max, max, 1);
    }

}
