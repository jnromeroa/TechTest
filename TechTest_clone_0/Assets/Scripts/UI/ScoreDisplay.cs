using UnityEngine;
using TMPro;
public class ScoreDisplay : MonoBehaviour
{
    private TMP_Text _text;
    private int _scoreP1 = 0;
    private int _scoreP2 = 0;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    public void UpdateScoreP1(int score)
    {
        _scoreP1 = score;
        UpdateText();
    }

    public void UpdateScoreP2(int score)
    {
        _scoreP2 = score;
        UpdateText();
    }
    private void UpdateText()
    {
        _text.text = $"{_scoreP1}-{_scoreP2}";
    }


}
