# 🛠️ Rooftop Runner 트러블슈팅 가이드

> Week 1 개발 중 발생한 오류와 해결 방법을 정리한 문서입니다.

---

## 📋 목차

1. [Package가 Ground를 뚫고 떨어지는 문제](#1-package가-ground를-뚫고-떨어지는-문제)
2. [ObjectPool에서 첫 번째만 스폰되는 문제](#2-objectpool에서-첫-번째만-스폰되는-문제)

---

## 1. Package가 Ground를 뚫고 떨어지는 문제

### 🔴 문제 증상
- Package 프리팹이 하늘에서 스폰된 후 Ground(바닥)를 뚫고 계속 아래로 떨어짐
- Ground는 Mesh Collider를 가지고 있음
- Package는 Box Collider를 가지고 있음

### 🔍 원인
**Box Collider의 "Is Trigger"가 체크되어 있어서 물리적 충돌이 무시됨**

Unity의 Trigger Collider는:
- `OnTriggerEnter`, `OnTriggerExit` 등의 이벤트는 발생시킴
- **물리적 충돌은 일으키지 않음** (오브젝트를 통과함)

Package 스크립트에서 플레이어와의 충돌 감지를 위해 Trigger를 사용했지만, 이로 인해 바닥과의 물리 충돌도 무시되는 부작용이 발생했습니다.

### ✅ 해결 방법

#### 방법 1: 두 개의 Collider 사용 (권장)

Package에 **2개의 Box Collider**를 추가하여 각각 다른 역할을 담당하게 합니다:

1. **물리 충돌용 Collider** (Is Trigger 해제)
   - Ground와 충돌하여 떨어지는 것을 막음
   - Size: `(1, 1, 1)` (기본 크기)

2. **수집 감지용 Collider** (Is Trigger 체크)
   - 플레이어와의 충돌을 감지
   - Size: `(1.2, 1.2, 1.2)` (약간 크게 - 수집 범위)

**구현 단계**:
1. `Package` Prefab 열기
2. **Inspector**에서 기존 Box Collider:
   - **Is Trigger**: ✅ 유지
   - **Size**: `(1.2, 1.2, 1.2)`으로 확대
3. **Add Component** → **Box Collider** (두 번째 추가)
   - **Is Trigger**: ❌ 체크 해제
   - **Size**: `(1, 1, 1)` 유지

**장점**:
- ✅ 바닥과의 물리 충돌 정상 작동
- ✅ 플레이어 수집 감지도 정상 작동
- ✅ 수집 범위를 따로 조절 가능
- ✅ 성능 문제 없음
- ✅ 많은 상용 게임에서 사용하는 표준 패턴

#### 방법 2: Collision Detection 변경

Rigidbody의 충돌 감지 정밀도를 높입니다.

**구현**:
1. Package Prefab → **Rigidbody** 컴포넌트
2. **Collision Detection**: `Discrete` → `Continuous`로 변경
3. **Box Collider**의 **Is Trigger**: 체크 해제
4. PlayerController에서 `OnControllerColliderHit` 사용하여 수집 감지

**단점**:
- ⚠️ 성능에 약간 영향
- ⚠️ Package 개수가 많으면 부담

---

## 2. ObjectPool에서 첫 번째만 스폰되는 문제

### 🔴 문제 증상
- ObjectPool에 20개의 Package가 생성됨
- 게임 시작 시 첫 번째 Package만 활성화되어 떨어짐
- 나머지 19개는 비활성화 상태로 멈춰있음
- 시간이 지나도 추가 스폰이 안됨

### 🔍 원인

**Console 로그 분석**:
```
Pool is empty! Creating new object.
NullReferenceException: Object reference not set to an instance of an object
Package.ResetPackage () (at Assets/Scripts/Item/Package.cs:86)
Object Pool Initialized: 20 objects
```

로그의 시간 순서를 보면:
1. ❌ `PackageSpawner.Start()` 먼저 실행 → ObjectPool이 비어있음!
2. ✅ `ObjectPool.Start()` 나중에 실행 → 20개 초기화 완료

**근본 원인 2가지**:

#### 원인 1: Unity 스크립트 실행 순서 문제
- Unity의 `Start()` 메서드는 실행 순서가 보장되지 않음
- `PackageSpawner.Start()`가 `ObjectPool.Start()`보다 먼저 실행됨
- PackageSpawner가 Get()을 호출했을 때 풀이 아직 초기화 안됨
- 결과: "Pool is empty!" 경고 발생

#### 원인 2: Rigidbody가 null인 상태로 ResetPackage() 호출
```csharp
public void ResetPackage()
{
    isCollected = false;
    rb.velocity = Vector3.zero;         // rb가 null!
    rb.angularVelocity = Vector3.zero;  // NullReferenceException
    gameObject.SetActive(true);
}
```

ObjectPool에서 새로 생성된 Package는:
- Instantiate 직후 `SetActive(false)` 됨
- `Package.Start()`가 실행 안됨 (비활성 상태)
- `rb = GetComponent<Rigidbody>()`가 실행 안됨
- `rb`가 null인 상태로 남음

### ✅ 해결 방법

#### 해결책 1: PackageSpawner를 Coroutine으로 지연 실행 (권장)

**`PackageSpawner.cs` 수정**:

```csharp
void Start()
{
    // SpawnInitialPackages(); // 삭제
    StartCoroutine(DelayedSpawn()); // 추가
}

// 새 메서드 추가
IEnumerator DelayedSpawn()
{
    // 한 프레임 대기 (ObjectPool 초기화 기다림)
    yield return null;

    SpawnInitialPackages();
}
```

**동작 원리**:
- `yield return null`은 다음 프레임까지 대기
- 모든 오브젝트의 `Start()`가 실행된 후 스폰 시작
- ObjectPool이 확실히 초기화된 상태

**장점**:
- ✅ 간단하고 확실한 해결책
- ✅ 코드 변경 최소화
- ✅ 이해하기 쉬움

#### 해결책 2: Package.ResetPackage()에 null 체크 추가

**`Package.cs` 수정**:

```csharp
public void ResetPackage()
{
    isCollected = false;

    // rb가 null이면 가져오기 (추가)
    if (rb == null)
    {
        rb = GetComponent<Rigidbody>();
    }

    // rb가 있을 때만 실행 (안전장치)
    if (rb != null)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    gameObject.SetActive(true);
}
```

**동작 원리**:
- `ResetPackage()` 호출 시 `rb`가 null이면 즉시 가져옴
- null 체크로 NullReferenceException 방지
- 방어적 프로그래밍 패턴

**장점**:
- ✅ 안전장치 역할
- ✅ 예상치 못한 null 에러 방지
- ✅ 다른 곳에서 ResetPackage() 호출해도 안전

#### 해결책 3: ObjectPool을 Awake()로 변경

**`ObjectPool.cs` 수정**:

```csharp
void Awake() // Start에서 Awake로 변경
{
    Initialize();
}
```

**동작 원리**:
- `Awake()`는 `Start()`보다 먼저 실행됨
- Unity 실행 순서: `Awake()` → `Start()`
- ObjectPool이 다른 스크립트의 `Start()`보다 먼저 초기화됨

**장점**:
- ✅ 실행 순서 명확하게 제어
- ✅ Unity 라이프사이클 활용

### 🎯 최종 권장 조합

**해결책 1 + 해결책 2를 모두 적용**하는 것을 권장합니다:

- **해결책 1**: 실행 순서 문제 근본적으로 해결
- **해결책 2**: 예상치 못한 상황에 대한 안전장치

이렇게 하면:
- ✅ 초기 스폰 정상 작동
- ✅ 재스폰 정상 작동
- ✅ null 에러 완전히 방지
- ✅ 코드 안정성 향상

---

## 📝 교훈 및 팁

### Unity 스크립트 실행 순서
- `Awake()`: 가장 먼저 실행 (의존성 초기화용)
- `Start()`: Awake() 이후 실행 (다른 오브젝트 참조용)
- 다른 스크립트에 의존하는 초기화는 `Awake()`에서 하거나 Coroutine으로 지연

### Trigger vs Collider
- **Trigger** (`Is Trigger` ✅): 감지만 하고 물리적으로 통과
- **Collider** (`Is Trigger` ❌): 실제 물리 충돌 발생
- 두 가지를 동시에 사용하여 각각의 장점 활용 가능

### ObjectPool과 비활성화 오브젝트
- 비활성화된 오브젝트는 `Start()`, `Awake()` 실행 안됨
- 풀에서 가져온 오브젝트는 명시적으로 초기화 필요
- `Reset()` 또는 `Initialize()` 메서드를 만들어 사용

### Null 체크의 중요성
- Unity에서는 항상 null 체크를 습관화
- 특히 `GetComponent<>()`로 가져온 컴포넌트는 null 가능성 고려
- 방어적 프로그래밍으로 예상치 못한 크래시 방지

---

## 🔗 참고 자료

- [Unity Script Execution Order](https://docs.unity3d.com/Manual/ExecutionOrder.html)
- [Unity Colliders and Triggers](https://docs.unity3d.com/Manual/CollidersOverview.html)
- [Unity Object Pooling](https://learn.unity.com/tutorial/introduction-to-object-pooling)
- [Unity Coroutines](https://docs.unity3d.com/Manual/Coroutines.html)

---

**문서 버전**: 1.0
**최종 수정일**: 2025-11-14
**작성자**: Claude AI Assistant
