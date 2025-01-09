using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _enemy; 
    // Start is called before the first frame update
    void Start()
    {
        _enemy.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            _enemy.SetActive(true);
        }
    }
}
