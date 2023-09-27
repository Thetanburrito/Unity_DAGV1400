using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    int myint = 5;
    
    
    
    void Start()
    {
        myint = MultiplybyTwo(myint);
        Debug.Log(myint);
    }


    int MultiplybyTwo(int Number)
    {
        int num = 2;
        num = num * Number;
        return num;
    }
}
