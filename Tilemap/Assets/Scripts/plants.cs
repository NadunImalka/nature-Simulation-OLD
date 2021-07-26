using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plants : MonoBehaviour
{
    public float food = 50;
    //public GameObject environment;


    void Start()
    {
       // environment = GameObject.Find("environmentControl");
    }

    void Update()
    {
        if (food < 1)
        {
            //environment.GetComponent<environmentControl>().plantLocations.Add(new Vector3(transform.position.x 
                //, transform.position.y-0.5f , transform.position.z));
            Destroy(gameObject);
        }
    }
}
