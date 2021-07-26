using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class environmentControl : MonoBehaviour
{
    public List<Vector3> plantLocations = new List<Vector3>();
    public GameObject plant;
    public GameObject[] Blocks;

    private GameObject ground2;
    private float growth = 100;

    private void Start()
    {
        ground2 = GameObject.Find("ground2");
        // Blocks = GameObject.FindGameObjectsWithTag("grass");
        Blocks = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (var block in Blocks)
        {
            if (block.name == "grassBlock")
            {
                plantLocations.Add(block.transform.position);
                Debug.LogWarning("NOt adding");
            }
        }
    }

    void Update()
    {
        growth -= 0.1f;
        // if (growth < 2)
        // {
        //     if (plants!=null)
        //     {
        //         Instantiate(plant, plants.First() , transform.rotation);
        //         plants.RemoveAt(0);
        //     }
        //     growth = 1000;
        // }
        if (growth<2 && plantLocations!=null)
        {
            growth = 100f;
            int random = Random.Range(0, plantLocations.Count);
            GameObject plantThis = Instantiate(plant, new Vector3(plantLocations[random].x,plantLocations[random].y+0.5f , plantLocations[random].z) 
                , transform.rotation);
            plantThis.GetComponent<MeshRenderer>().material.color = Color.black;
            plantThis.layer = LayerMask.NameToLayer("food");
            plantThis.name = "plant";
            plantThis.transform.SetParent(ground2.transform,true);
            plantLocations.RemoveAt(random);
        }
        
    }

   
}
