using UnityEngine;
using TMPro; // TextMeshPro 사용

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("In-Game UI")]
    [SerializeField] private TextMeshProUGUI collectedText;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Panels")]
    [SerializeField] private GameObject clearPanel;
    [SerializeField] private GameObject gameOverPanel;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (GameManager.Instance == null) return;

        // 점수 업데이트
        UpdateScore(GameManager.Instance.CollectedCount, GameManager.Instance.TargetCount);

        // 타이머 업데이트
        UpdateTimer(GameManager.Instance.RemainingTime);
    }

    void UpdateScore(int collected, int total)
    {
        if (collectedText != null)
        {
            collectedText.text = $"📦 수집: {collected}/{total}";
        }
    }

    void UpdateTimer(float timeInSeconds)
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeInSeconds / 60);
            int seconds = Mathf.FloorToInt(timeInSeconds % 60);
            timerText.text = $"⏰ 남은시간: {minutes}:{seconds:00}";

            // 시간이 30초 미만이면 빨간색으로 경고
            if (timeInSeconds < 30f)
            {
                timerText.color = Color.red;
            }
            else
            {
                timerText.color = Color.yellow;
            }
        }
    }

    public void ShowClearPanel()
    {
        if (clearPanel != null)
        {
            clearPanel.SetActive(true);
        }
    }

    public void ShowGameOverPanel()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
}