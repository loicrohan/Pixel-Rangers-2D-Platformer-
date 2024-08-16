using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] float speed = 4f;
    void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, 360f * speed * Time.deltaTime));
    }
}