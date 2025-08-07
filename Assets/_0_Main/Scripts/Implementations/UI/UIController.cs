using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Canvases")]
    [SerializeField] GameObject startCanvas;
    [SerializeField] GameObject playCanvas;

    [Header("Start Screen")]
    [SerializeField] Button startButton;
    [SerializeField] LayoutSelector layoutSelector;

    [Header("Play Screen")]
    [SerializeField] Button homeButton;
    [SerializeField] TextMeshProUGUI matchesText;
    [SerializeField] TextMeshProUGUI triesText;

    // Intent events for Composition Root
    public event Action OnStart;
    public event Action<int, int, int> OnLevelSelected;
    public event Action OnHome;

    void Awake()
    {
        // Initially only start screen
        startCanvas.SetActive(true);
        playCanvas.SetActive(false);

        // Start button → switch canvases + notify
        startButton.onClick.AddListener(() =>
        {
            startCanvas.SetActive(false);
            playCanvas.SetActive(true);
            OnStart?.Invoke();
        });

        // Dropdown → pass through rows, cols, index
        layoutSelector.OnLayoutSelected += (r, c, i) =>
            OnLevelSelected?.Invoke(r, c, i);

        // Home button → switch back + notify
        homeButton.onClick.AddListener(() =>
        {
            playCanvas.SetActive(false);
            startCanvas.SetActive(true);
            OnHome?.Invoke();
        });
    }

    /// <summary>
    /// Populate the level dropdown. Call this once from Composition Root.
    /// </summary>
    public void InitializeLevelDropdown(System.Collections.Generic.List<GameConfig.LayoutPreset> presets, int defaultIndex)
    {
        layoutSelector.Initialize(presets, defaultIndex);
    }

    /// <summary>
    /// Update the on-screen score.
    /// </summary>
    public void UpdateScore(int matches, int tries)
    {
        matchesText.text = $"Matches: {matches}";
        triesText.text = $"Tries: {tries}";
    }
    public void HandleGameOver()
    {

    }
}