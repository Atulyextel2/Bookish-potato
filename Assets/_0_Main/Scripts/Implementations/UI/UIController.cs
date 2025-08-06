using UnityEngine;
using UnityEngine.UI;
using System;

public class UIController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject levelPanel;
    [SerializeField] GameObject gameOverPanel;

    [Header("Buttons")]
    [SerializeField] Button startButton;
    [SerializeField] Button homeButton;

    [Header("Level Selection")]
    [SerializeField] LayoutSelector layoutSelector;

    [Header("Score Display")]
    [SerializeField] Text matchesText;
    [SerializeField] Text triesText;

    public event Action OnStart;
    public event Action<int, int, int> OnLevelSelected;
    public event Action OnHome;

    void Awake()
    {
        // wire Start
        startButton.onClick.AddListener(() => OnStart?.Invoke());

        // wire Level dropdown
        layoutSelector.OnLayoutSelected += (r, c, i) => OnLevelSelected?.Invoke(r, c, i);

        // wire Home
        homeButton.onClick.AddListener(() => OnHome?.Invoke());
    }

    public void ShowStartPanel(bool show) => startPanel.SetActive(show);
    public void ShowLevelPanel(bool show) => levelPanel.SetActive(show);
    public void ShowGameOverPanel(bool show) => gameOverPanel.SetActive(show);

    public void UpdateScore(int matches, int tries)
    {
        matchesText.text = $"Matches: {matches}";
        triesText.text = $"Tries: {tries}";
    }
}