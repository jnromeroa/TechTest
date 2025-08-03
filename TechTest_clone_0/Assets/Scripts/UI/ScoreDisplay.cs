using UnityEngine;
using TMPro;

/// <summary>
/// Displays the scores of two players using a TMP_Text component.
/// </summary>
[RequireComponent(typeof(TMP_Text))]
public class ScoreDisplay : MonoBehaviour
{
    /// <summary>
    /// Reference to the TMP_Text component.
    /// </summary>
    private TMP_Text _text;

    /// <summary>
    /// Score for Player 1.
    /// </summary>
    private int _scoreP1 = 0;

    /// <summary>
    /// Score for Player 2.
    /// </summary>
    private int _scoreP2 = 0;

    /// <summary>
    /// Initializes the TMP_Text component reference.
    /// </summary>
    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    /// <summary>
    /// Updates the score for Player 1 and refreshes the text display.
    /// </summary>
    /// <param name="score">The new score for Player 1.</param>
    public void UpdateScoreP1(int score)
    {
        _scoreP1 = score;
        UpdateText();
    }

    /// <summary>
    /// Updates the score for Player 2 and refreshes the text display.
    /// </summary>
    /// <param name="score">The new score for Player 2.</param>
    public void UpdateScoreP2(int score)
    {
        _scoreP2 = score;
        UpdateText();
    }

    /// <summary>
    /// Updates the TMP_Text with the current scores in "P1-P2" format.
    /// </summary>
    private void UpdateText()
    {
        _text.text = $"{_scoreP1}-{_scoreP2}";
    }
}
