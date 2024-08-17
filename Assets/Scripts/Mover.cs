using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] Transform[] points;
    [SerializeField] float speed = 2f;
    int counter = 0;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Vector3.Distance(transform.position, points[counter].position) < 0.1f)
        {
            counter++;
        }

        if(counter >= points.Length)
        {
            counter = 0;
        }

        transform.position = Vector3.MoveTowards(transform.position, points[counter].position, speed * Time.deltaTime);

    }
}