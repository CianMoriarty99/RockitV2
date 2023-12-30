using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissipate : MonoBehaviour
{
    public float scaleSpeed = 0.000000001f; // Adjust the speed of scaling

    private void Update()
    {
        // Reduce scale over time
        transform.localScale -= new Vector3(scaleSpeed * Time.deltaTime, scaleSpeed * Time.deltaTime, scaleSpeed * Time.deltaTime);

        transform.localScale = Vector3.Max(Vector3.zero, transform.localScale);

        // Check if the scale is almost zero
        if (transform.localScale.x <= 0.01f)
        {
            // Destroy the object after a delay
            Destroy(gameObject);
        }
    }
}
