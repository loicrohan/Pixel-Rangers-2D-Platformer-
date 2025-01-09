using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Skin
{
    public string skinName;
    public int skinPrice;
    public bool unlocked;
}

public class SkinSelector_UI : MonoBehaviour
{
    private LevelSelection_UI lvlSlctUI;
    private MainMenu_UI mMUI;
    [SerializeField] private Skin[] skinList;

    [Header("UI Details")]
    [SerializeField] private int skinIndex;
    [SerializeField] private int maxIndex;
    [SerializeField] private Animator skinDisplay;
    public Button buyButton;

    [SerializeField] private TextMeshProUGUI buySelectText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI bankText;
    #region Skin Selection Methods
    private void Start()
    {
        LoadSkinUnlocks();
        UpdateSkinDisplay();

        mMUI = GetComponentInParent<MainMenu_UI>();
        lvlSlctUI = mMUI.GetComponentInChildren<LevelSelection_UI>(true);
    }

    private void LoadSkinUnlocks()
    {
        for (int i = 0; i < skinList.Length; i++)
        {
            string skinName = skinList[i].skinName;
            bool skinUnlocked = PlayerPrefs.GetInt(skinName + "Unlocked", 0) == 1;

            if (skinUnlocked || i == 0)
                skinList[i].unlocked = true;
        }
    }

    public void SelectSkin()
    {
        // Only proceed if the skin is not unlocked
        if (!skinList[skinIndex].unlocked)
        {
            // If the Buy button is interactable, attempt to buy the skin
            if (buyButton.interactable)
            {
                bool purchaseSuccessful = BuySkin(skinIndex);
                if (purchaseSuccessful)
                {
                    Debug.Log("Skin purchased successfully!");
                }
            }
        }
        else
        {
            // If the skin is already unlocked, just select it
            SkinManager.instance.SetSkinId(skinIndex);
            mMUI.SwitchUI(lvlSlctUI.gameObject);
        }

        // Refresh the UI
        UpdateSkinDisplay();
    }

    public void NextSkin()
    {
        skinIndex++;

        if (skinIndex > maxIndex)
            skinIndex = 0;

        UpdateSkinDisplay();
        AudioManager.instance.PlaySFX(4);
    }

    public void PreviousSkin()
    {
        skinIndex--;

        if (skinIndex < 0)
            skinIndex = maxIndex;
        UpdateSkinDisplay();
        AudioManager.instance.PlaySFX(4);

    }

    // 2024-07-30 AI-Tag 
    // This was created with assistance from Muse, a Unity Artificial Intelligence product

    private void UpdateSkinDisplay()
    {
        // Display current bank amount
        bankText.text = "Bank : " + FruitsInBank();

        // Update the skin display, showing the selected skin
        for (int i = 0; i < skinDisplay.layerCount; i++)
        {
            skinDisplay.SetLayerWeight(i, i == skinIndex ? 1 : 0);
        }

        // If the current skin is unlocked, show "SELECT" and enable the button
        if (skinList[skinIndex].unlocked)
        {
            priceText.transform.parent.gameObject.SetActive(false); // Hide price
            buySelectText.text = "SELECT";
            buyButton.interactable = true;  // Button enabled for selection
        }
        else
        {
            // If the skin is locked, show the price and decide button state
            priceText.transform.parent.gameObject.SetActive(true); // Show price
            priceText.text = "PRICE : " + skinList[skinIndex].skinPrice;

            // Enable the Buy button only if the player has enough fruits
            bool canBuy = FruitsInBank() >= skinList[skinIndex].skinPrice;
            buyButton.interactable = canBuy;
            buySelectText.text = canBuy ? "BUY" : "NOT YET";
        }
    }
    #endregion
    #region Skin Shop System
    private bool BuySkin(int index)
    {
        // Check if the player has enough fruits without modifying the bank amount
        if (FruitsInBank() >= skinList[index].skinPrice)
        {
            // Deduct the fruits only after confirming the purchase
            int newFruitAmount = FruitsInBank() - skinList[index].skinPrice;
            PlayerPrefs.SetInt("TotalFruitsAmount", newFruitAmount);
            PlayerPrefs.Save();

            // Unlock the skin
            skinList[index].unlocked = true;
            PlayerPrefs.SetInt(skinList[index].skinName + "Unlocked", 1);
            PlayerPrefs.Save();

            // Refresh the display
            UpdateSkinDisplay();

            return true;
        }

        Debug.Log("Not enough fruits to buy this skin.");
        return false;
    }



    private int FruitsInBank() => PlayerPrefs.GetInt("TotalFruitsAmount");

    private bool HaveEnoughFruits(int price)
    {
        int currentFruits = FruitsInBank();
        Debug.Log("Current Fruits: " + currentFruits + ", Skin Price: " + price);

        if (FruitsInBank() > price)
        {
            PlayerPrefs.SetInt("TotalFruitsAmount", FruitsInBank() - price);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }
#endregion
}