using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/FireRate")]
public class FirerateBuff : PowerupEffect
{
    public float amount;
    public float coolDown;
    public override void Apply(GameObject target)
    {
        
        if (target.GetComponent<PlayerController>().frPowerUp == 0)
        {
            target.GetComponent<PlayerController>().fireRate -= amount;
            target.GetComponent<PlayerController>().frPowerUp = coolDown;
        }
        else
        {
            target.GetComponent<PlayerController>().fireRate = target.GetComponent<PlayerController>().fireRateDefault - amount;
            target.GetComponent<PlayerController>().frPowerUp = coolDown;
        }
    }
}
