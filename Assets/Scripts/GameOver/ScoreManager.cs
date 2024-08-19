using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;

    private void Start()
    {
        float score = GameManager.instance.score;
        float highScore = PlayerPrefs.GetFloat("HighScore");

        if (score > highScore)
        {
            highScore = score;

            PlayerPrefs.SetFloat("HighScore", highScore);
            PlayerPrefs.Save();
        }

        GameManager.instance.AddPearls((int)score);
        int currentPearls = PlayerPrefs.GetInt("Pearls");

        highScoreText.text = "Score: " + score.ToString("F2") + "\nHighscore: " + highScore.ToString("F2") + "\nPearls: " + currentPearls;
    }
}
