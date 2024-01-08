using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredValueText;

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManagerOnStateChanged;

        Hide();
    }


    private void GameManagerOnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGameOver())
        {
            recipesDeliveredValueText.text = DeliveryManager.Instance.GetSuccessfulDeliveriesAmount().ToString();

            Show();
        }
        else
        {
            Hide();
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