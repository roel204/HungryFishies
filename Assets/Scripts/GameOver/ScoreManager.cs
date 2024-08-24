using TMPro;
using TMPro.Examples;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI moneyText;

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

        // Update the player's money based on the score
        GameManager.instance.ChangeMoney((int)score);
        int currentPearls = GameManager.instance.money;

        // Display the score, high score, and money in the UI
        highScoreText.text = "Score: " + score.ToString("F2") + "\nHighscore: " + highScore.ToString("F2");

        moneyText.text = currentPearls + "\n (+" + (int)score + ")";
    }
}
