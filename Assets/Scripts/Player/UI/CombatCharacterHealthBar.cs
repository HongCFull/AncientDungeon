using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class CombatCharacterHealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CombatCharacter combatCharacter;
    [SerializeField] private Image greenBar;
    [SerializeField] private Image fadeBar;
    [SerializeField] private TMPro.TextMeshProUGUI healthText;

    [Header("Settings")] 
    [SerializeField] [Range(1f, 5f)] private float fadeDelay; 
    [SerializeField] [Range(1f, 100f)] private float fadeSpeed;

    private float fadingDuration;
    private float changeOfFadeValue;    //fade speed = 100* percentageChangeOfFadeValue  / fading duration
    private IEnumerator fadeProgress;
    void Awake()
    {
        float hpPercentage = combatCharacter.GetCurrentHealth() / combatCharacter.GetMaxHealth();
        greenBar.fillAmount = hpPercentage;
        fadeBar.fillAmount = hpPercentage;
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        UpdateHealthText();
        
        if (fadeProgress != null) {
            UpdateFadingDuration();
        }

        float hpPercentage = combatCharacter.GetCurrentHealth() / combatCharacter.GetMaxHealth();
        greenBar.fillAmount = hpPercentage;
        fadeProgress = FadeHPBar();
        StartCoroutine(fadeProgress);
    }

    IEnumerator FadeHPBar()
    {
        yield return new WaitForSeconds(fadeDelay);
        UpdateFadingDuration();
        float progress = 0f;
        while (progress<=fadingDuration) {
            fadeBar.fillAmount -= fadeSpeed/100f *Time.deltaTime;
            progress += Time.deltaTime;
            yield return null;
        }
    }

    private void UpdateFadingDuration()
    {
        float hpPercentage = combatCharacter.GetCurrentHealth() / combatCharacter.GetMaxHealth();
        changeOfFadeValue = fadeBar.fillAmount - hpPercentage;  
        fadingDuration = 100 * changeOfFadeValue / fadeSpeed;
    }

    private void UpdateHealthText()
    {
        healthText.text = combatCharacter.GetCurrentHealth()+" / "+ combatCharacter.GetMaxHealth();
    }
}
