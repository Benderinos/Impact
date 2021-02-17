﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volimeSlider;
    public TMP_Dropdown dropDownGraphics;
    public bool isShown = false;
    public MenuTweenManager menuTweenManager;

    void Start()
    {
        volimeSlider.value = 0;
        dropDownGraphics.value = 2;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isShown = false;
            menuTweenManager.Options(false);
        }
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}
