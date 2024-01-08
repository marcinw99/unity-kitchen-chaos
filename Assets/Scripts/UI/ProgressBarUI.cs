using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject progressControllerGameObject;
    [SerializeField] private Image barImage;

    private IProgressController progressController;

    private void Start()
    {
        progressController = progressControllerGameObject.GetComponent<IProgressController>();
        
        progressController.OnProgressChanged += ProgressControllerOnProgressChanged;
        barImage.fillAmount = 0f;
        Hide();
    }

    private void ProgressControllerOnProgressChanged(object sender, IProgressController.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.progressNormalized;

        if (e.progressNormalized == 0f || e.progressNormalized == 1f)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}