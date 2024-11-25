using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrampoline : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private float pushPower;
    [SerializeField] private float duration = .5f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            player.Push(transform.up * pushPower, duration);
            anim.SetTrigger("Activate");
        }
    }
}