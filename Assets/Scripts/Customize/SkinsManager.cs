﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkinsManager : MonoBehaviour
{
    public GameObject player;
    public ParticleSystem playerParticleSys;
    public GameObject lockImage;
    public List<SkinInfo> skins = new List<SkinInfo>();
    private int currentSkin = 0;
    private int skinIndex = 0;
    private int currentLevel;
    public TextMeshProUGUI skinCoinsText;
    public TextMeshProUGUI skinLevelText;
    public GameObject nextButton;
    public GameObject previousButton;

    void Awake()
    {
        CustomizeData data = SaveSystem.LoadCustomize();
        if (data != null)
        {
            SkinInfo skin = skins.Find(item => item.skin.name == data.skin);
            currentSkin = skins.IndexOf(skin);
            skinIndex = skins.IndexOf(skin);
        }
        else
        {
            currentSkin = 0;
            skinIndex = 0;
        }
        GetPlayerData();
        SelectSkin(skins[currentSkin]);
    }

    private void GetPlayerData()
    {
        PlayerData playerData = SaveSystem.LoadPlayerData();
        if (playerData != null)
        {
            currentLevel = playerData.level;
        }
        else
        {
            currentLevel = 0;
        }
    }

    public void selectCurrentSkin()
    {
        GetPlayerData();
        currentSkin = skins.IndexOf(GetselectedItem());
        skinIndex = currentSkin;
        UpdateArrows();
    }

    private void SelectSkin(SkinInfo skin)
    {
        if(skin != null)
        {
            IsSkinEnable(skin);
            playerParticleSys.GetComponent<ParticleSystemRenderer>().material = skin.skin;
        }
    }

    public void NextSkin()
    {

        int index = currentSkin += 1;
        if(index < skins.Count-1)
        {
            currentSkin = index;
            previousButton.SetActive(true);
        }
        else
        {
            previousButton.SetActive(true);
            nextButton.SetActive(false);
        }
        SkinInfo skin = skins[index];
        if (skin != null)
        {
            IsSkinEnable(skin);
            DisplaySkinMaterial(skin);
            DisplaySkinEffect(skin);
        }
    }

    public void PreviousSkin()
    {
        int index = currentSkin -= 1;
        if (index > 0)
        {
            currentSkin = index;
            nextButton.SetActive(true);
        }
        else
        {
            nextButton.SetActive(true);
            previousButton.SetActive(false);
        }
        SkinInfo skin = skins[index];
        if (skin != null)
        {
            IsSkinEnable(skin);
            DisplaySkinMaterial(skin);
        }
    }

    private void DisplaySkinMaterial(SkinInfo skin)
    {
        if (skin.skin != null)
        {
            playerParticleSys.GetComponent<ParticleSystemRenderer>().material = skin.skin;
        }
    }

    private void DisplaySkinEffect(SkinInfo skin)
    {
        if (skin.effect != null)
        {
            GameObject effect = GameObject.FindGameObjectWithTag("Skin");
            if (effect != null)
            {
                Destroy(effect);
            }
            GameObject newSkin = Instantiate(skin.effect, player.transform.position, Quaternion.identity);
            newSkin.transform.parent = player.transform;
        }
    }

    public SkinInfo GetselectedItem()
    {
        if (skins[currentSkin].enabled)
        {
            return skins[currentSkin];
        }
        else
        {
            currentSkin = skinIndex;
            playerParticleSys.GetComponent<ParticleSystemRenderer>().material = skins[skinIndex].skin;
            return skins[skinIndex];
        }
        
    }

    public string GetSkinName()
    {
        return GetselectedItem().skin.name;
    }

    private void IsSkinEnable(SkinInfo skin)
    {
        if (skin.unlockLevel <= currentLevel)
        {
            if(skin.coins > 0) //TODO check if skin buyed
            {
                lockImage.SetActive(true);
                skinLevelText.gameObject.SetActive(false);
                //skinCoinsText.gameObject.SetActive(true);
                skinCoinsText.text = "" + skin.coins;
            }
            else
            {
                lockImage.SetActive(false);
                skinLevelText.gameObject.SetActive(false);
                //skinCoinsText.gameObject.SetActive(false);
                skin.enabled = true;
            }
        }
        else
        {
            lockImage.SetActive(true);
            skinLevelText.gameObject.SetActive(true);
            skinLevelText.text = "" + skin.unlockLevel;
            skin.enabled = false;
        }
    }

    public void DisableLockImage()
    {
        lockImage.SetActive(false);
    }

    private void UpdateArrows()
    {
        if(currentSkin == 0)
        {
            nextButton.SetActive(true);
            previousButton.SetActive(false);
        }
        if(currentSkin == skins.Count - 1)
        {
            previousButton.SetActive(true);
            nextButton.SetActive(false);
        }
    }
    public void levelUp()
    {
        currentLevel++;
    }
}
