using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;

        timerText.text = "Time: " + timer.ToString("F2");
    }

    public float GetTimerValue()
    {
        return timer;
    }
}
