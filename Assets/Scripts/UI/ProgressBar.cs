using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image progressFiller;
    [SerializeField] private TextMeshProUGUI progressTxt;
    [SerializeField][Range(0,100f)] private float defaultProgressPercentage;
    
    private void OnEnable()
    {
        if (!progressFiller)
            throw new Exception("progressFiller of " + gameObject.name + " is not assigned");

        SetProgressBarToDefaultState();
    }
    private void OnDisable() => SetProgressBarToDefaultState();

    public void CloseAndResetProgressBar()
    {
        SetProgressBarToDefaultState();
        this.gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Update the progress filler and the progress text of this progress bar
    /// </summary>
    /// <param name="percentage"> 0 <= percentage <= 100 </param>
    public void UpdateProgressTo(float percentage)
    {
        progressFiller.fillAmount = percentage / 100f;
        progressTxt.text = percentage + " %";
    }

    private void SetProgressBarToDefaultState()
    {
        progressFiller.fillAmount = defaultProgressPercentage / 100f;
        progressTxt.text = defaultProgressPercentage + " %";
    }
}
