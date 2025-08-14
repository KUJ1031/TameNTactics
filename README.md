# **TameNTactics**

---

<img width="700" height="300" alt="image" src="https://github.com/user-attachments/assets/100548de-d924-40e7-9319-59ea1c060340" />

> **개성 있는 몬스터들을 수집하여, 자신만의 엔트리를 만드세요!**
>
</br></br>

### 프로젝트 결과물 소개

- **게임 플레이 사진**

<img width="360" height="200" alt="image" src="https://github.com/user-attachments/assets/ac76cb8f-68f6-4ff8-9c06-1d824fb4a577" />

<img width="360" height="200" alt="image" src="https://github.com/user-attachments/assets/d2dd87ee-dbe3-4615-a99b-841266bf04a9" />

<img width="360" height="200" alt="image" src="https://github.com/user-attachments/assets/e34d67ee-9baf-4c53-b78f-f7d7a77899a3" />

<img width="360" height="200" alt="image" src="https://github.com/user-attachments/assets/44a66cad-f025-41cf-ab2a-4f9f8ce2ea35" />

<img width="360" height="200" alt="image" src="https://github.com/user-attachments/assets/f213f93e-874c-4709-87ae-7ce449580bd7" />

<img width="360" height="200" alt="image" src="https://github.com/user-attachments/assets/050e93c4-7233-4b9f-b27e-ac8c8e44159f" />
</br></br>

---
    

**스토리**

전 세계를 휩쓴 ‘반향 증후군’은 극한 감정에 사로잡힌 인간을 이성 없는 이형으로 변이시켰다.

인류는 맞서 싸웠지만 끝내 광범위한 지역을 잃고, 감염자들이 세운 ‘파멸의 성’이 새로운 위협의 상징이 되었다. 일부는 그들을 완전히 제거하려 하고, 또 다른 일부는 여전히 인간성을 되찾을 가능성을 믿는다.

한 인물은 칼 대신 대화를 선택해 괴물이 된 자들의 목소리를 듣고자 한다.

그는 직접 싸우지 않지만, 무력으로 맞서는 이들의 선택 또한 막지 않는다.

이 전쟁은 단순한 생존을 넘어, 인간의 정의를 묻는 갈림길이 되었다.

---

### 프로젝트 개요 및 목표

프로젝트 개요

- **탐험과 몬스터 수집 시스템**
필드에 등장하는 다양한 몬스터를 마주치고 포획할 수 있으며, 포획 방법은 플레이어가 가진 포획 방법에 따라 확률이 결정됩니다. 수집한 몬스터는 개별 능력치, 성격, 스킬 구성이 존재하며, 플레이어는 이를 전략적으로 조합해 팀을 구성할 수 있습니다. 플레이어는 최대 5마리의 몬스터를 엔트리에 등록할 수 있고, 그중 3마리를 출전팀으로 설정하여 전투에 참여시킬 수 있습니다.

- **턴제 배틀 시스템**
전투는 클래식한 턴제 방식으로 진행되며, 각 몬스터는 자신의 속도 스탯에 따라 턴 순서가 결정됩니다.
각 몬스터는 고유의 스킬 세트를 보유하고 있으며, 대상 선택, 효과 적용, 궁극기 쿨타임 관리 등 다양한 전략적 요소가 포함됩니다. 전투 중에는 상태이상, 속성 상성, 궁극기 게이지 등 다양한 전투 변수들이 존재하여 단순한 수싸움을 넘어선 전략적 판단이 요구됩니다.

- **몬스터 성장 및 편성**
몬스터는 전투와 탐험을 통해 경험치를 획득하며 레벨업하고, 능력치가 점진적으로 상승합니다.
플레이어는 수집한 몬스터들을 분석하고 조합하여 상황에 맞는 최적의 팀을 구성하는 전략을 즐길 수 있습니다. 수집의 동기 부여를 위해 다양한 몬스터 디자인, 성격 등 변화를 구현하여 배틀의 전략성을 살리기 위해 스킬 타입, 속성 상성, 스탯 기반 턴 시스템 등 전투 로직에 집중하였습니다.

---

### 기술적인 도전 과제

<details>
<summary>김웅진</summary>

### - Addressable을 통한 CSV 데이터 파싱 및 NPC 대화 기능 적용

**구상 이유**
- 게임 내에서 대사와 연관된 다양한 데이터(텍스트, 스프라이트, 사운드 등)를 외부에서 효율적으로 관리하고, 대화 시스템과 게임 로직을 유기적으로 연결하여 유연하고 확장 가능한 대화 기능을 구현하려는 목적.
- CSV를 통해 대사 콘텐츠를 비개발자도 쉽게 수정·확장 가능하도록 분리.
- Dictionary와 트리 구조를 활용해 빠른 런타임 노드 조회와 독립적 대화 관리.
- Addressables를 사용해 필요한 리소스만 동적으로 로드, 메모리 최적화 및 원격 패치 가능.
- 이벤트 키를 활용해 대화와 퀘스트, 아이템 지급 등 게임 시스템 간 연동 강화.
- UI 및 입력 차단 플래그로 대화 중 플레이어 경험을 일관되고 안정적으로 유지.

**문제 발견**
- Addressables 비동기 로딩 중 대화 시작 호출 시 `dialogueTrees`가 비어 오류 발생 (race condition).
- Addressables 핸들 해제 누락으로 인한 메모리 누수와 리소스 언로드 실패.
- CSV 파서가 개행이나 큰따옴표 포함 필드 처리를 제대로 못해 데이터 손상 발생.
- 스프라이트 로드 전략 부재로 다중 캐릭터/표정 지원 및 UI 표시 문제.
- 입력 시스템 매핑 실패 시 상호작용 불가 및 입력 차단 상태 불일치 발생.
- CSV 내 존재하지 않는 노드 참조 시 대화가 예기치 않게 종료됨 (데이터 검증 부족).
- 한글 CSV 인코딩 문제로 이상 문자 표시 발생.
- CSV 하나에 모든 확장 로직 집약 시 데이터 복잡도 및 유지보수성 저하.

**해결 방안**
- 대화 시작 시 로딩 완료 대기 콜백 등록 → 비동기 로딩 완료 후 자동 재시도.
- Addressables 핸들 저장 및 `OnDestroy` 등에서 명확히 해제 처리.
- 대화 종료 시 입력 차단 해제 보장, 예외 경로까지 안전망 구성.
- `InputAction` 기반 안전한 입력 매핑과 폴백 처리.
- 상호작용 상태 명확히 관리해 대화 중 동작 충돌 방지.
- `TriggerEvent` 예외 처리 추가해 대화 흐름 중단 방지.
- `LateEventKey` 구문 통일 및 빈 키 무시.

---

### - Queue와 Coroutine을 통한 맵 내 실시간 팝업 기능

**구상 이유**
- 게임 플레이 중에 특정 이벤트(아이템 획득, 퀘스트 시작, NPC 등장 등)를 실시간으로 플레이어에게 알려야 함.
- 한 번에 여러 이벤트가 발생할 수 있으므로, 순서대로 차례차례 보여줘야 함.
- UI 애니메이션(슬라이드 인/아웃)과 일정 대기 시간(3초)을 포함한 팝업을 구현하려면 **비동기 처리**가 필요.
- Unity에서 프레임 단위 애니메이션과 순차 처리를 위해 **Coroutine**을 사용하고, 여러 이벤트를 차곡차곡 쌓기 위해 **Queue** 자료구조를 선택.

**문제 발견**
- 여러 알림이 동시에 발생하면, 이전 알림이 끝나기 전에 다음 알림이 나타나 UI가 겹치는 현상.
- 씬 전환 시, 아직 보여주지 못한 알림들이 사라져 플레이어가 놓칠 수 있음.
- 한 알림이 표시되는 동안 다른 알림이 들어오면 **순차 처리**가 필요하지만, 일반적인 `Update` 기반 UI 코드로는 이를 제어하기 어려움.

**해결 방안**
1. **Queue 활용 (`alertQueue`)**
   - 이벤트 요청을 `EventAlertRequest`로 생성 후 Queue에 저장.
   - FIFO 구조로 먼저 들어온 알림부터 처리 → 순차 표시 보장.
2. **Coroutine 기반 순차 처리 (`ProcessAlertQueue`)**
   - `isDisPlaying` 플래그로 중복 실행 방지.
   - Coroutine에서 `alertQueue`의 알림을 하나씩 꺼내 `DisPlayerAlert` 실행.
   - 각 알림: **슬라이드 인 → 일정 시간 대기 → 슬라이드 아웃**.
3. **씬 전환 시 알림 보존 (`pendingAlerts`)**
   - 씬 변경 전에 `AddPendingAlert`로 리스트에 저장.
   - `Start()`에서 다시 Queue에 넣어 알림 이어서 표시.

---

### - SerializableDictionary를 통한 JsonUtility 딕셔너리 직렬화 우회

**구상 이유**
- 게임 내 데이터를 **Key-Value** 구조로 관리 (아이템 ID-설명, 퀘스트 ID-보상 등).
- `JsonUtility`로 직렬화/역직렬화 가능하지만, 기본 `Dictionary`는 Unity에서 직렬화 불가.
- Inspector 노출 및 직렬화가 가능한 대체 구조 필요.

**문제 발견**
- `Dictionary`는 `[Serializable]` 속성이 없어 직렬화 불가.
- Inspector에서 표시되지 않아 데이터 수정/확인 불편.

**해결 방안**
1. **KeyValuePair 구조체 변환 (`SerializableKeyValuePair`)**
   - `[Serializable]` 클래스에 key, value 필드 정의 → Inspector, JsonUtility 직렬화 가능.
2. **리스트(pairs) + 런타임 딕셔너리 병행**
   - List로 Inspector, JSON 변환 / Dictionary로 런타임 접근.
3. **Unity 직렬화 콜백 사용 (`ISerializationCallbackReceiver`)**
   - `OnBeforeSerialize()` : 딕셔너리 → 리스트 변환.
   - `OnAfterDeserialize()` : 리스트 → 딕셔너리 변환.
4. **Wrapper 메서드 제공**
   - Add, Remove, ContainsKey, 인덱서 등 Dictionary와 동일한 사용 방식.

</details>

<details>
<summary>반장훈</summary>

### - MVP 패턴을 활용한 UI 구성

**구상 이유**
- 렌더링 부분과 로직 부분을 분리하여 관리 효율성 향상.
- 기존 프로젝트에서는 혼합 작성으로 인해 작은 수정도 불편.

**문제 발견**
- 한 파일에서 렌더 + 로직 혼용 시 확장성이 떨어지고 수정/추가가 어려움.

**해결 방안**
- UI 로직(데이터, PlayerPrefab 등)은 Presenter 및 Manager로 관리.
- 단순 렌더링은 View로 구성.

---

### - Visual Studio 디버깅 기능 활용

**구상 이유**
- 웹 개발 시 VSCode 디버깅처럼 게임 개발에서도 디버깅 활용.
- 버그 수정, 로직 검증 효율 상승 기대.

**문제 발견**
- `Debug.Log`만 사용 시 상세 데이터, 스택, 타이밍 확인이 어려움.

**해결 방안**
- 중단점을 통한 코드 실행 흐름 및 데이터 확인.
- 특정 타이밍에 값이 전달/저장되는지 검증.

</details>

<details>
<summary>서원</summary>

### - FSM(유한 상태 머신) 패턴을 활용한 턴제 기반 전투 구현

**도전 내용**
- 턴제 전투 시스템 최초 구현.
- 상태 변화가 많아 조건문만으로는 유지보수 어려움.
- 상태 예시: 대기 → 몬스터 선택 → 스킬 선택 → 대상 선택 → 스킬 실행.

**진행 방식**
- 각 전투 단계를 독립 `State` 클래스로 분리.
- `BattleStateMachine`이 현재 상태 관리 및 전환.
- 상태별 진입/갱신/종료 로직 분리.

**효과**
- 상태 전환 명확화, 확장성 향상.

---

### - 팩토리 패턴을 활용한 스킬 생성 로직

**도전 내용**
- 다양한 스킬 클래스와 실행 로직 존재.
- 기존에는 `switch`문 + `new`로 생성 → 유지보수 어려움.

**진행 방식**
- `NormalSkillFactory`에 `Dictionary<NormalSkillList, Func<SkillData, ISkillEffect>>` 등록.
- `GetNormalSkill(data)`로 스킬 객체 생성.
- 키값만 맞으면 새로운 스킬 추가 가능.

**효과**
- 스킬 추가/삭제 용이, 분기문 최소화.

</details>

<details>
<summary>민동현</summary>

### - Input System을 이용한 플레이어 키 셋팅

**구상**
- 새 Input System을 활용해 플레이어가 직접 게임 내 조작 키를 런타임에 변경할 수 있는 시스템을 구현.
- Unity Input System의 `InputActionAsset`을 기반으로 액션맵 및 액션별 기본 키맵을 설정.
- 키 변경 UI를 제작하여 플레이어가 원하는 키를 실시간으로 입력받아 바인딩을 변경하도록 구현.
- 직접 키 입력을 감지하는 기존 방식에서 벗어나, Input System의 `PerformInteractiveRebinding()` API와 커스텀 필드를 활용해 안정적이고 직관적인 리바인딩 흐름을 구축.
- 리셋 기능도 구현하여 언제든 기본 설정으로 복구할 수 있도록 지원.
- UI와 연동해 현재 바인딩 상태를 텍스트로 표시하며, 변경 완료 및 미입력 상태에 따른 알림 UI를 구현.

**해결**
- 플레이어가 자신의 조작 취향에 맞게 키를 자유롭게 변경할 수 있어 편의성이 크게 향상.
- 저장된 바인딩 정보를 PlayerPrefs에 관리해 설정을 영구 저장 및 복원할 수 있게 되어 사용자 경험이 개선되.
- UI와 로직이 분리되고 명확한 역할 분담을 통해 코드 구조가 간결하고 이해하기 간편.

</details>

<details>
<summary>김건형</summary>

### - ObjectPooling을 사용한 AudioManager

**구상**
- `ScriptableObject`로 `AudioData`(클립, 재생시간, 루프여부 등) 관리.

**문제**
- BGM은 단일 `AudioSource`로 가능하지만, SFX는 동시 다발 재생으로 단일 소스 불가.

**해결**
- 오브젝트 풀링으로 다수 `AudioSource`를 미리 생성 후 순환 사용.

</details>

</br></br>

---

### 사용된 기술 스택

- 사용 도구

| 항목 | 버전 |
| --- | --- |
| Unity | 2022.3.17f |
| Visual Studio |  |
| Notion |  |
| Figma |  |
</br>

- 사용 기술

| 이름 | 분류 |
| --- | --- |
| Singleton | 디자인 패턴 (싱글톤) |
| Factory Pattern | 디자인 패턴 (팩토리 패턴) |
| Observer Pattern | 디자인 패턴 (옵저버 패턴) |
| CSV | 데이터 포맷 및 파일 관리 |
| Addressable | Unity 어드레서블 에셋 관리 |
| InputAction | Unity 입력 시스템 |
| Finite State Machine (FSM) | 상태 기반 게임 로직 구현 |
| ScriptableObject | Unity 데이터 자산 관리 |
| JSON | 데이터 직렬화 및 저장 |
| Cinemachine | Unity 카메라 컨트롤 |
| EventBus | 상태 전달을 통한 게임 흐름 설계 |
</br>
- 버전 관리

| 이름 | 사용 이유 |
| --- | --- |
| Github | 소스 코드 버전 관리 |
| Github Desktob | Github GUI 클라이언트 |

</br></br>

---

### 클라이언트 구조

- 필드 내 이동 로직
  <img width="7448" height="3224" alt="image" src="https://github.com/user-attachments/assets/34479643-9b01-4c00-a3f2-f8a6d8b2ee9f" />

    
- FSM을 활용한 전투 로직
  <img width="1887" height="750" alt="image" src="https://github.com/user-attachments/assets/4f165871-41e1-435d-8b11-c8dfffd01b4b" />


</br></br>

---

### 프로젝트 결과 및 성과

| 구분 | 세부 내용 |
| --- | --- |
| 데이터 처리 | - CSV 파일과 Unity Addressable 시스템을 활용해 몬스터, 대사 등 게임 데이터베이스를 효율적으로 저장 및 관리데이터 파싱을 통해 게임 내 데이터와 정확히 동기화 구현 |
| 저장 및 불러오기 기능 | - JSON 포맷을 이용해 플레이어 진행 상황, 몬스터 상태, 인벤토리 등 게임 상태를 직렬화하여 저장저장된 JSON 파일을 역직렬화해 게임 실행 시 데이터 복원 기능 구현 |
| 전투 시스템 개발 | - FSM(유한 상태 머신) 구조를 적용한 턴제 전투 시스템 설계상태별 전투 흐름(스킬 선택, 타겟 지정, 스킬 실행, 턴 종료 등)을 명확히 구분하여 안정적 전투 진행 지원 |
| 데이터 관리 | - Scriptable Object를 활용해 몬스터, 스킬, 아이템 등 게임 요소들을 데이터 중심으로 관리코드와 데이터 분리를 통해 유지보수성 및 확장성 확보 |
| 협업 및 소통 | - Github를 활용하여 코드 버전 관리 및 팀원 간 효율적인 협업 체계 구축Pull Request, 이슈 관리로 개발 진행 상황 공유 및 피드백 활성화 |
| 팀 내 방향성 관리 | - Notion을 통해 기획 문서, 일정, 회의록 등을 통합 관리하여 팀 목표 및 작업 현황을 명확히 공유, Figma를 사용해 UI/UX 디자인 시안 및 작업물을 시각적으로 공유 및 검토 |
| 사용자 경험 개선 | - PA(Product Analysis)를 주기적으로 진행해 UI/UX 개선점 도출 및 적용플레이어 행동 데이터를 분석하여 게임 흐름과 인터페이스 최적화 |
| 유저 테스트 | - 실제 사용자 테스트를 통해 버그 및 불편사항을 발견하고 개선사용자 피드백을 반영해 게임 밸런스 및 콘텐츠 완성도 향상 |
| 프로젝트 도전 | - RPG 장르 개발에 도전하여 다양한 시스템 설계, 데이터 관리, UI/UX 경험 획득프로젝트 전반에 걸쳐 이전보다 높은 수준의 개발 및 협업 경험 축적 |

</br></br>

---

### 팀원 구성 및 연락처

| 이름   | 태그   | 담당 내용                                                         | 블로그                                      | 깃허브                                   |
|--------|--------|------------------------------------------------------------------|---------------------------------------------|-----------------------------------------|
| 김웅진 | 팀장   | - JSON을 통한 직렬화 및 역직렬화<br>- CSV, Addressable NPC 상호작용 기능<br>- 맵 내 퍼즐 및 퀘스트 로직<br>- 스토리 구상 및 NPC 대사 작성 | [블로그](https://thsusekdnlt1.tistory.com/) | [GitHub](https://github.com/KUJ1031/) |
| 반장훈 | 부팀장 | - 배틀 UI MVP 구성<br>- 배틀UI 매니저 관리<br>- 배틀 전반적인 오류, 버그 디버깅<br>- 프로젝트 일부 버그 디버깅       | [블로그](https://velog.io/@janghoon333/series) | [GitHub](https://tinaro.tistory.com/)  |
| 민동현 | 팀원   | - 플레이어 Entry 관리<br>- Audio 데이터 및 재생 관리전반적인 필드 UI디자인및 기능타일맵을 활용한 맵 구성 | [블로그](https://tinaro.tistory.com/)   | [GitHub](https://tinaro.tistory.com/) |
| 김건형 | 팀원   | - 플레이어 Entry 관리<br>- Audio 데이터 및 재생 관리<br>- 전반적인 필드 UI 디자인 및 기능<br>- 타일맵을 활용한 맵 구성 | [블로그](https://tinaro.tistory.com/)        | [GitHub](https://github.com/KGH1125)  |
| 서원   | 팀원   | - BattleManager 관련 로직 구상 및 작성<br>- FSM(유한상태머신) 구상 및 작성<br>- 팩토리 패턴을 활용한 스킬 생성 로직  | [블로그](https://velog.io/@smjwnaya1102/posts) | [GitHub](https://github.com/Won0001)  |

</br></br>

---
