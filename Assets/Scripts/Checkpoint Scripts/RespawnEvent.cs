using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnEvent : MonoBehaviour
{
    private StartPoint startPoint;

    private void Awake()
    {
        startPoint = GetComponent<StartPoint>();
    }
}