using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCameraTargetTrigger : MonoBehaviour
{
    [Header("Interactive Camera")]
    [SerializeField] protected CinemachineVirtualCamera _virtualCamera;
    [SerializeField] protected Transform newtarget;
    [SerializeField] protected GameObject deadZone;

    protected virtual void Start()
    {
        deadZone.SetActive(false);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            _virtualCamera.Follow = newtarget;
            deadZone.SetActive(true);
        }
    }
}