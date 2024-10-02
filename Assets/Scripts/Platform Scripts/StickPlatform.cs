using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickPlatform : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();

        if (player != null/*other.transform.tag == "Player"*/)
        {
            other.transform.SetParent(this.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();

        if (player != null/*other.transform.tag == "Player"*/)
        {
            other.transform.SetParent(null);
        }
    }
}