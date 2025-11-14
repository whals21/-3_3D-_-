using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private int targetPackageCount = 15;
    [SerializeField] private float timeLimit = 180f; // 3분

    [Header("References")]
    [SerializeField] private PackageSpawner packageSpawner;

    // 게임 상태
    public enum GameState { Playing, Cleared, GameOver }
    private GameState currentState = GameState.Playing;

    // 게임 데이터
    private int collectedCount = 0;
    private float remainingTime;

    // Properties
    public int CollectedCount => collectedCount;
    public int TargetCount => targetPackageCount;
    public float RemainingTime => remainingTime;
    public GameState CurrentState => currentState;

    void Awake()
    {
        // 싱글톤 설정
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        remainingTime = timeLimit;
        currentState = GameState.Playing;

        Debug.Log($"Game Start! Target: {targetPackageCount} packages in {timeLimit} seconds");
    }

    void Update()
    {
        if (currentState != GameState.Playing) return;

        UpdateTimer();
    }

    void UpdateTimer()
    {
        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0)
        {
            remainingTime = 0;
            GameOver();
        }
    }

    // 패키지 수집 시 호출
    public void CollectPackage()
    {
        if (currentState != GameState.Playing) return;

        collectedCount++;
        Debug.Log($"Collected: {collectedCount}/{targetPackageCount}");

        // PackageSpawner에 알림 (재스폰)
        if (packageSpawner != null)
        {
            packageSpawner.OnPackageCollected();
        }

        // 클리어 조건 체크
        CheckClearCondition();
    }

    void CheckClearCondition()
    {
        if (collectedCount >= targetPackageCount)
        {
            GameClear();
        }
    }

    void GameClear()
    {
        currentState = GameState.Cleared;
        Debug.Log("🎉 Game Clear!");

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowClearPanel();
        }

        // 시간 정지
        Time.timeScale = 0f;
    }

    void GameOver()
    {
        currentState = GameState.GameOver;
        Debug.Log("⏰ Time Over! Game Over!");

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowGameOverPanel();
        }

        // 시간 정지
        Time.timeScale = 0f;
    }

    // 재시작
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 게임 종료
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}