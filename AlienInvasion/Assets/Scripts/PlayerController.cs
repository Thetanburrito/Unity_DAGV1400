using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("KeyBinds")]
    public KeyCode FireKey = KeyCode.Space;


    [Header("PlayerStats")]
    public float movementSpeed;
    public float limitedRange;
    public float fireRate;
    private bool readyToShoot;
    private float horizontalInput;


    [Header("MiscInit")]
    public Transform blaster;
    public GameObject lazerBolt;
    
    
    void Start()
    {
        readyToShoot = true;
    }



    void Update()
    {
        GetInput();
        MovePlayer();
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
    private void MovePlayer()
    {
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * movementSpeed);
        
        if (transform.position.x > limitedRange)
        {
            transform.position = new Vector3(limitedRange, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -limitedRange)
        {
            transform.position = new Vector3(-limitedRange, transform.position.y, transform.position.z);
        }
    
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
    
    if(Input.GetKey(FireKey) && readyToShoot)
        {
            readyToShoot = false;
            Instantiate(lazerBolt, blaster.transform.position, lazerBolt.transform.rotation);
            Invoke(nameof(ResetShoot), fireRate);
        }
    
    }


    private void ResetShoot()
    {
        readyToShoot = true;
    }
}
