using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;

    // key bindings

    [SerializeField] private Button moveUpBindingButton;
    [SerializeField] private Button moveDownBindingButton;
    [SerializeField] private Button moveLeftBindingButton;
    [SerializeField] private Button moveRightBindingButton;
    [SerializeField] private Button interactBindingButton;
    [SerializeField] private Button interactAlternateBindingButton;
    [SerializeField] private Button pauseBindingButton;
    [SerializeField] private Button gamepadInteractBindingButton;
    [SerializeField] private Button gamepadInteractAlternateBindingButton;
    [SerializeField] private Button gamepadPauseBindingButton;

    [SerializeField] private TextMeshProUGUI moveUpBindingButtonText;
    [SerializeField] private TextMeshProUGUI moveDownBindingButtonText;
    [SerializeField] private TextMeshProUGUI moveLeftBindingButtonText;
    [SerializeField] private TextMeshProUGUI moveRightBindingButtonText;
    [SerializeField] private TextMeshProUGUI interactBindingButtonText;
    [SerializeField] private TextMeshProUGUI interactAlternateBindingButtonText;
    [SerializeField] private TextMeshProUGUI pauseBindingButtonText;
    [SerializeField] private TextMeshProUGUI gamepadInteractBindingButtonText;
    [SerializeField] private TextMeshProUGUI gamepadInteractAlternateBindingButtonText;
    [SerializeField] private TextMeshProUGUI gamepadPauseBindingButtonText;

    [SerializeField] private Transform pressToRebindKeyTransform;

    private Action onCloseButtonAction;


    private void Awake()
    {
        Instance = this;

        soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() =>
        {
            Hide();
            onCloseButtonAction();
        });

        moveUpBindingButton.onClick.AddListener(() => { RebindKey(GameInput.Binding.Move_Up); });
        moveDownBindingButton.onClick.AddListener(() => { RebindKey(GameInput.Binding.Move_Down); });
        moveLeftBindingButton.onClick.AddListener(() => { RebindKey(GameInput.Binding.Move_Left); });
        moveRightBindingButton.onClick.AddListener(() => { RebindKey(GameInput.Binding.Move_Right); });
        interactBindingButton.onClick.AddListener(() => { RebindKey(GameInput.Binding.Interact); });
        interactAlternateBindingButton.onClick.AddListener(() => { RebindKey(GameInput.Binding.Interact_Alt); });
        pauseBindingButton.onClick.AddListener(() => { RebindKey(GameInput.Binding.Pause); });
        gamepadInteractBindingButton.onClick.AddListener(() => { RebindKey(GameInput.Binding.Gamepad_Interact); });
        gamepadInteractAlternateBindingButton.onClick.AddListener(() =>
        {
            RebindKey(GameInput.Binding.Gamepad_InteractAlternate);
        });
        gamepadPauseBindingButton.onClick.AddListener(() => { RebindKey(GameInput.Binding.Gamepad_Pause); });
    }

    private void Start()
    {
        GameManager.Instance.OnGameUnpaused += GameManagerOnGameUnpaused;

        UpdateVisual();

        Hide();
        HidePressToRebindKey();
    }

    private void GameManagerOnGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        soundEffectsText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);

        moveUpBindingButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveDownBindingButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveLeftBindingButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        moveRightBindingButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        interactBindingButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAlternateBindingButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact_Alt);
        pauseBindingButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        gamepadInteractBindingButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact);
        gamepadInteractAlternateBindingButtonText.text =
            GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_InteractAlternate);
        gamepadPauseBindingButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause);
    }

    public void Show(Action onCloseButtonAction)
    {
        this.onCloseButtonAction = onCloseButtonAction;

        gameObject.SetActive(true);
        soundEffectsButton.Select();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ShowPressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(true);
    }

    public void HidePressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(false);
    }

    private void RebindKey(GameInput.Binding binding)
    {
        ShowPressToRebindKey();
        GameInput.Instance.RebindKey(binding, () =>
        {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }
}