using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    public Transform cameraPosition;
    public Transform player;

    private void Update()
    {
        transform.position = cameraPosition.position;
    }
}
