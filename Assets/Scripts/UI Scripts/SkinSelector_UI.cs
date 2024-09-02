using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSelector_UI : MonoBehaviour
{
    [SerializeField] private int currentIndex;
    [SerializeField] private int maxIndex;
    [SerializeField] private Animator skinDisplay;

    public void SelectSkin()
    {
        SkinManager.instance.SetSkinId(currentIndex);
    }

    public void NextSkin()
    {
        currentIndex++;

        if (currentIndex > maxIndex)
            currentIndex = 0;

        UpdateSkinDisplay();
        AudioManager.instance.PlaySFX(4);
    }

    public void PreviousSkin()
    {
        currentIndex--;

        if (currentIndex < 0)
            currentIndex = maxIndex;
        UpdateSkinDisplay();
        AudioManager.instance.PlaySFX(4);

    }

    // 2024-07-30 AI-Tag 
    // This was created with assistance from Muse, a Unity Artificial Intelligence product

    private void UpdateSkinDisplay()
    {
        for (int i = 0; i < skinDisplay.layerCount; i++)
        {
            skinDisplay.SetLayerWeight(i, i == currentIndex ? 1 : 0);
        }
    }

}