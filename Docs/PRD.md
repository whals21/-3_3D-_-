# 🏃 Rooftop Runner - PRD (Product Requirements Document)

## 📋 프로젝트 개요

### 게임 컨셉
**장르**: 3D 파쿠르 아케이드 / 아이템 수집
**플랫폼**: PC (Windows/Mac)
**타겟 플레이 타임**: 3~5분/라운드
**핵심 재미 요소**: 역동적인 이동 + 타이밍 기반 수집 + 시원한 액션

### 게임 목표
공중에서 떨어지는 패키지들을 제한 시간 내에 모두 수집하여 배달 완료

---

## 🎮 핵심 게임 메커니즘

### 1. 이동 시스템
| 기능 | 입력 | 설명 |
|------|------|------|
| 기본 이동 | WASD | CharacterController 기반 이동 |
| 달리기 | Shift 유지 | 이동 속도 1.5배 증가 |
| 점프 | Space | 기본 점프 (지면 체크 필수) |
| 더블 점프 | Space 2회 | 공중에서 1회 추가 점프 가능 |
| 갈고리 발사 | 마우스 우클릭 | 조준 지점으로 스윙/끌어당김 |

### 2. 패키지 스폰 시스템
- **스폰 방식**: 하늘 높은 곳에서 자유낙하 (Rigidbody + Gravity)
- **스폰 타이밍**:
  - 게임 시작 시 3개 동시 스폰
  - 수집 시 2초 후 새로운 패키지 1개 스폰
- **스폰 위치**: 맵 내 5~8개의 스폰 포인트 중 랜덤 선택
- **목표 개수**: 총 15개 (수집 시 클리어)

### 3. 수집 메커니즘
- **트리거 충돌**: 플레이어가 패키지와 접촉 시 자동 수집
- **자력 효과** (선택 과제 ①): 3m 이내 접근 시 플레이어 방향으로 끌려옴
- **피드백**: 수집 사운드 + 파티클 효과 + UI 갱신

---

## 🛠️ 기술 명세서

### 개발 환경
- **엔진**: Unity 2021.3 LTS 이상
- **그래픽스**: URP (Universal Render Pipeline) 권장
- **스크립트**: C#
- **버전 관리**: Git

### 주요 컴포넌트 구조

```
📦 Scripts
 ┣ 📂 Player
 ┃ ┣ PlayerController.cs          // 이동, 점프, 더블점프
 ┃ ┣ GrappleController.cs         // 갈고리 시스템
 ┃ ┣ PlayerAnimationController.cs // 애니메이션 상태 관리
 ┃ ┗ CameraFollow.cs              // 3인칭 카메라
 ┣ 📂 Item
 ┃ ┣ Package.cs                   // 패키지 개별 동작
 ┃ ┣ PackageSpawner.cs            // 랜덤 스폰 관리
 ┃ ┣ ItemMagnet.cs                // 자력 효과 (선택)
 ┃ ┗ PackageData.cs (SO)          // ScriptableObject (선택)
 ┣ 📂 Environment
 ┃ ┣ Trampoline.cs                // 트램펄린 점프대
 ┃ ┗ GrapplePoint.cs              // 갈고리 가능 지점 표시
 ┣ 📂 Manager
 ┃ ┣ GameManager.cs               // 게임 상태/점수 관리
 ┃ ┗ ObjectPool.cs                // 패키지 오브젝트 풀
 ┗ 📂 UI
   ┣ UIManager.cs                 // UI 전체 관리
   ┣ ScoreUI.cs                   // 점수/개수 표시
   ┣ TimerUI.cs                   // 타이머 표시
   ┗ ClearPanel.cs                // 클리어/게임오버 패널
```

---

## ✅ 필수 기능 상세 명세

### 1) 플레이어 시스템

#### A. 이동 (PlayerController.cs)
```csharp
// 주요 변수
- float moveSpeed = 5f
- float sprintMultiplier = 1.5f
- float jumpForce = 8f
- bool canDoubleJump = true
- bool isGrounded (Physics.CheckSphere)

// 주요 메서드
- void Move()           // WASD 입력 처리
- void Jump()           // 점프 + 더블점프 로직
- void CheckGround()    // 지면 체크 (Raycast/Sphere)
```

#### B. 갈고리 시스템 (GrappleController.cs)
```csharp
// 주요 변수
- float grappleRange = 20f
- float grappleSpeed = 15f
- LayerMask grappleLayer (건물만)
- LineRenderer ropeLine

// 주요 메서드
- void StartGrapple()   // 레이캐스트로 타겟 찾기
- void ExecuteGrapple() // 스윙 또는 당기기
- void StopGrapple()    // 갈고리 해제
```

#### C. 애니메이션 (PlayerAnimationController.cs)
```csharp
// 애니메이터 파라미터
- float "Speed"         // 0(Idle) ~ 1(Walk) ~ 2(Run)
- bool "IsJumping"
- bool "IsGrappling"

// 상태
1. Idle (속도 0)
2. Run (이동 중)
3. Jump (공중)
```

#### D. 카메라 (CameraFollow.cs)
```csharp
// 3인칭 추적 카메라
- Vector3 offset = (0, 3, -7)
- float smoothSpeed = 0.125f
- 플레이어 뒤에서 부드럽게 따라가기
```

---

### 2) 아이템 시스템

#### A. 패키지 (Package.cs)
```csharp
// 주요 변수
- Rigidbody rb
- float magnetRange = 3f (선택 과제)
- bool isCollected = false

// 주요 메서드
- void Fall()                    // 중력으로 낙하
- void MagnetEffect(Transform)   // 플레이어 방향으로 이동
- void OnTriggerEnter()          // 수집 처리
```

#### B. 스폰 시스템 (PackageSpawner.cs)
```csharp
// 주요 변수
- GameObject packagePrefab
- Transform[] spawnPoints (5~8개)
- ObjectPool packagePool
- int totalPackages = 15
- int currentSpawned = 0
- float spawnHeight = 30f
- float respawnDelay = 2f

// 주요 메서드
- void SpawnInitialPackages(3개) // 시작 시 3개 스폰
- void SpawnPackage()            // 랜덤 위치 선택 후 스폰
- Vector3 GetRandomSpawnPoint()  // 랜덤 스폰 포인트 반환
```

#### C. 오브젝트 풀 (ObjectPool.cs)
```csharp
// 패키지 재사용으로 성능 최적화
- Queue<GameObject> pool
- void Initialize(prefab, size=20)
- GameObject Get()
- void Return(GameObject)
```

---

### 3) 환경 오브젝트

#### 트램펄린 (Trampoline.cs)
```csharp
- float bounceForce = 15f
- void OnCollisionEnter()  // 플레이어 닿으면 위로 튕김
- 파티클 효과 재생
```

#### 갈고리 포인트 (GrapplePoint.cs)
```csharp
- Renderer 색상 표시 (빨강/파랑 등)
- 선택적: 플레이어 가까이 오면 하이라이트
```

---

### 4) 게임 매니저 (GameManager.cs)
```csharp
// 싱글톤 패턴
- static GameManager Instance

// 게임 상태
- enum GameState { Playing, Cleared, GameOver }
- GameState currentState

// 점수 관리
- int collectedCount = 0
- int targetCount = 15
- float remainingTime = 180f (3분)

// 주요 메서드
- void CollectPackage()      // 수집 시 호출
- void UpdateTimer()         // 매 프레임 시간 감소
- void CheckClearCondition() // 클리어 체크
- void GameClear()           // 클리어 처리
- void GameOver()            // 시간 초과 처리
```

---

### 5) UI 시스템

#### 인게임 UI
```
상단 왼쪽:
┌─────────────────┐
│ 📦 수집: 7/15   │
│ ⏰ 남은시간: 2:34│
└─────────────────┘
```

#### 클리어 패널
```
┌───────────────────────┐
│   🎉 배달 완료!       │
│                       │
│   수집: 15/15         │
│   걸린 시간: 1:23     │
│                       │
│  [다시 시작] [종료]   │
└───────────────────────┘
```

#### UIManager.cs
```csharp
- Text collectedText
- Text timerText
- GameObject clearPanel
- GameObject gameOverPanel

- void UpdateScore(int collected, int total)
- void UpdateTimer(float time)
- void ShowClearPanel()
- void ShowGameOverPanel()
```

---

## 🌟 선택 과제 구현 계획

### ① 아이템 자력 효과 ⭐
**구현 위치**: `Package.cs` Update()
```csharp
void Update() {
    float distance = Vector3.Distance(transform.position, player.position);
    if (distance < magnetRange && !isCollected) {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * magnetSpeed * Time.deltaTime;
    }
}
```
**비주얼**: 파란 파티클 이펙트 + 빨려 들어가는 애니메이션

---

### ② 최고 점수 저장 ⭐
**구현**: PlayerPrefs 사용
```csharp
// GameManager.cs
void SaveBestTime(float time) {
    float bestTime = PlayerPrefs.GetFloat("BestTime", 999f);
    if (time < bestTime) {
        PlayerPrefs.SetFloat("BestTime", time);
        PlayerPrefs.Save();
    }
}
```
**UI**: 클리어 패널에 "최고 기록: 1:15" 표시

---

### ③ 방향 화살표 표시 ⭐⭐
**구현 위치**: `DirectionIndicator.cs` (UI Canvas)
```csharp
- Transform nearestPackage
- RectTransform arrowImage (화면 가장자리)

void Update() {
    FindNearestPackage();
    Vector3 screenPos = Camera.main.WorldToScreenPoint(nearestPackage.position);

    // 화면 밖이면 화살표 표시
    if (IsOffScreen(screenPos)) {
        ShowArrow(screenPos);
    } else {
        HideArrow();
    }
}
```
**비주얼**: 오렌지색 화살표 UI + 거리 텍스트

---

### ④ 색상/효과 변화 ⭐
**구현**: Directional Light 색상 변화
```csharp
// GameManager.cs
void UpdateSkyColor() {
    float progress = (float)collectedCount / targetCount;

    // 아침(파랑) → 낮(노랑) → 저녁(주황) → 밤(보라)
    Color skyColor = Color.Lerp(morningColor, eveningColor, progress);
    RenderSettings.ambientLight = skyColor;
    directionalLight.color = skyColor;
}
```

---

### ⑤ ScriptableObject 활용 ⭐⭐
**PackageData.cs**
```csharp
[CreateAssetMenu(fileName = "PackageData", menuName = "Game/Package")]
public class PackageData : ScriptableObject {
    public string packageName;      // "편지", "소포", "특급"
    public int scoreValue;          // 10, 20, 50
    public Color packageColor;      // 색상
    public GameObject visualPrefab; // 외형
    public float weight;            // 낙하 속도 영향
}
```
**활용**: 여러 종류의 패키지 만들기 (일반/특급/VIP 등)

---

### ⑥ 시간 제한 ⭐⭐
**이미 필수 구현**: 3분 타이머 → 0초 되면 GameOver

**추가 아이디어**:
- 특정 패키지 수집 시 시간 +10초 보너스
- 연속 수집 시 콤보 타이머

---

### ⑦ 아이템 상호작용 ⭐⭐⭐
**구현**: 마우스 오버 시 정보 표시
```csharp
// Package.cs
void OnMouseEnter() {
    UIManager.Instance.ShowPackageInfo(packageData);
}

// UI에 툴팁 표시
┌────────────────┐
│ 📦 특급 배송   │
│ 점수: +50      │
│ 무게: 5kg      │
└────────────────┘
```

---

## 🎨 에셋 리스트

### 3D 모델
| 항목 | 설명 | 추천 소스 |
|------|------|-----------|
| 플레이어 | 인간형 캐릭터 | Mixamo (무료) |
| 빌딩 | 단순한 큐브 박스 형태 | ProBuilder로 직접 제작 |
| 패키지 | 박스 + 리본 모델 | Asset Store "Cardboard Box" |
| 트램펄린 | 원형 점프대 | 간단한 실린더 + 스프링 |
| 갈고리 | 작은 후크 모델 | 무료 에셋 또는 기본 도형 |

### 애니메이션
- **Mixamo** (mixamo.com) - 무료
  - Idle
  - Running
  - Jumping
  - (선택) Hanging (갈고리 스윙)

### 이펙트
| 효과 | 파티클 |
|------|--------|
| 수집 효과 | 반짝이 + 링 확산 (노란색) |
| 자력 효과 | 파란 불빛 궤적 |
| 트램펄린 | 하얀 연기 분출 |
| 착지 효과 | 먼지 (회색) |
| 갈고리 발사 | 빠른 라인 + 불꽃 |

### 사운드
| 이벤트 | 효과음 |
|--------|--------|
| 수집 | "띠링~" (벨 소리) |
| 점프 | "휘익" (바람 소리) |
| 갈고리 | "슈웅!" (발사음) |
| 트램펄린 | "퐁~" (스프링) |
| 클리어 | 환호 음악 |

**추천 소스**: Freesound.org, Unity Asset Store (Free)

---

## 📅 개발 단계별 계획 (2주 기준)

### Week 1: 핵심 시스템 구축

#### Day 1-2: 프로젝트 세팅 + 플레이어 기본 이동
- [ ] Unity 프로젝트 생성 (URP)
- [ ] Git 저장소 초기화
- [ ] 씬 구성 (Plane 지면, 큐브 빌딩 3~5개)
- [ ] CharacterController 기반 이동 구현 (WASD)
- [ ] 점프 + 더블 점프 구현
- [ ] 지면 체크 로직
- [ ] 3인칭 카메라 팔로우

#### Day 3-4: 패키지 스폰 + 수집 시스템
- [ ] Package 프리팹 생성 (큐브 + Rigidbody)
- [ ] 스폰 포인트 배치 (빈 오브젝트 5~8개)
- [ ] PackageSpawner 구현 (랜덤 스폰)
- [ ] 오브젝트 풀 구현
- [ ] 충돌 감지 → 수집 로직
- [ ] 수집 시 사운드/파티클

#### Day 5-6: 게임 매니저 + UI
- [ ] GameManager 싱글톤 구현
- [ ] 점수/목표 관리 변수
- [ ] 타이머 시스템 (180초)
- [ ] UI Canvas 구성
  - 점수 텍스트
  - 타이머 텍스트
- [ ] 클리어 조건 체크
- [ ] 클리어 패널 UI
- [ ] 다시 시작/종료 버튼

#### Day 7: 환경 오브젝트 + 테스트
- [ ] 트램펄린 구현 (OnCollisionEnter)
- [ ] 빌딩 배치 및 레벨 디자인
- [ ] 첫 플레이 테스트
- [ ] 밸런스 조정 (점프력, 이동속도, 스폰 간격)

---

### Week 2: 갈고리 시스템 + 폴리싱

#### Day 8-9: 갈고리 시스템
- [ ] GrappleController 구현
- [ ] 레이캐스트 타겟 감지
- [ ] LineRenderer로 로프 시각화
- [ ] 스윙/당기기 물리 구현
- [ ] 갈고리 포인트 배치 및 표시
- [ ] 갈고리 사운드/이펙트

#### Day 10-11: 애니메이션 + 폴리싱
- [ ] Mixamo 캐릭터 다운로드
- [ ] 애니메이션 임포트 (Idle/Run/Jump)
- [ ] Animator Controller 구성
- [ ] 상태 전환 로직 연결
- [ ] 시각 효과 개선 (파티클, 조명)
- [ ] 사운드 추가

#### Day 12-13: 선택 과제 구현
- [ ] ① 자력 효과
- [ ] ② 최고 기록 저장
- [ ] ③ 방향 화살표 (또는)
- [ ] ④ 하늘 색상 변화
- [ ] 추가 폴리싱

#### Day 14: 빌드 + 시연 영상
- [ ] Windows/Mac 빌드
- [ ] 시연 영상 녹화 (30~60초)
  - 게임 시작
  - 이동/점프/갈고리 시연
  - 패키지 수집
  - 랜덤 스폰 확인
  - 클리어 달성
- [ ] README.md 작성
- [ ] Git 최종 커밋 및 푸시

---

## 🎯 핵심 성공 지표

### 필수 달성 항목
- ✅ 플레이어 이동 (WASD + 점프)
- ✅ 더블 점프 구현
- ✅ 갈고리 시스템 작동
- ✅ 패키지 랜덤 스폰 (하늘에서 낙하)
- ✅ 15개 수집 시 클리어
- ✅ 오브젝트 풀 사용
- ✅ 애니메이션 3종 (Idle/Run/Jump)
- ✅ UI (점수, 타이머, 클리어 패널)
- ✅ 시간 제한 (3분)
- ✅ 수집 시 피드백 (사운드/이펙트)

### 선택 과제 목표
최소 2개 이상 구현 권장:
- 자력 효과 (쉬움, 임팩트 큼) ⭐
- 최고 기록 (매우 쉬움) ⭐
- 하늘 색상 변화 (쉬움, 비주얼 좋음) ⭐

---

## 📝 제출물 체크리스트

### 1. 빌드 파일
- [ ] Windows .exe (또는 Mac .app)
- [ ] Data 폴더 포함
- [ ] 실행 가능 확인

### 2. 시연 영상 (30~60초)
- [ ] 게임 시작 화면
- [ ] 플레이어 조작 (이동, 점프, 더블점프, 갈고리)
- [ ] 패키지가 하늘에서 떨어지는 장면
- [ ] 트램펄린 사용
- [ ] 패키지 수집 장면
- [ ] UI 변화 (점수 증가)
- [ ] 랜덤 스폰 확인
- [ ] 클리어 달성

### 3. Git 저장소
- [ ] README.md (게임 설명, 조작법, 구현 기능)
- [ ] 커밋 히스토리 명확
- [ ] .gitignore 설정 (Library, Temp 제외)

---

## 📌 참고 자료

### Unity 공식 문서
- [CharacterController](https://docs.unity3d.com/ScriptReference/CharacterController.html)
- [Physics.Raycast](https://docs.unity3d.com/ScriptReference/Physics.Raycast.html)
- [Object Pooling](https://learn.unity.com/tutorial/introduction-to-object-pooling)
- [LineRenderer](https://docs.unity3d.com/Manual/class-LineRenderer.html)

### 추천 튜토리얼
- Brackeys - Third Person Movement in Unity
- Code Monkey - Object Pooling
- Brackeys - 2D Grappling Hook (3D로 응용 가능)

---

**문서 버전**: 1.0
**최종 수정일**: 2025-11-14
**작성자**: Claude AI Assistant
