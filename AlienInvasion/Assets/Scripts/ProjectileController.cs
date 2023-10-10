using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [Header("Projectile Statistics")]
    public float speed;
    public float lifeSpan;
    void Update()
    {
        transform.Translate(Vector3.forward * speed);

        if (lifeSpan > 0)
        {
            lifeSpan -= Time.deltaTime;
        }
        else if (lifeSpan <= 0)
        {
            DestroyObject();
        }
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }

}
