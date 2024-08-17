using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    Animator animator;
    [SerializeField] AudioSource deathAudio;

    private void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.transform.tag == "trap")
        {
            animator.SetTrigger("Death");
            deathAudio.Play();
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}