using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DifficultyButton_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI difficultyInfo;

    [TextArea]
    [SerializeField] private string description;

    public void OnPointerEnter(PointerEventData eventData)
    {
        difficultyInfo.text = description;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        difficultyInfo.text = "";
    }
}