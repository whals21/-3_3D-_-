# 🎮 Week 2 상세 작업 가이드

> 이 문서는 Unity에서 Rooftop Runner 게임의 Week 2 작업을 단계별로 수행하는 방법을 상세히 안내합니다.

---

## 📅 Day 8-9: 갈고리 시스템

### ✅ 목표
- GrappleController 스크립트 구현
- 마우스 우클릭으로 갈고리 발사
- 레이캐스트로 건물 감지
- LineRenderer로 로프 시각화
- 플레이어를 갈고리 지점으로 끌어당기기
- 갈고리 포인트 배치 및 표시

---

### 1단계: GrappleController 스크립트 작성

#### 1-1. 스크립트 생성
1. **Project** → `Assets/Scripts/Player` 폴더
2. 우클릭 → **Create** → **C# Script**
3. 이름: `GrappleController`

#### 1-2. 스크립트 작성
```csharp
using UnityEngine;

public class GrappleController : MonoBehaviour
{
    [Header("Grapple Settings")]
    [SerializeField] private float grappleRange = 30f;
    [SerializeField] private float grappleSpeed = 20f;
    [SerializeField] private float grappleDuration = 1f;
    [SerializeField] private LayerMask grappleLayer;

    [Header("References")]
    [SerializeField] private LineRenderer ropeLine;
    [SerializeField] private Transform grappleOrigin; // 로프 시작 위치
    [SerializeField] private Camera playerCamera;

    // Grapple 상태
    private bool isGrappling = false;
    private Vector3 grapplePoint;
    private float grappleTimer = 0f;

    // Components
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // 카메라 자동 찾기
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        // LineRenderer 설정
        if (ropeLine != null)
        {
            ropeLine.enabled = false;
            ropeLine.positionCount = 2;
        }

        // Grapple Origin 자동 설정 (플레이어 중심)
        if (grappleOrigin == null)
        {
            grappleOrigin = transform;
        }
    }

    void Update()
    {
        HandleGrappleInput();

        if (isGrappling)
        {
            ExecuteGrapple();
        }
    }

    void HandleGrappleInput()
    {
        // 마우스 우클릭으로 갈고리 발사
        if (Input.GetMouseButtonDown(1) && !isGrappling)
        {
            StartGrapple();
        }

        // 갈고리 취소 (우클릭 떼기 또는 Space)
        if ((Input.GetMouseButtonUp(1) || Input.GetKeyDown(KeyCode.Space)) && isGrappling)
        {
            StopGrapple();
        }
    }

    void StartGrapple()
    {
        // 화면 중앙에서 레이캐스트 발사
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        // Grapple 가능한 오브젝트 감지
        if (Physics.Raycast(ray, out hit, grappleRange, grappleLayer))
        {
            grapplePoint = hit.point;
            isGrappling = true;
            grappleTimer = 0f;

            // LineRenderer 활성화
            if (ropeLine != null)
            {
                ropeLine.enabled = true;
            }

            Debug.Log("Grapple Start: " + hit.collider.name);
        }
        else
        {
            Debug.Log("갈고리 범위 밖이거나 감지 불가능한 오브젝트입니다.");
        }
    }

    void ExecuteGrapple()
    {
        grappleTimer += Time.deltaTime;

        // 플레이어를 갈고리 지점으로 당기기
        Vector3 direction = (grapplePoint - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, grapplePoint);

        // 가까워지면 멈춤
        if (distance > 2f)
        {
            controller.Move(direction * grappleSpeed * Time.deltaTime);
        }
        else
        {
            StopGrapple();
        }

        // 로프 시각화
        UpdateRopeLine();

        // 일정 시간 후 자동 종료
        if (grappleTimer >= grappleDuration)
        {
            StopGrapple();
        }
    }

    void UpdateRopeLine()
    {
        if (ropeLine != null && isGrappling)
        {
            ropeLine.SetPosition(0, grappleOrigin.position);
            ropeLine.SetPosition(1, grapplePoint);
        }
    }

    void StopGrapple()
    {
        isGrappling = false;
        grappleTimer = 0f;

        // LineRenderer 비활성화
        if (ropeLine != null)
        {
            ropeLine.enabled = false;
        }

        Debug.Log("Grapple Stop");
    }

    // Gizmo로 갈고리 범위 시각화
    void OnDrawGizmosSelected()
    {
        if (playerCamera == null) return;

        Gizmos.color = Color.green;
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Gizmos.DrawRay(ray.origin, ray.direction * grappleRange);
    }

    // 외부에서 갈고리 상태 확인용
    public bool IsGrappling()
    {
        return isGrappling;
    }
}
```

3. **Ctrl + S** 저장

---

### 2단계: Player에 GrappleController 추가

#### 2-1. 스크립트 추가
1. **Hierarchy**에서 `Player` 오브젝트 선택
2. **Inspector** → **Add Component** → `GrappleController` 검색 후 추가

#### 2-2. LineRenderer 추가
1. `Player` 선택 상태에서
2. **Add Component** → `Line Renderer` 검색 후 추가
3. **Line Renderer** 설정:
   - **Positions**: `2`
   - **Width**: `0.1` (얇은 로프)
   - **Materials**:
     - **Element 0**: `Default-Line` (기본 머티리얼)
   - **Color Gradient**:
     - 시작: 흰색
     - 끝: 회색

#### 2-3. GrappleController 설정
1. `Player` 선택 → **GrappleController** 컴포넌트 찾기
2. **Inspector** 설정:
   - **Grapple Range**: `30`
   - **Grapple Speed**: `20`
   - **Grapple Duration**: `1.5`
   - **Grapple Layer**:
     - 드롭다운 클릭 → **Everything** 체크 (또는 나중에 Building 레이어만)
   - **Rope Line**: `Player`의 **Line Renderer** 컴포넌트를 드래그
   - **Grapple Origin**: `Player` 오브젝트 자신을 드래그
   - **Player Camera**: `Main Camera`를 드래그

---

### 3단계: 건물에 Grapple 레이어 설정 (선택)

더 정확한 갈고리 감지를 위해 건물에만 반응하도록 설정합니다.

#### 3-1. 새 레이어 생성
1. Unity 상단 **Layers** 드롭다운 (Inspector 우측 상단)
2. **Edit Layers...**
3. **User Layer 8** 클릭
4. 이름: `Building` 입력 후 Enter

#### 3-2. 건물 오브젝트에 레이어 적용
1. **Hierarchy**에서 `Building_01` 선택
2. **Inspector** 상단 **Layer** 드롭다운 → **Building** 선택
3. "Change children as well?" 팝업 → **Yes, change children**
4. 다른 모든 Building 오브젝트에도 반복

#### 3-3. GrappleController 레이어 설정
1. `Player` 선택 → **GrappleController**
2. **Grapple Layer**:
   - 드롭다운 → **Nothing** 클릭 (전부 해제)
   - **Building**만 체크

---

### 4단계: 갈고리 포인트 표시 오브젝트 생성 (선택)

빌딩에 갈고리 가능 지점을 시각적으로 표시합니다.

#### 4-1. 갈고리 포인트 오브젝트
1. **Hierarchy** → `Building_01` 선택
2. 우클릭 → **3D Object** → **Sphere**
3. 이름: `GrapplePoint`
4. **Transform**:
   - Position: `(0, 4, 0)` ← 빌딩 중간 높이
   - Scale: `(0.5, 0.5, 0.5)` ← 작게

#### 4-2. Material 생성
1. `Assets/Materials` → 새 Material: `GrapplePointMaterial`
2. **Albedo**: 초록색 (RGB 0, 255, 100)
3. **Emission**: ✅ 체크
   - **Emission Color**: 초록색 (약간 빛나게)
4. `GrapplePoint` Sphere에 적용

#### 4-3. Collider 제거 (선택)
- GrapplePoint의 **Sphere Collider** 제거 (우클릭 → Remove Component)
- 이유: 시각적 표시용이므로 충돌 불필요

#### 4-4. 다른 빌딩에도 복제
1. `GrapplePoint`를 `Assets/Prefabs`로 드래그 (Prefab 생성)
2. 다른 Building들에도 자식으로 배치
3. 각 빌딩마다 2~3개씩 다양한 높이에 배치

---

### 5단계: 갈고리 테스트

#### 5-1. 플레이 모드 실행
1. **▶ Play** 버튼 클릭
2. **Game View** 확인

#### 5-2. 테스트 항목
- [x] 빌딩을 바라보고 **마우스 우클릭** → 갈고리 발사
- [x] 흰색 LineRenderer(로프)가 보이는가?
- [x] 플레이어가 빌딩 쪽으로 끌려가는가?
- [x] 가까워지면 자동으로 멈추는가?
- [x] 우클릭 떼거나 Space 누르면 갈고리 취소되는가?

#### 5-3. 문제 해결
**문제 1**: 갈고리가 발사되지 않음
- Console에 "갈고리 범위 밖..." 메시지 확인
- Grapple Range를 `50`으로 늘려보기
- Grapple Layer가 올바르게 설정되었는지 확인

**문제 2**: 로프가 안 보임
- Rope Line에 Line Renderer가 연결되었는지 확인
- Line Renderer의 Width를 `0.5`로 크게 해보기

**문제 3**: 플레이어가 안 끌려감
- Grapple Speed를 `30`으로 높여보기
- CharacterController가 제대로 작동하는지 확인

---

### 6단계: 갈고리 폴리싱 (선택)

#### 6-1. 로프 Material 개선
1. `Assets/Materials` → 새 Material: `RopeMaterial`
2. **Shader**: `Unlit/Color` 선택 (빛 영향 안받음)
3. **Color**: 갈색 또는 회색 (RGB 80, 60, 40)
4. `Player` → **Line Renderer** → **Materials** → **Element 0**에 `RopeMaterial` 드래그

#### 6-2. 로프 곡선 효과 (선택)
Line Renderer를 3개 이상의 점으로 만들어 곡선처럼 보이게 합니다.

**`GrappleController.cs` 수정**:
```csharp
// Start() 메서드에서
void Start()
{
    // ...
    if (ropeLine != null)
    {
        ropeLine.enabled = false;
        ropeLine.positionCount = 10; // 2 → 10으로 변경 (곡선)
    }
}

// UpdateRopeLine() 메서드 수정
void UpdateRopeLine()
{
    if (ropeLine != null && isGrappling)
    {
        int segments = ropeLine.positionCount;
        Vector3 startPos = grappleOrigin.position;
        Vector3 endPos = grapplePoint;

        for (int i = 0; i < segments; i++)
        {
            float t = i / (float)(segments - 1);
            Vector3 point = Vector3.Lerp(startPos, endPos, t);

            // 중간에 아래로 처지는 효과 (포물선)
            float sag = Mathf.Sin(t * Mathf.PI) * 2f; // 2f = 처짐 정도
            point.y -= sag;

            ropeLine.SetPosition(i, point);
        }
    }
}
```

---

## ✅ Day 8-9 완료 체크리스트

- [x] GrappleController 스크립트 작성
- [x] Player에 GrappleController 추가
- [x] Line Renderer 추가 및 설정
- [x] 마우스 우클릭으로 갈고리 발사 확인
- [x] 로프 시각화 확인
- [x] 플레이어가 갈고리 지점으로 이동 확인
- [x] (선택) Building 레이어 생성 및 적용
- [ ] (선택) 갈고리 포인트 표시 오브젝트 배치
- [x] (선택) 로프 Material 개선
- [x] (선택) 로프 곡선 효과 추가

---

---

## 📅 Day 10-11: 애니메이션 + 폴리싱

### ✅ 목표
- Mixamo에서 캐릭터 및 애니메이션 다운로드
- Unity로 임포트
- Animator Controller 구성
- 애니메이션 상태 전환 구현
- 시각 효과 개선 (파티클, 조명)

---

### 1단계: Mixamo에서 캐릭터 다운로드

#### 1-1. Mixamo 웹사이트 접속
1. 브라우저에서 [mixamo.com](https://www.mixamo.com) 접속
2. **Sign In** (Adobe 계정 필요 - 무료)
3. 계정이 없다면 **Sign Up** (무료)

#### 1-2. 캐릭터 선택
1. 좌측 **Characters** 탭 클릭
2. 검색창에서 원하는 캐릭터 검색:
   - 추천: "X Bot", "Y Bot" (간단한 로봇)
   - 또는: "Hoodie", "Runner" (사람)
3. 마음에 드는 캐릭터 클릭

#### 1-3. 캐릭터 다운로드
1. 우측 상단 **Download** 버튼 클릭
2. 설정:
   - **Format**: `FBX for Unity (.fbx)`
   - **Pose**: `T-Pose`
3. **Download** 클릭
4. 파일 이름 예시: `XBot.fbx`

---

### 2단계: Mixamo에서 애니메이션 다운로드

#### 2-1. Idle 애니메이션
1. Mixamo 좌측 **Animations** 탭
2. 검색: `Idle`
3. 마음에 드는 Idle 애니메이션 클릭 (예: "Idle")
4. 우측에서 애니메이션 미리보기 확인
5. **Download** 버튼 클릭
6. 설정:
   - **Format**: `FBX for Unity (.fbx)`
   - **Skin**: ❌ **Without Skin** (애니메이션만)
   - **Frame Rate**: `30`
   - **Keyframe Reduction**: `none`
7. **Download** → 파일 이름: `Idle.fbx`

#### 2-2. Running 애니메이션
1. 검색: `Running`
2. "Running" 또는 "Run" 애니메이션 선택
3. 같은 방식으로 다운로드: `Running.fbx`

#### 2-3. Jumping 애니메이션
1. 검색: `Jump`
2. "Jumping" 선택
3. 다운로드: `Jumping.fbx`

#### 2-4. (선택) Falling 애니메이션
1. 검색: `Falling`
2. "Falling Idle" 선택
3. 다운로드: `Falling.fbx`

---

### 3단계: Unity로 임포트

#### 3-1. 폴더 생성
1. **Project** → `Assets` → 새 폴더: `Models`
2. `Assets` → 새 폴더: `Animations` (이미 있으면 스킵)

#### 3-2. 캐릭터 모델 임포트
1. 다운로드한 `XBot.fbx` 파일을 찾기
2. Unity 에디터의 **Project** 패널 → `Assets/Models` 폴더로 드래그
3. 임포트 완료까지 대기 (몇 초)

#### 3-3. 애니메이션 파일 임포트
1. `Idle.fbx`, `Running.fbx`, `Jumping.fbx` (+ Falling.fbx)
2. 모두 `Assets/Animations` 폴더로 드래그
3. 임포트 완료 대기

---

### 4단계: 캐릭터 모델 설정

#### 4-1. 캐릭터 FBX 설정
1. **Project**에서 `XBot.fbx` 클릭
2. **Inspector** → **Rig** 탭 클릭
3. 설정:
   - **Animation Type**: `Humanoid`
   - **Avatar Definition**: `Create From This Model`
4. **Apply** 버튼 클릭

#### 4-2. 애니메이션 FBX 설정
각 애니메이션 파일에 대해 반복:

1. `Idle.fbx` 클릭
2. **Inspector** → **Rig** 탭
3. 설정:
   - **Animation Type**: `Humanoid`
   - **Avatar Definition**: `Copy From Other Avatar`
   - **Source**: `XBot.fbx`의 Avatar 선택 (드롭다운에서)
4. **Apply** 클릭

5. **Animation** 탭 클릭
6. **Clips** 섹션에서 클립 선택 (보통 1개)
7. 클립 이름을 `Idle`로 변경
8. **Loop Time**: ✅ 체크 (Idle은 반복)
9. **Apply** 클릭

**Running.fbx, Jumping.fbx도 같은 방식**:
- Running: Loop Time ✅
- Jumping: Loop Time ❌ (점프는 반복 안함)
- Falling: Loop Time ✅

---

### 5단계: 플레이어에 캐릭터 모델 적용

#### 5-1. 기존 Capsule 숨기기
1. **Hierarchy** → `Player` 선택
2. **Inspector**에서 **Mesh Renderer** 찾기
3. 좌측 체크박스 해제 (비활성화)

#### 5-2. 캐릭터 모델 추가
1. **Project** → `Assets/Models/XBot.fbx` 펼치기 (▶ 클릭)
2. 하위에 있는 캐릭터 모델 (보통 `XBot` 또는 첫 번째 아이템)
3. `Player` 오브젝트로 드래그 (자식으로 추가)

#### 5-3. 캐릭터 모델 Transform 조정
1. `Player` 하위의 `XBot` 선택
2. **Transform**:
   - Position: `(0, -1, 0)` ← 발이 바닥에 닿도록
   - Rotation: `(0, 0, 0)`
   - Scale: `(1, 1, 1)`

---

### 6단계: Animator Controller 생성

#### 6-1. Animator Controller 생성
1. **Project** → `Assets/Animations` 폴더
2. 우클릭 → **Create** → **Animator Controller**
3. 이름: `PlayerAnimator`

#### 6-2. Player에 Animator 추가
1. **Hierarchy** → `Player` 선택
2. **Inspector** → **Add Component** → `Animator`
3. **Animator** 컴포넌트 설정:
   - **Controller**: `PlayerAnimator` 드래그
   - **Avatar**: `XBot Avatar` 선택 (드롭다운)
   - **Apply Root Motion**: ❌ 체크 해제 (중요!)

---

### 7단계: Animator Controller 구성

#### 7-1. Animator 창 열기
1. **Project**에서 `PlayerAnimator` 더블클릭
2. **Animator** 창이 열림 (탭으로 추가됨)

#### 7-2. 애니메이션 State 추가
1. **Animator** 창에서 빈 공간 우클릭
2. **Create State** → **Empty**
3. 이름: `Idle`
4. State 클릭 → **Inspector**에서:
   - **Motion**: `Idle` 애니메이션 클립 드래그

5. 같은 방식으로 State 추가:
   - `Run` State → Motion: `Running`
   - `Jump` State → Motion: `Jumping`
   - (선택) `Fall` State → Motion: `Falling`

#### 7-3. 기본 State 설정
1. `Idle` State 우클릭
2. **Set as Layer Default State** 선택
3. `Idle` State가 주황색으로 변함

#### 7-4. Parameters 추가
1. **Animator** 창 좌측 **Parameters** 탭
2. **+** 버튼 클릭
3. **Float** 선택 → 이름: `Speed`
4. **+** 클릭 → **Bool** 선택 → 이름: `IsJumping`
5. **+** 클릭 → **Bool** 선택 → 이름: `IsFalling`

#### 7-5. Transition 생성

**Idle → Run**:
1. `Idle` State 우클릭 → **Make Transition**
2. `Run` State로 화살표 드래그
3. 생성된 화살표(Transition) 클릭
4. **Inspector**:
   - **Has Exit Time**: ❌ 체크 해제
   - **Transition Duration**: `0.1`
   - **Conditions**: **+** 클릭
     - `Speed` `Greater` `0.1`

**Run → Idle**:
1. `Run` → `Idle` Transition 생성
2. **Inspector**:
   - **Has Exit Time**: ❌
   - **Transition Duration**: `0.1`
   - **Conditions**: `Speed` `Less` `0.1`

**Any State → Jump**:
1. **Any State** (왼쪽 위 회색 박스) 우클릭 → **Make Transition**
2. `Jump` State로 연결
3. **Inspector**:
   - **Has Exit Time**: ❌
   - **Transition Duration**: `0.1`
   - **Conditions**: `IsJumping` `true`

**Jump → Idle** (점프 끝):
1. `Jump` → `Idle` Transition 생성
2. **Inspector**:
   - **Has Exit Time**: ✅ 체크 (애니메이션 끝나면 자동 전환)
   - **Exit Time**: `0.9`

---

### 8단계: PlayerAnimationController 스크립트 작성

#### 8-1. 스크립트 생성
1. `Assets/Scripts/Player` 폴더
2. **C# Script**: `PlayerAnimationController`

#### 8-2. 스크립트 작성
```csharp
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
```

3. **Ctrl + S** 저장

#### 8-3. 스크립트 적용
1. **Hierarchy** → `Player` 선택
2. **Add Component** → `PlayerAnimationController`
3. **Inspector**:
   - **Animator**: `Player`의 Animator 컴포넌트 드래그
   - **Player Controller**: `Player`의 PlayerController 컴포넌트 드래그

---

### 9단계: 애니메이션 테스트

#### 9-1. 플레이 모드 실행
1. **▶ Play**
2. **Game View** 확인

#### 9-2. 테스트 항목
- [x] 정지 시 Idle 애니메이션 재생
- [x] WASD로 이동 시 Run 애니메이션 재생
- [x] 점프 시 Jump 애니메이션 재생
- [x] 착지 시 다시 Idle/Run으로 전환

#### 9-3. 문제 해결
**문제 1**: 애니메이션이 안 나옴
- Animator Controller가 Player에 연결되었는지 확인
- Avatar가 설정되었는지 확인
- Apply Root Motion이 꺼져있는지 확인

**문제 2**: 캐릭터가 이상하게 움직임
- Apply Root Motion ❌ 확인
- 캐릭터 모델의 Position Y를 -1로 조정

**문제 3**: 애니메이션 전환이 안됨
- Animator 창에서 Parameters 확인
- PlayerAnimationController가 제대로 추가되었는지 확인

---

### 10단계: 시각 효과 개선

#### 10-1. 조명 추가 (Rim Light)
1. **Hierarchy** → `Player` 선택
2. 우클릭 → **Light** → **Point Light**
3. 이름: `PlayerLight`
4. **Transform**:
   - Position: `(0, 1, 0)`
5. **Light** 설정:
   - **Color**: 밝은 파란색
   - **Range**: `5`
   - **Intensity**: `2`

#### 10-2. 착지 파티클 추가 (선택)
1. `Player` 하위에 **Particle System** 생성
2. 이름: `LandingEffect`
3. 설정:
   - **Looping**: ❌
   - **Play On Awake**: ❌
   - **Duration**: `0.3`
   - **Start Color**: 흰색/회색
   - **Shape**: Circle

나중에 PlayerController에서 착지 시 `landingEffect.Play()` 호출

---

## ✅ Day 10-11 완료 체크리스트

- [x] Mixamo에서 캐릭터 다운로드
- [x] Mixamo에서 애니메이션 3종 다운로드 (Idle/Run/Jump)
- [x] Unity로 캐릭터 및 애니메이션 임포트
- [x] 캐릭터 Rig을 Humanoid로 설정
- [x] 애니메이션 Avatar 복사 및 설정
- [x] Player에 캐릭터 모델 추가
- [x] Animator Controller 생성
- [x] Animator State 및 Transition 구성
- [x] PlayerAnimationController 스크립트 작성
- [x] 애니메이션 정상 작동 확인
- [x] (선택) 조명 효과 추가
- [ ] (선택) 파티클 효과 추가

---

---

## 📅 Day 12-13: 선택 과제 구현

### ✅ 목표
선택 과제 중 최소 2~3개 구현:
- ① 자력 효과 (이미 구현됨)
- ② 최고 점수 저장
- ③ 방향 화살표 표시
- ④ 하늘 색상 변화
- ⑤ ScriptableObject 활용

---

### 선택 과제 ①: 자력 효과 (이미 완료)

Week 1에서 이미 구현되었습니다! ✅
- Package.cs의 `MagnetEffect()` 메서드
- 3m 이내 접근 시 플레이어 방향으로 끌려옴

---

### 선택 과제 ②: 최고 점수 저장

#### 구현 방법
**`Assets/Scripts/Manager/GameManager.cs` 수정**:

```csharp
// 클래스 상단에 변수 추가
private float bestTime = 999f;
private float gameTime = 0f;

void Start()
{
    remainingTime = timeLimit;
    currentState = GameState.Playing;
    gameTime = 0f;

    // ===== 최고 기록 불러오기 =====
    bestTime = PlayerPrefs.GetFloat("BestTime", 999f);
    Debug.Log($"현재 최고 기록: {bestTime:F2}초");
    // ===========================

    // ... 기존 코드
}

void Update()
{
    if (currentState != GameState.Playing) return;

    UpdateTimer();

    // ===== 게임 시간 계산 =====
    gameTime += Time.deltaTime;
    // ========================
}

void GameClear()
{
    currentState = GameState.Cleared;

    // ===== 최고 기록 저장 =====
    float clearTime = timeLimit - remainingTime; // 걸린 시간

    if (clearTime < bestTime)
    {
        bestTime = clearTime;
        PlayerPrefs.SetFloat("BestTime", bestTime);
        PlayerPrefs.Save();
        Debug.Log($"🎉 신기록! {bestTime:F2}초");
    }
    // ========================

    if (UIManager.Instance != null)
    {
        UIManager.Instance.ShowClearPanel(clearTime, bestTime); // 파라미터 추가
    }

    Time.timeScale = 0f;
}

// 외부에서 접근 가능하도록 Property 추가
public float BestTime => bestTime;
public float GameTime => gameTime;
```

**`Assets/Scripts/UI/UIManager.cs` 수정**:

```csharp
[Header("Panels")]
[SerializeField] private GameObject clearPanel;
[SerializeField] private TextMeshProUGUI clearTimeText; // 추가
[SerializeField] private TextMeshProUGUI bestTimeText;  // 추가

public void ShowClearPanel(float clearTime, float bestTime)
{
    if (clearPanel != null)
    {
        clearPanel.SetActive(true);

        // 클리어 시간 표시
        if (clearTimeText != null)
        {
            int minutes = Mathf.FloorToInt(clearTime / 60);
            int seconds = Mathf.FloorToInt(clearTime % 60);
            clearTimeText.text = $"클리어 타임: {minutes}:{seconds:00}";
        }

        // 최고 기록 표시
        if (bestTimeText != null)
        {
            int minutes = Mathf.FloorToInt(bestTime / 60);
            int seconds = Mathf.FloorToInt(bestTime % 60);
            bestTimeText.text = $"최고 기록: {minutes}:{seconds:00}";
        }
    }
}
```

**UI 수정**:
1. `ClearPanel` → `InfoText` 하위에 텍스트 2개 추가
2. `ClearTimeText`: "클리어 타임: 1:23"
3. `BestTimeText`: "최고 기록: 1:15"
4. UIManager에 연결

---

### 선택 과제 ③: 방향 화살표 표시

#### 구현 방법

**스크립트 생성**: `Assets/Scripts/UI/DirectionIndicator.cs`

```csharp
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DirectionIndicator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform arrowImage;
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private Transform player;
    [SerializeField] private Camera playerCamera;

    [Header("Settings")]
    [SerializeField] private float edgeMargin = 50f;

    private Transform nearestPackage;

    void Update()
    {
        FindNearestPackage();

        if (nearestPackage != null)
        {
            UpdateArrowPosition();
        }
        else
        {
            HideArrow();
        }
    }

    void FindNearestPackage()
    {
        GameObject[] packages = GameObject.FindGameObjectsWithTag("Package");
        float minDistance = Mathf.Infinity;
        Transform nearest = null;

        foreach (GameObject pkg in packages)
        {
            if (!pkg.activeInHierarchy) continue;

            float distance = Vector3.Distance(player.position, pkg.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = pkg.transform;
            }
        }

        nearestPackage = nearest;
    }

    void UpdateArrowPosition()
    {
        Vector3 screenPos = playerCamera.WorldToScreenPoint(nearestPackage.position);

        // 화면 밖이면 화살표 표시
        if (IsOffScreen(screenPos))
        {
            ShowArrow(screenPos);
        }
        else
        {
            HideArrow();
        }
    }

    bool IsOffScreen(Vector3 screenPos)
    {
        return screenPos.z < 0 ||
               screenPos.x < 0 || screenPos.x > Screen.width ||
               screenPos.y < 0 || screenPos.y > Screen.height;
    }

    void ShowArrow(Vector3 screenPos)
    {
        arrowImage.gameObject.SetActive(true);

        // 화면 가장자리로 클램프
        screenPos.x = Mathf.Clamp(screenPos.x, edgeMargin, Screen.width - edgeMargin);
        screenPos.y = Mathf.Clamp(screenPos.y, edgeMargin, Screen.height - edgeMargin);

        arrowImage.position = screenPos;

        // 화살표 회전 (패키지 방향)
        Vector3 direction = nearestPackage.position - player.position;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        arrowImage.rotation = Quaternion.Euler(0, 0, -angle);

        // 거리 표시
        if (distanceText != null)
        {
            float distance = Vector3.Distance(player.position, nearestPackage.position);
            distanceText.text = $"{distance:F0}m";
        }
    }

    void HideArrow()
    {
        arrowImage.gameObject.SetActive(false);
    }
}
```

**UI 생성**:
1. `UI_Canvas` 하위에 **Image** 생성: `DirectionArrow`
2. 화살표 이미지 또는 삼각형 모양
3. 색상: 오렌지색
4. 하위에 **TextMeshProUGUI**: `DistanceText`
5. DirectionIndicator 스크립트 추가 및 연결

---

### 선택 과제 ④: 하늘 색상 변화

#### 구현 방법

**`GameManager.cs`에 메서드 추가**:

```csharp
[Header("Sky Colors")]
[SerializeField] private Color morningColor = new Color(0.5f, 0.7f, 1f); // 파란색
[SerializeField] private Color eveningColor = new Color(1f, 0.5f, 0.2f); // 주황색
[SerializeField] private Light directionalLight;

void Start()
{
    // ... 기존 코드

    // Directional Light 자동 찾기
    if (directionalLight == null)
    {
        directionalLight = FindObjectOfType<Light>();
    }
}

public void CollectPackage()
{
    // ... 기존 코드

    UpdateSkyColor(); // 추가

    CheckClearCondition();
}

void UpdateSkyColor()
{
    float progress = (float)collectedCount / targetPackageCount;

    // 아침 → 저녁으로 색상 변화
    Color skyColor = Color.Lerp(morningColor, eveningColor, progress);

    // Ambient Light 색상 변경
    RenderSettings.ambientLight = skyColor;

    // Directional Light 색상 변경
    if (directionalLight != null)
    {
        directionalLight.color = skyColor;
    }
}
```

**Inspector 설정**:
1. `GameManager` 선택
2. **Morning Color**: 파란색 (RGB 130, 180, 255)
3. **Evening Color**: 주황색 (RGB 255, 130, 50)
4. **Directional Light**: Hierarchy의 `Directional Light` 드래그

---

### 선택 과제 ⑤: ScriptableObject 활용

#### 구현 방법

**`Assets/Scripts/Item/PackageData.cs` 생성**:

```csharp
using UnityEngine;

[CreateAssetMenu(fileName = "PackageData", menuName = "Game/Package Data")]
public class PackageData : ScriptableObject
{
    [Header("Package Info")]
    public string packageName = "일반 배송";
    public int scoreValue = 10;
    public Color packageColor = Color.yellow;

    [Header("Physics")]
    public float weight = 1f;

    [Header("Visual")]
    public GameObject visualPrefab;
}
```

**PackageData 에셋 생성**:
1. **Project** → `Assets` → 새 폴더: `Data`
2. `Data` 폴더에서 우클릭
3. **Create** → **Game** → **Package Data**
4. 이름: `NormalPackage`
5. **Inspector**:
   - Package Name: "일반 배송"
   - Score Value: `10`
   - Package Color: 노란색

6. 같은 방식으로 추가 생성:
   - `ExpressPackage`: "특급 배송", 점수 `20`, 빨간색
   - `VIPPackage`: "VIP 배송", 점수 `50`, 금색

**Package.cs 수정하여 ScriptableObject 활용** (선택적 고급 작업)

---

## ✅ Day 12-13 완료 체크리스트

- [ ] ① 자력 효과 (Week 1에서 완료)
- [ ] ② 최고 점수 저장 기능 구현
- [ ] ② PlayerPrefs로 기록 저장/불러오기
- [ ] ② UI에 최고 기록 표시
- [ ] ③ 방향 화살표 UI 생성
- [ ] ③ DirectionIndicator 스크립트 구현
- [ ] ③ 가까운 패키지 방향 표시
- [ ] ④ 하늘 색상 변화 구현
- [ ] ④ 수집 개수에 따라 조명 색상 변경
- [ ] ⑤ ScriptableObject 생성 및 데이터 관리

최소 2~3개 완료하면 충분합니다!

---

---

## 📅 Day 14: 빌드 + 시연 영상

### ✅ 목표
- Windows 또는 Mac 빌드 생성
- 30~60초 시연 영상 녹화
- README.md 작성
- 최종 Git 커밋 및 푸시

---

### 1단계: 빌드 설정

#### 1-1. Build Settings 열기
1. Unity 상단 메뉴 **File** → **Build Settings...**
2. Build Settings 창이 열림

#### 1-2. 씬 추가
1. **Scenes In Build** 섹션 확인
2. `MainScene`이 없다면:
   - **Add Open Scenes** 버튼 클릭
   - 또는 **Project**에서 MainScene을 드래그

#### 1-3. 플랫폼 선택
**Windows**:
1. **Platform** 목록에서 **PC, Mac & Linux Standalone** 선택
2. **Target Platform**: `Windows`
3. **Architecture**: `x86_64` (64-bit)

**Mac**:
1. **Platform**: **PC, Mac & Linux Standalone**
2. **Target Platform**: `macOS`

선택 후 **Switch Platform** 클릭 (시간 소요)

---

### 2단계: Player Settings 설정

#### 2-1. Player Settings 열기
1. Build Settings 창에서 **Player Settings...** 버튼 클릭
2. Inspector에 Player Settings 표시

#### 2-2. 기본 설정
1. **Company Name**: 본인 이름 또는 팀 이름
2. **Product Name**: `Rooftop Runner`
3. **Version**: `1.0`
4. **Default Icon**: 아이콘 이미지 (선택사항)

#### 2-3. 해상도 설정
1. **Resolution and Presentation** 섹션 펼치기
2. **Fullscreen Mode**: `Windowed` 또는 `Fullscreen Window`
3. **Default Screen Width**: `1920`
4. **Default Screen Height**: `1080`
5. **Resizable Window**: ✅ 체크

---

### 3단계: 빌드 실행

#### 3-1. 빌드 시작
1. **Build Settings** 창으로 돌아가기
2. **Build** 버튼 클릭
3. 저장 위치 선택:
   - 프로젝트 폴더 밖에 새 폴더 생성: `Builds`
   - 파일 이름: `RooftopRunner` (Windows는 .exe 자동 추가)
4. **저장** 클릭

#### 3-2. 빌드 대기
- 빌드 진행 상태 표시줄 확인
- 5~10분 소요 (프로젝트 크기에 따라)

#### 3-3. 빌드 완료 확인
1. 빌드 완료 시 폴더가 자동으로 열림
2. 파일 확인:
   - **Windows**: `RooftopRunner.exe` + `RooftopRunner_Data` 폴더
   - **Mac**: `RooftopRunner.app`

---

### 4단계: 빌드 테스트

#### 4-1. 실행 테스트
1. `RooftopRunner.exe` (또는 .app) 더블클릭
2. 게임이 정상적으로 실행되는지 확인

#### 4-2. 테스트 항목
- [ ] 게임 시작 정상
- [ ] 플레이어 이동 작동
- [ ] 점프 및 더블 점프 작동
- [ ] 갈고리 작동
- [ ] 패키지 수집 작동
- [ ] UI 정상 표시
- [ ] 15개 수집 시 클리어 패널
- [ ] 다시 시작 버튼 작동

---

### 5단계: 시연 영상 녹화

#### 5-1. 녹화 도구 준비
**Windows**:
- **OBS Studio** (무료, 추천)
- **Xbox Game Bar** (Windows 내장, Win + G)

**Mac**:
- **QuickTime Player** (Mac 내장)
- **OBS Studio**

#### 5-2. 녹화 내용 (30~60초)
다음 순서로 녹화:

1. **게임 시작** (3초)
   - 타이틀 또는 게임 화면

2. **플레이어 조작 시연** (10초)
   - WASD 이동
   - 점프 및 더블 점프
   - 트램펄린 사용
   - 갈고리 사용 (건물 이동)

3. **패키지 수집** (15초)
   - 패키지가 하늘에서 떨어지는 장면
   - 자력 효과 (가까이 가면 끌려옴)
   - 수집 시 사라지는 장면
   - UI 점수 증가 확인

4. **랜덤 스폰 확인** (5초)
   - 2초 후 다른 위치에서 재스폰되는 장면

5. **추가 기능** (10초)
   - 방향 화살표 (선택 과제)
   - 하늘 색상 변화
   - 기타 구현한 기능

6. **클리어 달성** (5초)
   - 15개 수집 완료
   - 클리어 패널 표시

#### 5-3. 영상 편집 (선택)
- 불필요한 부분 자르기
- 자막 추가 ("이동", "점프", "수집" 등)
- 배경 음악 (저작권 주의)

#### 5-4. 영상 저장
- 파일 형식: MP4 (권장)
- 해상도: 1080p 이상
- 파일명: `RooftopRunner_Demo.mp4`

---

### 6단계: README.md 작성

#### 6-1. README 파일 생성
프로젝트 루트 폴더에 `README.md` 파일 생성 (이미 있으면 수정)

#### 6-2. README 내용 템플릿
```markdown
# 🏃 Rooftop Runner

3D 파쿠르 아케이드 스타일의 아이템 수집 게임

![게임 스크린샷](스크린샷.png)

## 📖 게임 소개

도시 옥상을 뛰어다니며 하늘에서 떨어지는 패키지를 수집하는 배달원이 되어보세요!
갈고리를 이용해 건물을 이동하고, 제한 시간 내에 15개의 패키지를 모두 수집하면 승리합니다.

## 🎮 조작법

- **W, A, S, D**: 이동
- **Left Shift**: 달리기
- **Space**: 점프
- **Space (공중에서)**: 더블 점프
- **마우스 우클릭**: 갈고리 발사
- **마우스 이동**: 카메라 회전

## ✨ 주요 기능

### 필수 기능
- ✅ 3인칭 플레이어 이동 (WASD, 점프, 더블 점프)
- ✅ 갈고리 시스템 (건물 이동)
- ✅ 패키지 랜덤 스폰 (하늘에서 낙하)
- ✅ 오브젝트 풀 시스템
- ✅ 자력 효과 (패키지 자동 끌림)
- ✅ 애니메이션 (Idle, Run, Jump)
- ✅ UI (점수, 타이머, 클리어 패널)
- ✅ 시간 제한 (3분)
- ✅ 트램펄린 점프대

### 선택 기능
- ✅ 자력 효과
- ✅ 최고 점수 저장
- ✅ 방향 화살표 표시
- ✅ 하늘 색상 변화

## 🛠️ 개발 환경

- **엔진**: Unity 2021.3 LTS
- **언어**: C#
- **그래픽**: URP (Universal Render Pipeline)
- **3D 모델**: Mixamo

## 📦 빌드 정보

- **플랫폼**: Windows / Mac
- **버전**: 1.0
- **빌드 날짜**: 2025-11-14

## 🎥 시연 영상

[YouTube 링크 또는 영상 파일 경로]

## 📂 프로젝트 구조

```
RooftopRunner/
├── Assets/
│   ├── Scripts/
│   │   ├── Player/
│   │   ├── Item/
│   │   ├── Manager/
│   │   └── UI/
│   ├── Prefabs/
│   ├── Materials/
│   ├── Animations/
│   └── Scenes/
├── Docs/
│   ├── PRD.md
│   ├── Week1-Guide.md
│   ├── Week2-Guide.md
│   └── Troubleshooting.md
└── README.md
```

## 🚀 실행 방법

1. `Builds` 폴더의 실행 파일 다운로드
2. `RooftopRunner.exe` (또는 .app) 실행
3. 게임 시작!

## 📝 개발 일지

- **Week 1**: 플레이어 이동, 패키지 시스템, UI, 트램펄린
- **Week 2**: 갈고리 시스템, 애니메이션, 선택 과제, 빌드

## 👤 개발자

- **이름**: [본인 이름]
- **GitHub**: [GitHub 링크]

## 📄 라이센스

이 프로젝트는 학습 목적으로 제작되었습니다.
```

---

### 7단계: 최종 Git 커밋

#### 7-1. 파일 정리
1. 불필요한 파일 삭제
2. `.gitignore` 확인 (Library, Temp, Builds 제외)

#### 7-2. 최종 커밋
```bash
git add .
git commit -m "Week 2 완료: 갈고리 시스템, 애니메이션, 선택 과제, 빌드"
git push origin 브랜치명
```

#### 7-3. 태그 생성 (선택)
```bash
git tag -a v1.0 -m "Rooftop Runner v1.0 Release"
git push origin v1.0
```

---

## ✅ Day 14 완료 체크리스트

- [ ] Build Settings 설정
- [ ] Player Settings 설정
- [ ] Windows 또는 Mac 빌드 생성
- [ ] 빌드 파일 테스트
- [ ] 시연 영상 녹화 (30~60초)
- [ ] 영상에 모든 필수 기능 포함
- [ ] README.md 작성
- [ ] 스크린샷 추가 (선택)
- [ ] 최종 Git 커밋
- [ ] (선택) Git 태그 생성

---

## 🎉 프로젝트 완료!

축하합니다! Rooftop Runner 프로젝트를 완성했습니다!

### 제출물 확인
- ✅ 빌드 파일 (RooftopRunner.exe + _Data 폴더)
- ✅ 시연 영상 (30~60초, MP4)
- ✅ Git 저장소 (커밋 히스토리)
- ✅ README.md (게임 설명, 조작법)
- ✅ 문서 (PRD, Week1/2 Guide, Troubleshooting)

### 다음 단계
- 포트폴리오에 추가
- 친구들에게 플레이 테스트 요청
- 피드백 받아서 개선
- 추가 기능 구현 (레벨 디자인, 사운드 등)

---

**문서 버전**: 1.0
**최종 수정일**: 2025-11-14
**작성자**: Claude AI Assistant
