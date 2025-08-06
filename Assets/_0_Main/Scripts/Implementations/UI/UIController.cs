using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _matchesText;
    [SerializeField] TextMeshProUGUI _triesText;
    [SerializeField] TextMeshProUGUI _leaderBoardText;

    public void UpdateScore(int matches, int tries)
    {
        _matchesText.text = $"Matches: {matches}";
        _triesText.text = $"Tries: {tries}";
    }

    public void LeaderBoardUpdate(int level, int matches, int tries)
    {
        _matchesText.text = $"Matches: {matches}";
        _triesText.text = $"Tries: {tries}";
    }

}