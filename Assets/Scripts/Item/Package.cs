using UnityEngine;

public class Package : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float magnetRange = 3f;
    [SerializeField] private float magnetSpeed = 5f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem collectEffect;
    [SerializeField] private AudioClip collectSound;

    private Transform player;
    private bool isCollected = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 플레이어 찾기
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (isCollected || player == null) return;

        // 자력 효과 (선택 과제 ①)
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < magnetRange)
        {
            MagnetEffect();
        }
    }

    void MagnetEffect()
    {
        // 플레이어 방향으로 끌려가기
        Vector3 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * magnetSpeed;
    }

    void OnTriggerEnter(Collider other)
    {
        // 플레이어와 충돌 체크
        if (other.CompareTag("Player") && !isCollected)
        {
            Collect();
        }
    }

    void Collect()
    {
        isCollected = true;

        // GameManager에 수집 알림 
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CollectPackage();
        }

        Debug.Log("Package Collected!");

        // 이펙트 재생 (나중에 추가)
        if (collectEffect != null)
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity);
        }

        // 사운드 재생 (나중에 추가)
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }

        Debug.Log("Package Collected!");

        // 오브젝트 비활성화 (풀로 반환)
        gameObject.SetActive(false);
    }

    // 패키지 리셋 (오브젝트 풀에서 재사용 시)
    public void ResetPackage()
    {
        isCollected = false;
    
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        gameObject.SetActive(true);
    }
}