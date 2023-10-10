using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    public float topBounds = 30.0f;

    public float lowerBounds = -10.0f;

    private void Awake()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z < lowerBounds)
        {
            Destroy(gameObject);
        }
        if (transform.position.z > topBounds)
        {
            Destroy(gameObject);
        }
    }
}
