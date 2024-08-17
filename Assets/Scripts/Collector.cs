using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collector : MonoBehaviour
{
    [SerializeField] Text countText;
    private int countStrwBrys = 0;
    [SerializeField] AudioSource collectAudio;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "fruit")
        {
            collectAudio.Play();
            countStrwBrys++;
            countText.text = countStrwBrys.ToString();
            Destroy(other.gameObject);
        }
    }


}