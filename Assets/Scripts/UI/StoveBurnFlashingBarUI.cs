using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnFlashingBarUI : MonoBehaviour
{
    private const string IS_FLASHING_ANIMATION = "isFlashing";

    [SerializeField] private StoveCounter stoveCounter;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounterOnProgressChanged;

        animator.SetBool(IS_FLASHING_ANIMATION, false);
    }

    private void StoveCounterOnProgressChanged(object sender, IProgressController.OnProgressChangedEventArgs e)
    {
        float burnShowProgressAmount = 0.5f;
        bool show = stoveCounter.isFried() && e.progressNormalized >= burnShowProgressAmount;

        animator.SetBool(IS_FLASHING_ANIMATION, show);
    }
}