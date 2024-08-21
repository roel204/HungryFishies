using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;

    private void Start()
    {
        // Get the selected fish index
        int selectedFishIndex = GameManager.instance.selectedFish;
        string highScoreKey = "HighScoreFish_" + selectedFishIndex;

        // Get the current score and the high score for the selected fish
        float score = GameManager.instance.score;
        float highScore = PlayerPrefs.GetFloat(highScoreKey, 0);

        // Update the high score if the current score is higher
        if (score > highScore)
        {
            highScore = score;

            PlayerPrefs.SetFloat(highScoreKey, highScore);
            PlayerPrefs.Save();
        }

        // Update the player's pearls based on the score
        GameManager.instance.ChangePearls((int)score);
        int currentPearls = GameManager.instance.pearls;

        // Display the score, high score, and pearls in the UI
        highScoreText.text = "Score: " + score.ToString("F2") +
                             "\nHighscore: " + highScore.ToString("F2") +
                             "\nPearls: " + currentPearls + " (+" + (int)score + ")";
    }
}
