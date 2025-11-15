using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;

    // Animator Parameter 이름 (문자열 대신 Hash 사용 - 최적화)
    private int speedHash;
    private int isJumpingHash;

    // 현재 이동 속도
    private Vector3 lastPosition;
    private float currentSpeed = 0f;

    void Start()
    {
        // 컴포넌트 자동 찾기
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (playerController == null)
        {
            playerController = GetComponent<PlayerController>();
        }

        // Animator Parameter Hash 미리 계산
        speedHash = Animator.StringToHash("Speed");
        isJumpingHash = Animator.StringToHash("IsJumping");

        lastPosition = transform.position;
    }

    void Update()
    {
        UpdateAnimationParameters();
    }

    void UpdateAnimationParameters()
    {
        // 현재 이동 속도 계산
        float distance = Vector3.Distance(transform.position, lastPosition);
        currentSpeed = distance / Time.deltaTime;
        lastPosition = transform.position;

        // Speed 파라미터 업데이트 (0 ~ 2 범위로 정규화)
        float normalizedSpeed = Mathf.Clamp01(currentSpeed / 5f); // 5 = moveSpeed
        animator.SetFloat(speedHash, normalizedSpeed * 2f); // 0~2

        // IsJumping 파라미터 업데이트
        // PlayerController의 isGrounded를 public으로 만들어야 함
        bool isGrounded = CheckGrounded();
        animator.SetBool(isJumpingHash, !isGrounded);
    }

    // 지면 체크 (PlayerController와 동일한 로직)
    bool CheckGrounded()
    {
        Vector3 spherePosition = transform.position - new Vector3(0, 1f, 0);
        return Physics.CheckSphere(spherePosition, 0.2f);
    }
}