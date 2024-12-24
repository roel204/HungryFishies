using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardsManager : MonoBehaviour
{
    public Transform rewardsContainer;
    public GameObject rewardUIPrefab;
    public TextMeshProUGUI titleText;

    private float highScore;
    private int[] rewardClaimed;
    private int[] rewardRequirements;
    private int[] rewardAmounts;

    public void ClaimReward(int rewardID)
    {
        if (highScore >= rewardRequirements[rewardID] && rewardClaimed[rewardID] == 0)
        {
            rewardClaimed[rewardID] = 1; // Mark reward as claimed
            GameManager.instance.ChangeMoney(rewardAmounts[rewardID]);
            PlayerPrefs.SetInt("Reward_" + GameManager.instance.fishDataList[GameManager.instance.selectedFish].id + "_" + rewardID, 1);
            PlayerPrefs.Save();
            Debug.Log($"Reward {rewardID} claimed!");
            UpdateUI();
        }
        else
        {
            Debug.Log("Reward cannot be claimed (either already claimed or requirements not met).");
        }
    }

    private void UpdateUI()
    {
        // Retrieve data
        var selectedFishID = GameManager.instance.fishDataList[GameManager.instance.selectedFish].id;
        highScore = PlayerPrefs.GetFloat("HighScore_" + selectedFishID, 0);
        rewardRequirements = GameManager.instance.fishDataList[GameManager.instance.selectedFish].rewardRequirements;
        rewardAmounts = GameManager.instance.fishDataList[GameManager.instance.selectedFish].rewardAmounts;

        titleText.text = GameManager.instance.fishDataList[GameManager.instance.selectedFish].name + " Milestones";

        // Initialize or resize rewardClaimed array
        rewardClaimed = new int[rewardRequirements.Length];

        for (int i = 0; i < rewardRequirements.Length; i++)
        {
            rewardClaimed[i] = PlayerPrefs.GetInt("Reward_" + selectedFishID + "_" + i, 0);
        }

        // Clear and rebuild rewards UI
        foreach (Transform child in rewardsContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < rewardRequirements.Length; i++)
        {
            // Instantiate reward UI prefab
            GameObject rewardUI = Instantiate(rewardUIPrefab, rewardsContainer);
            TextMeshProUGUI rewardText = rewardUI.transform.Find("RewardText").GetComponent<TextMeshProUGUI>();
            Button claimButton = rewardUI.transform.Find("ClaimButton").GetComponent<Button>();
            TextMeshProUGUI buttonText = claimButton.GetComponentInChildren<TextMeshProUGUI>();

            // Update reward text and button state
            rewardText.text = $"Milestone {i + 1}: {rewardRequirements[i]} seconds";
            if (rewardClaimed[i] == 1)
            {
                buttonText.text = "Claimed";
                claimButton.interactable = false;
            }
            else
            {
                claimButton.onClick.RemoveAllListeners();
                buttonText.text = $"Claim {rewardAmounts[i]}";
                claimButton.interactable = highScore >= rewardRequirements[i];

                int rewardIndex = i; // Capture index for the button callback
                claimButton.onClick.AddListener(() => ClaimReward(rewardIndex));
            }
        }
    }

    public void Open()
    {
        UpdateUI();
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
