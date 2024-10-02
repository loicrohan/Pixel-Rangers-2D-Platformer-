using System.Collections;
using UnityEngine;

public class TrapFire : MonoBehaviour
{
    [SerializeField] private float offDuration = 2f; // Duration fire stays off/on
    private Animator anim;
    private CapsuleCollider2D fireCollider;
    private bool isActive;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        fireCollider = GetComponent<CapsuleCollider2D>();

        if (anim == null)
            Debug.LogError("Animator component is missing on " + gameObject.name);
        if (fireCollider == null)
            Debug.LogError("CapsuleCollider2D component is missing on " + gameObject.name);
    }

    private void Start()
    {
        SetFire(true); // Start with fire on
        StartCoroutine(FireLoop());
    }

    private IEnumerator FireLoop()
    {
        while (true) // Infinite loop
        {
            SetFire(false); // Turn off fire
            yield return new WaitForSeconds(offDuration); // Wait for offDuration
            SetFire(true); // Turn on fire
            yield return new WaitForSeconds(offDuration); // Wait for offDuration
        }
    }

    private void SetFire(bool active)
    {
        if (anim != null)
            anim.SetBool("active", active);

        if (fireCollider != null)
            fireCollider.enabled = active;

        isActive = active;
    }
}