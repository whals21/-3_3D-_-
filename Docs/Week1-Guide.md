# 🎮 Week 1 상세 작업 가이드

> 이 문서는 Unity에서 Rooftop Runner 게임의 Week 1 작업을 단계별로 수행하는 방법을 상세히 안내합니다.

---

## 📅 Day 1-2: 프로젝트 세팅 + 플레이어 기본 이동

### ✅ 목표
- Unity 프로젝트 생성 및 초기 설정
- 기본 맵 구성 (지면, 빌딩)
- CharacterController를 이용한 WASD 이동
- 점프 및 더블 점프 구현
- 3인칭 카메라 구현

---

### 1단계: Unity 프로젝트 생성

#### 1-1. Unity Hub에서 새 프로젝트 생성
1. **Unity Hub** 실행
2. **"새 프로젝트"** 또는 **"New Project"** 클릭
3. 템플릿 선택:
   - **3D (URP)** 선택 (Universal Render Pipeline)
   - 만약 3D (URP)가 없다면 **3D** 선택 후 나중에 URP 추가
4. 프로젝트 설정:
   - **프로젝트 이름**: `RooftopRunner`
   - **위치**: 원하는 폴더 지정
5. **"프로젝트 생성"** 클릭
6. Unity 에디터가 열릴 때까지 대기 (1~3분 소요)

#### 1-2. 프로젝트 구조 설정
1. **Project 패널** (하단)에서 `Assets` 폴더 확인
2. `Assets` 폴더에 다음 폴더들을 생성:
   - **우클릭** → **Create** → **Folder**
   - 폴더 이름:
     - `Scripts`
     - `Prefabs`
     - `Materials`
     - `Scenes`
     - `Animations`

3. `Scripts` 폴더 내부에 하위 폴더 생성:
   - `Player`
   - `Item`
   - `Environment`
   - `Manager`
   - `UI`

#### 1-3. 씬 저장
1. **File** → **Save As...**
2. `Assets/Scenes` 폴더 선택
3. 씬 이름: `MainScene`
4. **저장** 클릭

---

### 2단계: 기본 맵 구성

#### 2-1. 지면 생성
1. **Hierarchy 패널** (좌측)에서 우클릭
2. **3D Object** → **Plane** 선택
3. Plane이 생성되면 이름을 `Ground`로 변경
4. **Inspector 패널** (우측)에서 Transform 설정:
   - Position: `(0, 0, 0)`
   - Rotation: `(0, 0, 0)`
   - Scale: `(5, 1, 5)` ← 넓은 바닥 생성

#### 2-2. 지면 Material 생성
1. **Project 패널**에서 `Assets/Materials` 폴더 선택
2. 우클릭 → **Create** → **Material**
3. Material 이름: `GroundMaterial`
4. **Inspector**에서:
   - **Albedo** 색상 클릭 → 회색 또는 원하는 색상 선택 (예: RGB 150, 150, 150)
5. **Hierarchy**에서 `Ground` 오브젝트 선택
6. `GroundMaterial`을 **Project**에서 드래그하여 **Scene View**의 Ground에 드롭

#### 2-3. 빌딩 생성 (3~5개)
1. **Hierarchy** 우클릭 → **3D Object** → **Cube**
2. 이름을 `Building_01`로 변경
3. **Inspector**에서 Transform 설정:
   - Position: `(10, 5, 0)`
   - Rotation: `(0, 0, 0)`
   - Scale: `(4, 10, 4)` ← 높은 빌딩

4. 같은 방식으로 빌딩 4개 더 생성:
   - `Building_02`: Position `(-10, 7, 5)`, Scale `(5, 14, 5)`
   - `Building_03`: Position `(0, 4, 15)`, Scale `(3, 8, 3)`
   - `Building_04`: Position `(15, 6, -10)`, Scale `(4, 12, 4)`
   - `Building_05`: Position `(-8, 5, -12)`, Scale `(3, 10, 3)`

#### 2-4. 빌딩 Material 생성
1. `Assets/Materials`에서 새 Material 생성: `BuildingMaterial`
2. Albedo 색상: 갈색 또는 회색 (예: RGB 100, 80, 60)
3. 모든 Building 오브젝트에 드래그하여 적용

#### 2-5. 조명 확인
1. **Hierarchy**에서 `Directional Light` 선택 (기본 생성되어 있음)
2. **Inspector**에서:
   - Rotation: `(50, -30, 0)` ← 자연스러운 그림자
   - Intensity: `1`

---

### 3단계: 플레이어 오브젝트 생성

#### 3-1. 플레이어 기본 오브젝트
1. **Hierarchy** 우클릭 → **3D Object** → **Capsule**
2. 이름을 `Player`로 변경
3. **Inspector** Transform:
   - Position: `(0, 1, 0)` ← 지면 위에 배치
   - Rotation: `(0, 0, 0)`
   - Scale: `(1, 1, 1)`

#### 3-2. CharacterController 컴포넌트 추가
1. `Player` 오브젝트 선택 상태에서
2. **Inspector** 하단 **Add Component** 클릭
3. 검색창에 `CharacterController` 입력
4. **CharacterController** 선택
5. CharacterController 설정 확인:
   - **Center**: `(0, 1, 0)`
   - **Radius**: `0.5`
   - **Height**: `2`
   - **Slope Limit**: `45`
   - **Step Offset**: `0.3`

**중요**: Capsule Collider가 있다면 제거:
- **Inspector**에서 **Capsule Collider** 찾기
- 우클릭 → **Remove Component**

#### 3-3. Player Material 생성
1. `Assets/Materials`에서 새 Material: `PlayerMaterial`
2. Albedo 색상: 파란색 (예: RGB 50, 100, 200)
3. `Player` Capsule에 적용

#### 3-4. Player Tag 설정
1. `Player` 오브젝트 선택
2. **Inspector** 상단 **Tag** 드롭다운 클릭
3. **Player** 선택 (기본 태그)

---

### 4단계: PlayerController 스크립트 작성

#### 4-1. 스크립트 파일 생성
1. **Project** → `Assets/Scripts/Player` 폴더 선택
2. 우클릭 → **Create** → **C# Script**
3. 이름: `PlayerController` (철자 정확히!)
4. Enter로 확정

#### 4-2. 스크립트 편집
1. `PlayerController` 더블클릭 → Visual Studio 또는 코드 에디터 열림
2. 아래 코드를 **전체 복사하여 붙여넣기**:

```csharp
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float jumpForce = 8f;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    // Components
    private CharacterController controller;

    // Movement variables
    private Vector3 velocity;
    private bool isGrounded;
    private bool canDoubleJump;

    // Constants
    private float gravity = -20f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // groundLayer를 Everything으로 설정 (나중에 Inspector에서 조정)
        groundLayer = ~0;
    }

    void Update()
    {
        CheckGround();
        Move();
        Jump();
        ApplyGravity();
    }

    void CheckGround()
    {
        // 캐릭터 발 위치에서 Raycast로 지면 체크
        Vector3 spherePosition = transform.position - new Vector3(0, 1f, 0);
        isGrounded = Physics.CheckSphere(spherePosition, groundCheckDistance, groundLayer);

        // 땅에 닿으면 더블점프 리셋
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 약간의 downward force
            canDoubleJump = true;
        }
    }

    void Move()
    {
        // WASD 입력 받기
        float horizontal = Input.GetAxis("Horizontal"); // A, D
        float vertical = Input.GetAxis("Vertical");     // W, S

        // 이동 방향 계산
        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;

        // 달리기 체크 (Left Shift)
        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= sprintMultiplier;
        }

        // CharacterController로 이동
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);
    }

    void Jump()
    {
        // Space 키 입력 체크
        if (Input.GetButtonDown("Jump"))
        {
            // 지면에 있으면 일반 점프
            if (isGrounded)
            {
                velocity.y = jumpForce;
                canDoubleJump = true; // 더블점프 가능하게 설정
            }
            // 공중에 있고 더블점프 가능하면
            else if (canDoubleJump)
            {
                velocity.y = jumpForce;
                canDoubleJump = false; // 더블점프 사용
                Debug.Log("Double Jump!");
            }
        }
    }

    void ApplyGravity()
    {
        // 중력 적용
        velocity.y += gravity * Time.deltaTime;

        // Y축 이동 적용
        controller.Move(velocity * Time.deltaTime);
    }

    // Gizmos로 Ground Check 시각화 (Scene View에서만 보임)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 spherePosition = transform.position - new Vector3(0, 1f, 0);
        Gizmos.DrawWireSphere(spherePosition, groundCheckDistance);
    }
}
```

3. **Ctrl + S** (또는 Cmd + S)로 저장
4. Unity 에디터로 돌아오기

#### 4-3. 스크립트를 Player에 추가
1. **Hierarchy**에서 `Player` 오브젝트 선택
2. **Project**에서 `PlayerController` 스크립트를 드래그
3. **Inspector**의 `Player` 오브젝트에 드롭
   - 또는: **Inspector** 하단 **Add Component** → `PlayerController` 검색 후 추가

#### 4-4. Ground Layer 설정
1. `Player` 선택 → **Inspector**에서 **PlayerController** 컴포넌트 찾기
2. **Ground Layer** 드롭다운 클릭
3. **Everything** 체크 (또는 나중에 Ground 레이어만 선택)

---

### 5단계: 3인칭 카메라 구현

#### 5-1. CameraFollow 스크립트 생성
1. **Project** → `Assets/Scripts/Player` 폴더
2. 우클릭 → **Create** → **C# Script**
3. 이름: `CameraFollow`

#### 5-2. 스크립트 작성
1. `CameraFollow` 더블클릭
2. 아래 코드 전체 복사하여 붙여넣기:

```csharp
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target; // 플레이어

    [Header("Camera Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0, 3, -7);
    [SerializeField] private float smoothSpeed = 0.125f;

    [Header("Rotation Settings")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float minVerticalAngle = -20f;
    [SerializeField] private float maxVerticalAngle = 60f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        // 마우스 커서 숨기기 (선택사항)
        // Cursor.lockState = CursorLockMode.Locked;

        if (target == null)
        {
            Debug.LogError("Camera Target이 설정되지 않았습니다!");
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 마우스 입력으로 카메라 회전
        RotateCamera();

        // 카메라 위치 계산
        FollowTarget();
    }

    void RotateCamera()
    {
        // 마우스 입력 받기
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Y축 회전 (좌우)
        rotationY += mouseX;

        // X축 회전 (상하) - 제한
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

        // 플레이어도 Y축 회전 적용 (캐릭터가 바라보는 방향)
        target.rotation = Quaternion.Euler(0, rotationY, 0);
    }

    void FollowTarget()
    {
        // 목표 위치 계산 (오프셋 + 회전 적용)
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
        Vector3 desiredPosition = target.position + rotation * offset;

        // 부드럽게 이동
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // 카메라가 플레이어를 바라보도록
        transform.LookAt(target.position + Vector3.up * 1.5f); // 플레이어 중심보다 약간 위
    }
}
```

3. **Ctrl + S**로 저장

#### 5-3. Main Camera 설정
1. **Hierarchy**에서 `Main Camera` 선택
2. **Inspector**에서 Transform 초기화:
   - Position: `(0, 3, -7)`
3. **Add Component** → `CameraFollow` 스크립트 추가
4. **Camera Follow** 컴포넌트에서:
   - **Target** 필드에 **Hierarchy**의 `Player` 오브젝트를 드래그하여 연결

---

### 6단계: 첫 테스트 플레이

#### 6-1. 플레이 모드 실행
1. Unity 상단 중앙의 **▶ Play 버튼** 클릭
2. **Game View**로 자동 전환됨

#### 6-2. 조작 테스트
- **W, A, S, D**: 이동 확인
- **Left Shift + 이동**: 달리기 확인
- **Space**: 점프 확인
- **Space (공중에서)**: 더블 점프 확인
- **마우스 이동**: 카메라 회전 확인

#### 6-3. 문제 해결
**문제 1**: 캐릭터가 움직이지 않음
- **해결**: `PlayerController` 스크립트가 Player에 제대로 추가되었는지 확인

**문제 2**: 캐릭터가 바닥을 뚫고 떨어짐
- **해결**: Ground에 Collider가 있는지 확인 (Plane은 기본적으로 Mesh Collider 있음)

**문제 3**: 점프가 안됨
- **해결**: **Edit** → **Project Settings** → **Input Manager** → **Jump** 축이 Space로 설정되어 있는지 확인

**문제 4**: 카메라가 이상하게 움직임
- **해결**: CameraFollow의 Target이 Player로 연결되었는지 확인

#### 6-4. 플레이 중지
- Unity 상단 **▶ 버튼** 다시 클릭 (빨간색에서 회색으로)

---

### 7단계: 씬 저장 및 Git 커밋 (선택)

#### 7-1. 씬 저장
1. **Ctrl + S** (Cmd + S) 눌러서 씬 저장
2. 또는 **File** → **Save**

#### 7-2. Git 커밋 (Git 사용 시)
1. Git 클라이언트 또는 터미널에서:
```bash
git add .
git commit -m "Day 1-2: 플레이어 이동 및 카메라 구현 완료"
git push origin 브랜치명
```

---

## ✅ Day 1-2 완료 체크리스트

- [x] Unity 프로젝트 생성 (RooftopRunner)
- [x] 폴더 구조 설정 (Scripts, Prefabs, Materials 등)
- [x] Ground Plane 생성 및 Material 적용
- [x] 빌딩 3~5개 배치
- [x] Player Capsule 생성 및 CharacterController 추가
- [x] PlayerController 스크립트 작성 및 적용
- [x] WASD 이동 작동 확인
- [x] 점프 및 더블 점프 작동 확인
- [x] CameraFollow 스크립트 작성 및 적용
- [x] 3인칭 카메라 작동 확인
- [x] 마우스로 카메라 회전 확인

모든 항목이 체크되면 **Day 3-4**로 진행하세요!

---

---

## 📅 Day 3-4: 패키지 스폰 + 수집 시스템

### ✅ 목표
- 패키지 프리팹 생성
- 하늘에서 떨어지는 스폰 시스템 구현
- 오브젝트 풀 시스템 구현
- 플레이어와 충돌 시 수집 처리
- 수집 시 사운드/파티클 효과

---

### 1단계: 패키지(Package) 오브젝트 생성

#### 1-1. 패키지 기본 오브젝트
1. **Hierarchy** 우클릭 → **3D Object** → **Cube**
2. 이름을 `Package`로 변경
3. **Inspector** Transform:
   - Position: `(0, 10, 0)` ← 일단 공중에 배치
   - Rotation: `(0, 45, 0)` ← 45도 회전으로 다이아몬드처럼
   - Scale: `(0.8, 0.8, 0.8)` ← 작게

#### 1-2. Material 적용
1. `Assets/Materials`에서 새 Material 생성: `PackageMaterial`
2. Albedo 색상: 노란색 (RGB 255, 220, 0)
3. `Package` Cube에 드래그하여 적용

#### 1-3. Rigidbody 추가 (중력으로 떨어지게)
1. `Package` 선택
2. **Inspector** → **Add Component** → `Rigidbody` 검색 후 추가
3. Rigidbody 설정:
   - **Mass**: `1`
   - **Drag**: `0`
   - **Angular Drag**: `0.5`
   - **Use Gravity**: ✅ 체크
   - **Is Kinematic**: ❌ 체크 해제

#### 1-4. Collider를 Trigger로 설정
1. `Package` 선택 → **Inspector**에서 **Box Collider** 찾기
2. **Is Trigger**: ✅ 체크
   - 이유: Trigger로 설정해야 플레이어가 통과하면서 수집 가능

**중요**: Rigidbody가 있는 오브젝트에 Trigger Collider가 있으면 OnTriggerEnter 감지 가능

#### 1-5. Package Tag 생성 및 설정
1. `Package` 선택
2. **Inspector** 상단 **Tag** 드롭다운 → **Add Tag...**
3. **Tags** 섹션에서 **+** 클릭
4. 새 태그 이름: `Package` 입력 후 **Save**
5. 다시 `Package` 오브젝트 선택
6. **Tag** 드롭다운에서 **Package** 선택

---

### 2단계: Package 스크립트 작성

#### 2-1. 스크립트 생성
1. **Project** → `Assets/Scripts/Item` 폴더
2. 우클릭 → **Create** → **C# Script**
3. 이름: `Package`

#### 2-2. 스크립트 작성
```csharp
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

        // GameManager에 수집 알림 (나중에 구현)
        // GameManager.Instance.CollectPackage();

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

        // 오브젝트 비활성화 (풀로 반환)
        gameObject.SetActive(false);
    }

    // 패키지 리셋 (오브젝트 풀에서 재사용 시)
    public void ResetPackage()
    {
        isCollected = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        gameObject.SetActive(true);
    }
}
```

3. **Ctrl + S** 저장

#### 2-3. 스크립트 적용
1. `Package` 오브젝트 선택
2. **Add Component** → `Package` 스크립트 추가
3. Inspector에서 설정 확인:
   - **Magnet Range**: `3`
   - **Magnet Speed**: `5`

---

### 3단계: Package를 Prefab으로 만들기

#### 3-1. Prefab 생성
1. **Hierarchy**에서 `Package` 오브젝트 선택
2. **Project** → `Assets/Prefabs` 폴더 열기
3. `Package` 오브젝트를 **Hierarchy에서 드래그**하여 **Prefabs 폴더에 드롭**
4. Prefab이 생성되면 파란색 큐브 아이콘으로 표시됨

#### 3-2. Hierarchy에서 Package 삭제
1. **Hierarchy**의 `Package` 오브젝트 선택
2. **Delete** 키 누르기 (또는 우클릭 → Delete)
   - 이유: Prefab으로만 관리하고 씬에는 스폰 시스템이 생성함

---

### 4단계: 스폰 포인트 생성

#### 4-1. 스폰 포인트 부모 오브젝트 생성
1. **Hierarchy** 우클릭 → **Create Empty**
2. 이름: `SpawnPoints`
3. Transform Position: `(0, 0, 0)`

#### 4-2. 스폰 포인트 자식 오브젝트 생성 (5~8개)
1. `SpawnPoints` 오브젝트 우클릭 → **Create Empty**
2. 이름: `SpawnPoint_01`
3. Transform Position: `(0, 30, 0)` ← 하늘 높이
4. 아이콘 표시 (선택사항):
   - **Inspector** 좌측 상단 큐브 아이콘 클릭
   - 색상 선택 (예: 빨간색)

5. 같은 방식으로 5~8개 생성:
   - `SpawnPoint_02`: `(10, 30, 5)`
   - `SpawnPoint_03`: `(-10, 30, -5)`
   - `SpawnPoint_04`: `(15, 30, -10)`
   - `SpawnPoint_05`: `(-12, 30, 8)`
   - (선택) `SpawnPoint_06`: `(5, 30, 15)`
   - (선택) `SpawnPoint_07`: `(-8, 30, -12)`
   - (선택) `SpawnPoint_08`: `(12, 30, 12)`

**팁**: SpawnPoint는 빌딩 위쪽 공중에 배치하여 떨어지는 패키지가 빌딩에 닿도록

---

### 5단계: ObjectPool 스크립트 작성

#### 5-1. 스크립트 생성
1. **Project** → `Assets/Scripts/Manager` 폴더
2. **Create** → **C# Script**
3. 이름: `ObjectPool`

#### 5-2. 스크립트 작성
```csharp
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize = 20;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        // 풀에 오브젝트 미리 생성
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            obj.transform.SetParent(transform); // 풀 오브젝트의 자식으로
            pool.Enqueue(obj);
        }

        Debug.Log($"Object Pool Initialized: {poolSize} objects");
    }

    // 풀에서 오브젝트 가져오기
    public GameObject Get()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            // 풀이 부족하면 새로 생성
            Debug.LogWarning("Pool is empty! Creating new object.");
            GameObject obj = Instantiate(prefab);
            obj.transform.SetParent(transform);
            return obj;
        }
    }

    // 풀에 오브젝트 반환
    public void Return(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        pool.Enqueue(obj);
    }
}
```

3. **Ctrl + S** 저장

#### 5-3. ObjectPool 오브젝트 생성
1. **Hierarchy** 우클릭 → **Create Empty**
2. 이름: `PackagePool`
3. **Add Component** → `ObjectPool` 스크립트 추가
4. **Inspector**에서:
   - **Prefab**: **Project**의 `Prefabs/Package` 프리팹을 드래그하여 연결
   - **Pool Size**: `20`

---

### 6단계: PackageSpawner 스크립트 작성

#### 6-1. 스크립트 생성
1. `Assets/Scripts/Item` 폴더
2. **C# Script** 생성: `PackageSpawner`

#### 6-2. 스크립트 작성
```csharp
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
```

3. **Ctrl + S** 저장

#### 6-3. PackageSpawner 오브젝트 생성
1. **Hierarchy** 우클릭 → **Create Empty**
2. 이름: `PackageSpawner`
3. **Add Component** → `PackageSpawner` 스크립트 추가
4. **Inspector**에서 연결:
   - **Package Pool**: `PackagePool` 오브젝트 드래그
   - **Spawn Points**:
     - **Size**: `5` (또는 생성한 스폰 포인트 개수)
     - 각 **Element**에 `SpawnPoint_01`, `SpawnPoint_02`, ... 드래그
   - **Total Packages To Spawn**: `15`
   - **Respawn Delay**: `2`
   - **Initial Spawn Count**: `3`

**Spawn Points 배열 채우는 방법**:
1. **Spawn Points** 옆 화살표 클릭하여 펼치기
2. **Size**를 스폰 포인트 개수로 설정 (예: 5)
3. **Hierarchy**에서 `SpawnPoints` 오브젝트 펼치기
4. `SpawnPoint_01`을 **Element 0**에 드래그
5. `SpawnPoint_02`를 **Element 1**에 드래그
6. 반복...

---

### 7단계: 테스트 플레이

#### 7-1. 플레이 모드 실행
1. **▶ Play** 버튼 클릭
2. **Scene View**와 **Game View** 전환하며 확인

#### 7-2. 확인 사항
- [x] 게임 시작 시 패키지 3개가 하늘에서 떨어지는가?
- [x] 패키지가 땅에 떨어져서 굴러가는가?
- [x] 플레이어가 패키지에 가까이 가면 끌려오는가? (자력 효과)
- [x] 플레이어가 패키지와 충돌하면 사라지는가?
- [x] Console에 "Package Collected!" 메시지가 뜨는가?
- [ ] 2초 후 새로운 패키지가 스폰되는가?

#### 7-3. 문제 해결
**문제 1**: 패키지가 스폰되지 않음
- `PackageSpawner`의 Package Pool이 연결되었는지 확인
- Spawn Points 배열이 비어있지 않은지 확인

**문제 2**: 패키지가 플레이어를 뚫고 지나감
- `Package`의 Box Collider가 **Is Trigger** 체크되었는지 확인
- `Player`에 **CharacterController**가 있는지 확인 (Collider 역할)

**문제 3**: 자력 효과가 작동하지 않음
- `Package` 스크립트의 `player` 변수가 null이 아닌지 확인
- Player 오브젝트에 "Player" Tag가 설정되었는지 확인

---

### 8단계: 수집 파티클 효과 추가 (선택)

#### 8-1. 파티클 시스템 생성
1. **Hierarchy** 우클릭 → **Effects** → **Particle System**
2. 이름: `CollectEffect`
3. **Inspector**에서 파티클 설정:
   - **Duration**: `0.5`
   - **Start Lifetime**: `0.5`
   - **Start Speed**: `5`
   - **Start Size**: `0.2`
   - **Start Color**: 노란색
   - **Emission** → **Rate over Time**: `50`
   - **Shape** → **Shape**: Sphere, **Radius**: `0.5`

#### 8-2. Prefab으로 만들기
1. `CollectEffect`를 `Assets/Prefabs` 폴더로 드래그
2. **Hierarchy**에서 `CollectEffect` 삭제

#### 8-3. Package 스크립트에 연결
1. **Prefabs** 폴더에서 `Package` Prefab 더블클릭 (Prefab Edit 모드)
2. **Inspector**에서 **Package** 스크립트 찾기
3. **Collect Effect** 필드에 `CollectEffect` Prefab 드래그
4. Prefab 창 상단 **< (Back)** 버튼 클릭하여 나가기

---

## ✅ Day 3-4 완료 체크리스트

- [x] Package 큐브 생성 및 Material 적용
- [x] Rigidbody 추가 (중력 적용)
- [x] Box Collider를 Trigger로 설정
- [x] Package 스크립트 작성 및 적용
- [x] Package를 Prefab으로 생성
- [x] 스폰 포인트 5~8개 배치
- [x] ObjectPool 스크립트 작성 및 설정
- [x] PackageSpawner 스크립트 작성 및 설정
- [x] 초기 3개 패키지 스폰 확인
- [x] 수집 시 패키지 사라짐 확인
- [x] 자력 효과 작동 확인
- [ ] 2초 후 재스폰 확인
- [x] (선택) 수집 파티클 효과 추가

모든 항목이 완료되면 **Day 5-6**로 진행하세요!

---

---

## 📅 Day 5-6: 게임 매니저 + UI

### ✅ 목표
- GameManager 싱글톤 구현
- 수집 개수 추적 및 클리어 조건 체크
- 타이머 시스템 (180초)
- UI Canvas 구성 (점수, 타이머)
- 클리어/게임오버 패널

---

### 1단계: GameManager 스크립트 작성

#### 1-1. 스크립트 생성
1. `Assets/Scripts/Manager` 폴더
2. **C# Script** 생성: `GameManager`

#### 1-2. 스크립트 작성
```csharp
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

        // UI 패널 표시 (나중에 구현)
        // UIManager.Instance.ShowClearPanel();

        // 시간 정지
        Time.timeScale = 0f;
    }

    void GameOver()
    {
        currentState = GameState.GameOver;
        Debug.Log("⏰ Time Over! Game Over!");

        // UI 패널 표시 (나중에 구현)
        // UIManager.Instance.ShowGameOverPanel();

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
```

3. **Ctrl + S** 저장

#### 1-3. GameManager 오브젝트 생성
1. **Hierarchy** 우클릭 → **Create Empty**
2. 이름: `GameManager`
3. **Add Component** → `GameManager` 스크립트 추가
4. **Inspector**에서:
   - **Target Package Count**: `15`
   - **Time Limit**: `180`
   - **Package Spawner**: `PackageSpawner` 오브젝트 드래그

---

### 2단계: Package 스크립트 수정 (GameManager 연동)

#### 2-1. Package.cs 수정
1. `Assets/Scripts/Item/Package.cs` 열기
2. **Collect() 메서드** 수정:

```csharp
void Collect()
{
    isCollected = true;

    // ===== 이 부분 추가/수정 =====
    // GameManager에 수집 알림
    if (GameManager.Instance != null)
    {
        GameManager.Instance.CollectPackage();
    }
    // =============================

    Debug.Log("Package Collected!");

    // 이펙트 재생
    if (collectEffect != null)
    {
        Instantiate(collectEffect, transform.position, Quaternion.identity);
    }

    // 사운드 재생
    if (collectSound != null)
    {
        AudioSource.PlayClipAtPoint(collectSound, transform.position);
    }

    // 오브젝트 비활성화 (풀로 반환)
    gameObject.SetActive(false);
}
```

3. **Ctrl + S** 저장

---

### 3단계: UI Canvas 생성

#### 3-1. Canvas 생성
1. **Hierarchy** 우클릭 → **UI** → **Canvas**
2. Canvas가 자동 생성되며 **EventSystem**도 함께 생성됨
3. Canvas 이름: `UI_Canvas`

#### 3-2. Canvas 설정
1. `UI_Canvas` 선택
2. **Inspector** → **Canvas** 컴포넌트:
   - **Render Mode**: Screen Space - Overlay
3. **Canvas Scaler** 컴포넌트:
   - **UI Scale Mode**: Scale With Screen Size
   - **Reference Resolution**: `1920 x 1080`
   - **Match**: `0.5` (Width와 Height 중간)

---

### 4단계: 점수 UI 생성

#### 4-1. 배경 패널 생성 (선택사항)
1. `UI_Canvas` 우클릭 → **UI** → **Panel**
2. 이름: `ScorePanel`
3. **Rect Transform**:
   - **Anchors**: 좌측 상단 (Left-Top)
   - **Pos X**: `150`, **Pos Y**: `-50`
   - **Width**: `250`, **Height**: `100`
4. **Image** 컴포넌트:
   - **Color**: 반투명 검정 (R:0, G:0, B:0, A:150)

#### 4-2. 수집 개수 텍스트
1. `ScorePanel` 우클릭 → **UI** → **Text - TextMeshPro**
   - 처음이면 "Import TMP Essentials" 창 뜸 → **Import** 클릭
2. 이름: `CollectedText`
3. **Rect Transform**:
   - **Anchors**: Stretch (좌우상하 늘어남)
   - **Left**: `10`, **Right**: `-10`, **Top**: `-10`, **Bottom**: `10`
4. **TextMeshPro - Text (UI)** 컴포넌트:
   - **Text**: `📦 수집: 0/15`
   - **Font Size**: `24`
   - **Color**: 흰색
   - **Alignment**: 좌측 상단 (Left-Top)
   - **Font Style**: Bold (선택)

#### 4-3. 타이머 텍스트
1. `ScorePanel` 우클릭 → **UI** → **Text - TextMeshPro**
2. 이름: `TimerText`
3. **Rect Transform**:
   - **Anchors**: Stretch
   - **Left**: `10`, **Right**: `-10`, **Top**: `-50`, **Bottom**: `-10`
4. **TextMeshPro**:
   - **Text**: `⏰ 남은시간: 3:00`
   - **Font Size**: `24`
   - **Color**: 노란색 (RGB 255, 220, 0)
   - **Alignment**: 좌측 하단

---

### 5단계: UIManager 스크립트 작성

#### 5-1. 스크립트 생성
1. `Assets/Scripts/UI` 폴더
2. **C# Script**: `UIManager`

#### 5-2. 스크립트 작성
```csharp
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
```

3. **Ctrl + S** 저장

#### 5-3. UIManager 오브젝트 생성 및 연결
1. **Hierarchy** 우클릭 → **Create Empty**
2. 이름: `UIManager`
3. **Add Component** → `UIManager` 스크립트 추가
4. **Inspector**에서 연결:
   - **Collected Text**: `CollectedText` 오브젝트 드래그
   - **Timer Text**: `TimerText` 오브젝트 드래그
   - Clear Panel, Game Over Panel은 나중에 연결

---

### 6단계: 클리어 패널 생성

#### 6-1. 패널 배경
1. `UI_Canvas` 우클릭 → **UI** → **Panel**
2. 이름: `ClearPanel`
3. **Rect Transform**: 전체 화면 (기본값 유지)
4. **Image**:
   - **Color**: 반투명 검정 (R:0, G:0, B:0, A:200)
5. **Inspector** 상단에서 비활성화: 체크박스 해제 (게임 시작 시 숨김)

#### 6-2. 제목 텍스트
1. `ClearPanel` 우클릭 → **UI** → **Text - TextMeshPro**
2. 이름: `TitleText`
3. **Rect Transform**:
   - **Pos X**: `0`, **Pos Y**: `150`
   - **Width**: `600`, **Height**: `100`
4. **TextMeshPro**:
   - **Text**: `🎉 배달 완료!`
   - **Font Size**: `60`
   - **Color**: 노란색
   - **Alignment**: Center

#### 6-3. 정보 텍스트
1. `ClearPanel` 우클릭 → **UI** → **Text - TextMeshPro**
2. 이름: `InfoText`
3. **Rect Transform**:
   - **Pos Y**: `0`
   - **Width**: `500`, **Height**: `200`
4. **TextMeshPro**:
   - **Text**:
   ```
   수집: 15/15
   걸린 시간: 2:15
   ```
   - **Font Size**: `36`
   - **Alignment**: Center

#### 6-4. 다시 시작 버튼
1. `ClearPanel` 우클릭 → **UI** → **Button - TextMeshPro**
2. 이름: `RestartButton`
3. **Rect Transform**:
   - **Pos X**: `-120`, **Pos Y**: `-150`
   - **Width**: `200`, **Height**: `60`
4. 버튼 텍스트 수정:
   - `RestartButton` 펼치기 → `Text (TMP)` 선택
   - **Text**: `다시 시작`
   - **Font Size**: `28`

#### 6-5. 종료 버튼
1. `ClearPanel` 우클릭 → **UI** → **Button - TextMeshPro**
2. 이름: `QuitButton`
3. **Rect Transform**:
   - **Pos X**: `120`, **Pos Y**: `-150`
   - **Width**: `200`, **Height**: `60`
4. 텍스트:
   - **Text**: `종료`
   - **Font Size**: `28`

#### 6-6. 버튼 기능 연결
1. `RestartButton` 선택
2. **Inspector** → **Button** 컴포넌트 → **On Click()**
3. **+** 클릭
4. **Hierarchy**에서 `GameManager` 오브젝트를 드래그하여 **None (Object)** 칸에 드롭
5. 드롭다운: **GameManager** → **RestartGame()**

6. `QuitButton`도 같은 방식:
   - **GameManager** → **QuitGame()**

---

### 7단계: 게임오버 패널 생성

#### 7-1. 패널 복제
1. **Hierarchy**에서 `ClearPanel` 선택
2. **Ctrl + D** (복제)
3. 이름: `GameOverPanel`
4. 비활성화 상태 유지

#### 7-2. 텍스트 수정
1. `GameOverPanel` → `TitleText` 선택
2. **Text**: `⏰ 시간 초과!`
3. **Color**: 빨간색

#### 7-3. InfoText 수정
1. `GameOverPanel` → `InfoText`
2. **Text**:
```
시간이 부족했어요!
수집: 12/15
```

---

### 8단계: UIManager에 패널 연결

1. **Hierarchy**에서 `UIManager` 선택
2. **Inspector**에서:
   - **Clear Panel**: `ClearPanel` 드래그
   - **Game Over Panel**: `GameOverPanel` 드래그

---

### 9단계: GameManager에서 UI 호출 추가

#### 9-1. GameManager.cs 수정
1. `Assets/Scripts/Manager/GameManager.cs` 열기
2. **GameClear()** 메서드 수정:

```csharp
void GameClear()
{
    currentState = GameState.Cleared;
    Debug.Log("🎉 Game Clear!");

    // ===== 이 줄 주석 해제 =====
    if (UIManager.Instance != null)
    {
        UIManager.Instance.ShowClearPanel();
    }
    // =========================

    Time.timeScale = 0f;
}
```

3. **GameOver()** 메서드 수정:

```csharp
void GameOver()
{
    currentState = GameState.GameOver;
    Debug.Log("⏰ Time Over! Game Over!");

    // ===== 이 줄 주석 해제 =====
    if (UIManager.Instance != null)
    {
        UIManager.Instance.ShowGameOverPanel();
    }
    // =========================

    Time.timeScale = 0f;
}
```

4. **Ctrl + S** 저장

---

### 10단계: 최종 테스트

#### 10-1. 플레이 모드 실행
1. **▶ Play** 클릭
2. **Game View** 확인

#### 10-2. 확인 사항
- [x] 좌측 상단에 "📦 수집: 0/15" 표시되는가?
- [x] 타이머가 "⏰ 남은시간: 3:00"에서 감소하는가?
- [x] 패키지 수집 시 "수집: 1/15"로 증가하는가?
- [x] 15개 수집 시 클리어 패널이 나타나는가?
- [x] "다시 시작" 버튼 클릭 시 씬이 재시작되는가?
- [x] "종료" 버튼 클릭 시 게임이 종료되는가? (에디터는 플레이 모드 종료)

#### 10-3. 타이머 테스트
**빠른 테스트를 위해 시간 제한 줄이기**:
1. `GameManager` 선택
2. **Time Limit**: `10` (10초로 변경)
3. **Play** 눌러서 10초 후 게임오버 패널 확인
4. 테스트 완료 후 다시 `180`으로 복구

---

## ✅ Day 5-6 완료 체크리스트

- [x] GameManager 싱글톤 구현
- [x] 수집 개수 추적 기능
- [x] 타이머 시스템 (3분)
- [x] 클리어 조건 체크 (15개 수집)
- [x] Canvas 및 UI 생성
- [x] 점수 텍스트 표시 및 업데이트
- [x] 타이머 텍스트 표시 및 업데이트
- [x] 클리어 패널 생성
- [x] 게임오버 패널 생성
- [x] 다시 시작 버튼 기능
- [x] 종료 버튼 기능
- [x] Package 수집 시 GameManager 연동
- [x] 15개 수집 시 클리어 확인
- [x] 시간 초과 시 게임오버 확인

모든 항목 완료되면 **Day 7**로 진행하세요!

---

---

## 📅 Day 7: 환경 오브젝트 + 테스트

### ✅ 목표
- 트램펄린 구현 (점프대)
- 빌딩 재배치 및 레벨 디자인
- 전체 게임 플레이 테스트
- 밸런스 조정

---

### 1단계: 트램펄린 오브젝트 생성

#### 1-1. 기본 오브젝트
1. **Hierarchy** 우클릭 → **3D Object** → **Cylinder**
2. 이름: `Trampoline`
3. **Transform**:
   - Position: `(5, 0.5, 5)` ← 빌딩 사이
   - Rotation: `(0, 0, 0)`
   - Scale: `(2, 0.2, 2)` ← 얇고 넓게

#### 1-2. Material 적용
1. `Assets/Materials`에서 새 Material: `TrampolineMaterial`
2. **Albedo**: 초록색 (RGB 0, 255, 100)
3. `Trampoline` Cylinder에 적용

#### 1-3. Collider 확인
- Cylinder는 기본적으로 **Capsule Collider** 있음
- 그대로 유지 (Trigger 아님!)

---

### 2단계: Trampoline 스크립트 작성

#### 2-1. 스크립트 생성
1. `Assets/Scripts/Environment` 폴더
2. **C# Script**: `Trampoline`

#### 2-2. 스크립트 작성
```csharp
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float bounceForce = 15f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem bounceEffect;
    [SerializeField] private AudioClip bounceSound;

    void OnCollisionEnter(Collision collision)
    {
        // 플레이어가 트램펄린에 닿았는지 체크
        if (collision.gameObject.CompareTag("Player"))
        {
            Bounce(collision.gameObject);
        }
    }

    void Bounce(GameObject player)
    {
        // PlayerController 찾기
        PlayerController playerController = player.GetComponent<PlayerController>();

        if (playerController != null)
        {
            // PlayerController에 점프 강제 적용
            // (velocity를 직접 수정할 수 없으므로 공개 메서드 필요)
            Debug.Log("Trampoline Bounce!");

            // 이펙트
            if (bounceEffect != null)
            {
                bounceEffect.Play();
            }

            // 사운드
            if (bounceSound != null)
            {
                AudioSource.PlayClipAtPoint(bounceSound, transform.position);
            }
        }
    }
}
```

3. **Ctrl + S** 저장

#### 2-3. PlayerController에 트램펄린 지원 추가

**문제**: CharacterController는 velocity를 외부에서 수정하기 어려움

**해결책**: PlayerController에 공개 메서드 추가

1. `Assets/Scripts/Player/PlayerController.cs` 열기
2. **클래스 끝 부분**에 다음 메서드 추가:

```csharp
// 외부에서 점프 강제 적용 (트램펄린용)
public void ForceJump(float force)
{
    velocity.y = force;
}
```

3. **Ctrl + S** 저장

#### 2-4. Trampoline.cs 수정
1. `Trampoline.cs` 다시 열기
2. **Bounce() 메서드** 수정:

```csharp
void Bounce(GameObject player)
{
    PlayerController playerController = player.GetComponent<PlayerController>();

    if (playerController != null)
    {
        // ===== 이 줄 수정 =====
        playerController.ForceJump(bounceForce);
        Debug.Log("Trampoline Bounce!");
        // ======================

        // 이펙트
        if (bounceEffect != null)
        {
            bounceEffect.Play();
        }

        // 사운드
        if (bounceSound != null)
        {
            AudioSource.PlayClipAtPoint(bounceSound, transform.position);
        }
    }
}
```

3. **Ctrl + S** 저장

#### 2-5. 스크립트 적용
1. `Trampoline` 오브젝트 선택
2. **Add Component** → `Trampoline` 스크립트 추가
3. **Inspector**:
   - **Bounce Force**: `15` (높이 조절 가능)

---

### 3단계: 트램펄린 파티클 추가 (선택)

#### 3-1. 파티클 시스템
1. `Trampoline` 오브젝트 선택
2. **우클릭** → **Effects** → **Particle System**
3. 자식으로 생성됨, 이름: `BounceEffect`
4. **Particle System** 설정:
   - **Looping**: ❌ 체크 해제
   - **Play On Awake**: ❌ 체크 해제
   - **Duration**: `0.3`
   - **Start Lifetime**: `0.5`
   - **Start Speed**: `3`
   - **Start Size**: `0.3`
   - **Start Color**: 흰색
   - **Emission** → **Rate over Time**: `0`
   - **Emission** → **Bursts**: **+** 클릭
     - **Time**: `0`
     - **Count**: `20`

#### 3-2. Trampoline 스크립트에 연결
1. `Trampoline` 오브젝트 선택
2. **Inspector** → **Trampoline** 스크립트
3. **Bounce Effect**: `BounceEffect` (자식 오브젝트) 드래그

---

### 4단계: 트램펄린 추가 배치

#### 4-1. Prefab 생성
1. `Trampoline` 오브젝트를 `Assets/Prefabs` 폴더로 드래그
2. Prefab 생성 완료

#### 4-2. 추가 배치
1. **Prefabs** 폴더에서 `Trampoline` Prefab을 **Scene View**로 드래그
2. 빌딩 사이사이에 3~5개 배치:
   - 예시 위치:
     - `(5, 0.5, 5)`
     - `(-8, 0.5, -8)`
     - `(12, 0.5, -5)`
     - `(-5, 0.5, 10)`

---

### 5단계: 레벨 디자인 개선

#### 5-1. 빌딩 높이 다양화
1. 각 Building 오브젝트 선택
2. **Scale Y** 값을 다르게 조정:
   - `Building_01`: `(4, 8, 4)`
   - `Building_02`: `(5, 12, 5)`
   - `Building_03`: `(3, 6, 3)`
   - `Building_04`: `(4, 15, 4)` ← 가장 높게
   - `Building_05`: `(3, 9, 3)`

#### 5-2. 빌딩 옥상 플랫폼 추가 (선택)
1. **Hierarchy** → `Building_01` 선택
2. 우클릭 → **3D Object** → **Cube**
3. 이름: `Rooftop`
4. **Transform**:
   - Position: `(0, 5.5, 0)` ← 빌딩 꼭대기
   - Scale: `(1.1, 0.2, 1.1)` ← 얇은 바닥
5. Material: `GroundMaterial` 적용

6. 다른 빌딩에도 같은 방식으로 Rooftop 추가

#### 5-3. 스폰 포인트 재배치
1. **Hierarchy** → `SpawnPoints` 펼치기
2. 각 `SpawnPoint`를 빌딩 위쪽으로 이동:
   - 빌딩 근처 상공 (Y: 25~35)
   - 다양한 X, Z 위치

**팁**: 스폰 포인트를 Scene View에서 드래그하여 시각적으로 배치

---

### 6단계: 전체 플레이 테스트

#### 6-1. 테스트 항목
1. **▶ Play** 버튼 클릭
2. 다음 항목들을 순서대로 테스트:

**이동 시스템**:
- [ ] WASD로 부드럽게 이동되는가?
- [ ] Shift로 달리기가 작동하는가?
- [x] 점프 및 더블 점프가 자연스러운가?
- [x] 마우스로 카메라 회전이 잘 되는가?

**패키지 시스템**:
- [x] 게임 시작 시 3개가 하늘에서 떨어지는가?
- [x] 스폰 위치가 다양한가?
- [x] 자력 효과로 끌려오는가?
- [x] 수집 시 사라지는가?
- [x] 2초 후 재스폰되는가?

**트램펄린**:
- [ ] 트램펄린에 닿으면 높이 튀어오르는가?
- [ ] 파티클 효과가 나타나는가?

**UI**:
- [x] 수집 개수가 실시간으로 업데이트되는가?
- [x] 타이머가 정상적으로 감소하는가?
- [x] 30초 이하일 때 빨간색으로 변하는가?

**게임 종료**:
- [x] 15개 수집 시 클리어 패널이 뜨는가?
- [x] 시간 초과 시 게임오버 패널이 뜨는가?
- [x] "다시 시작" 버튼이 작동하는가?

---

### 7단계: 밸런스 조정

#### 7-1. 이동 속도 조정
**너무 느리다면**:
1. `Player` → **PlayerController**
2. **Move Speed**: `5` → `7`
3. **Sprint Multiplier**: `1.5` → `2`

**너무 빠르다면**:
- **Move Speed**: `5` → `4`

#### 7-2. 점프력 조정
**너무 낮다면**:
- **Jump Force**: `8` → `10`

**너무 높다면**:
- **Jump Force**: `8` → `6`

#### 7-3. 자력 효과 조정
**너무 강하다면**:
1. **Prefabs/Package** Prefab 열기
2. **Magnet Range**: `3` → `2`
3. **Magnet Speed**: `5` → `3`

**너무 약하다면**:
- **Magnet Range**: `3` → `4`
- **Magnet Speed**: `5` → `7`

#### 7-4. 타이머 조정
**너무 빡빡하다면**:
- `GameManager` → **Time Limit**: `180` → `240` (4분)

**너무 여유롭다면**:
- **Time Limit**: `180` → `150` (2분 30초)

#### 7-5. 트램펄린 높이 조정
**너무 높이 튀면**:
- `Trampoline` Prefab → **Bounce Force**: `15` → `12`

**너무 낮으면**:
- **Bounce Force**: `15` → `18`

---

### 8단계: 최종 정리

#### 8-1. Hierarchy 정리
1. 빈 오브젝트로 그룹화:
   - **Create Empty** → `--- MANAGERS ---`
   - `GameManager`, `UIManager`, `PackagePool`, `PackageSpawner` 를 이 아래로 이동

   - **Create Empty** → `--- ENVIRONMENT ---`
   - `Ground`, `Buildings`, `Trampolines`, `SpawnPoints` 이동

2. 최종 Hierarchy 구조:
```
MainScene
├─ --- MANAGERS ---
│  ├─ GameManager
│  ├─ UIManager
│  ├─ PackagePool
│  └─ PackageSpawner
├─ --- ENVIRONMENT ---
│  ├─ Ground
│  ├─ Building_01
│  ├─ Building_02
│  ├─ ...
│  ├─ Trampoline (여러 개)
│  └─ SpawnPoints
│     ├─ SpawnPoint_01
│     └─ ...
├─ Player
├─ Main Camera
├─ Directional Light
├─ UI_Canvas
│  ├─ ScorePanel
│  ├─ ClearPanel
│  └─ GameOverPanel
└─ EventSystem
```

#### 8-2. 씬 저장
1. **Ctrl + S** (Cmd + S)
2. **File** → **Save Project**

---

### 9단계: Git 커밋

```bash
git add .
git commit -m "Week 1 완료: 플레이어 이동, 패키지 수집, UI, 트램펄린 구현"
git push origin 브랜치명
```

---

## ✅ Day 7 완료 체크리스트

- [x] 트램펄린 오브젝트 생성
- [x] Trampoline 스크립트 구현
- [x] PlayerController에 ForceJump 메서드 추가
- [ ] 트램펄린 파티클 효과
- [x] 트램펄린 3~5개 배치
- [ ] 빌딩 높이 다양화
- [x] (선택) 빌딩 옥상 플랫폼 추가
- [ ] 스폰 포인트 재배치
- [ ] 전체 플레이 테스트 완료
- [x] 이동 속도 밸런스 조정
- [x] 점프력 밸런스 조정
- [x] 자력 효과 밸런스 조정
- [x] 타이머 밸런스 조정
- [x] Hierarchy 정리
- [x] 씬 저장
- [x] Git 커밋

---

## 🎉 Week 1 완료!

축하합니다! Week 1의 모든 핵심 시스템을 구현했습니다.

### 완성된 기능:
✅ 플레이어 이동 (WASD, 달리기, 점프, 더블 점프)
✅ 3인칭 카메라
✅ 패키지 랜덤 스폰 (하늘에서 낙하)
✅ 오브젝트 풀 시스템
✅ 자력 효과
✅ 수집 시스템
✅ 게임 매니저 (점수, 타이머, 클리어 조건)
✅ UI (점수, 타이머, 클리어 패널)
✅ 트램펄린

### 다음 단계: Week 2
Week 1 작업이 완료되면 알려주세요!
- 갈고리 시스템
- 애니메이션
- 선택 과제
- 폴리싱
- 빌드

을 진행합니다. Week 2 가이드는 Week 1 완료 후 생성됩니다! 🚀

---

**문서 버전**: 1.0
**최종 수정일**: 2025-11-14
