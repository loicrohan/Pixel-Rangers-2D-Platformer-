using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Settings_UI : MonoBehaviour
{
    //[SerializeField] private GameObject firstSelected;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float mixerMultiplier = 25;

    [Header("BGM Settings")]
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private TextMeshProUGUI BGMSliderText;
    [SerializeField] private string BGMParameter;

    [Header("SFX Settings")]
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private TextMeshProUGUI SFXSliderText;
    [SerializeField] private string SFXParameter;

    public void SFXSliderValue(float value)
    {
        SFXSliderText.text = Mathf.RoundToInt(value * 100) + "%";
        float newValue = Mathf.Log10(value) * mixerMultiplier;
        audioMixer.SetFloat(SFXParameter, newValue);
    }

    public void BGMSliderValue(float value)
    {
        BGMSliderText.text = Mathf.RoundToInt(value * 100) + "%";
        float newValue = Mathf.Log10(value) * mixerMultiplier;
        audioMixer.SetFloat(BGMParameter, newValue);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(SFXParameter, SFXSlider.value);
        PlayerPrefs.SetFloat(BGMParameter, BGMSlider.value);
    }

    private void OnEnable()
    {
        //GetComponentInParent<MainMenu_UI>().UpdateLastSelected(firstSelected);
        //EventSystem.current.SetSelectedGameObject(firstSelected);

        SFXSlider.value = PlayerPrefs.GetFloat(SFXParameter, .7f);
        BGMSlider.value = PlayerPrefs.GetFloat(BGMParameter, .7f);
    }
}