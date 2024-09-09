using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CinemachineVirtualDynamic : MonoBehaviour
{
    [SerializeField] private Transform focusOnPlayer;
    private CinemachineVirtualCamera virtualCamera;
    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        focusOnPlayer = FindFirstObjectByType<Player>().transform;

    }

    // Update is called once per frame
    public void ResetCameraPosition()
    {
        if (focusOnPlayer == null)
        {
            virtualCamera.Follow = focusOnPlayer;

        }

    }
}