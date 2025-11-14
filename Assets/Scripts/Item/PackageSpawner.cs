using System.Collections;
using UnityEngine;

public class PackageSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private ObjectPool packagePool;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int totalPackagesToSpawn = 15;
    [SerializeField] private float respawnDelay = 2f;
    [SerializeField] private int initialSpawnCount = 3;

    private int currentSpawnedCount = 0;

    void Start()
    {
        StartCoroutine(DelayedSpawn());
    }

    IEnumerator DelayedSpawn()
    {
        yield return null;
        SpawnInitialPackages();
    }

    // 게임 시작 시 초기 패키지 스폰
    void SpawnInitialPackages()
    {
        for (int i = 0; i < initialSpawnCount; i++)
        {
            SpawnPackage();
        }
    }

    // 패키지 1개 스폰
    public void SpawnPackage()
    {
        if (currentSpawnedCount >= totalPackagesToSpawn)
        {
            Debug.Log("모든 패키지 스폰 완료!");
            return;
        }

        // 랜덤 스폰 포인트 선택
        Vector3 spawnPosition = GetRandomSpawnPoint();

        // 풀에서 패키지 가져오기
        GameObject package = packagePool.Get();

        if (package != null)
        {
            package.transform.position = spawnPosition;
            package.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

            // Package 스크립트 리셋
            Package packageScript = package.GetComponent<Package>();
            if (packageScript != null)
            {
                packageScript.ResetPackage();
            }

            currentSpawnedCount++;
            Debug.Log($"Package Spawned: {currentSpawnedCount}/{totalPackagesToSpawn}");
        }
    }

    // 랜덤 스폰 포인트 위치 반환
    Vector3 GetRandomSpawnPoint()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("스폰 포인트가 설정되지 않았습니다!");
            return Vector3.zero;
        }

        int randomIndex = Random.Range(0, spawnPoints.Length);
        return spawnPoints[randomIndex].position;
    }

    // 패키지 수집 후 호출 (GameManager에서 호출)
    public void OnPackageCollected()
    {
        // 2초 후 다시 스폰
        StartCoroutine(RespawnAfterDelay());
    }

    IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnPackage();
    }
}
