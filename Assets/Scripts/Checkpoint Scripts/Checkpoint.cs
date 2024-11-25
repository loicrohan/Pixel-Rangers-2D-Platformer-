using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private bool active;

    [SerializeField] private bool canBeReactivated;

    private void Start()
    {
        canBeReactivated = GameManager.instance.canReactivate;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (active)
            return;

        Player player = collision.GetComponent<Player>();

        if (player != null)
            ActivateCheckpoint();
    }

    private void ActivateCheckpoint()
    {
        active = true;
        anim.SetTrigger("Activate");
        PlayerManager.instance.UpdateRespawnPosition(transform);
    }
}