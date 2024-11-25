using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FruitType { Apple,Banana,Cherries,Kiwi,Melon,Orange,Pineapple,Strawberry}

public class Fruit : MonoBehaviour
{
    [SerializeField] private FruitType fruitType;
    [SerializeField] private GameObject pickupVFX;

    private GameManager gameManager;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        SetRandomLookIfNeeded();
    }

    private void SetRandomLookIfNeeded()
    {
        if (gameManager.FruitsHaveRandomLook() == false)
        {
            UpdateFruitVisuals();
            return;
        }


        int randomIndex = Random.Range(0, 8); // max value is exclusive, so it will give number from 0 to 7.
        anim.SetFloat("FruitIndex", randomIndex);
    }

    private void UpdateFruitVisuals() => anim.SetFloat("FruitIndex", (int)fruitType);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            AudioManager.instance.PlaySFX(8);
            gameManager.AddFruit();
            Destroy(gameObject);

            GameObject newFX = Instantiate(pickupVFX, transform.position, Quaternion.identity);
        }
    }
}