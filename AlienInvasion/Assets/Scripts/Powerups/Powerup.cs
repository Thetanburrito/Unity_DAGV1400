using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Powerup : MonoBehaviour
{

    // throw in a check that determines if it is just player or not.

    // idk how to check for the player specifically my player game object is just called Player
    public PowerupEffect powerupEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            Destroy(gameObject);
            powerupEffect.Apply(other.gameObject);
        }
    }
}
