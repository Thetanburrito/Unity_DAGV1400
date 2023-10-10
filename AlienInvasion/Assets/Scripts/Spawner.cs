using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float defaultCooldown;
    [SerializeField] private float Cooldown;
    public GameObject spawnedItem;


    void Update()
    {
        if (Cooldown > 0)
        {
            Cooldown -= Time.deltaTime;
        }
        else if (Cooldown <= 0)
        {
            Cooldown = defaultCooldown;
            Instantiate(spawnedItem, transform.position, transform.rotation);
        }
    }
}
