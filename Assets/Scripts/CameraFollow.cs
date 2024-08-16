using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = new Vector3(player.position.x, 0f, transform.position.z);
    }
}