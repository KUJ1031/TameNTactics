using System.Collections.Generic;
using UnityEngine;

public class WanderingShopNPCHandler : WanderingShopNPC
{
    [Header("떠상 등장 관련 설정")]
    private List<int> appearedTimes = new List<int> { 0, 1440 }; // 게임 분 기준
    public List<Transform> appearedTransform = new List<Transform>(); // 등장할 수 있는 위치들
    public GameObject npcPrefab; // 떠상 NPC 프리팹

    private int appearDuration = 60; // 몇 분 동안 떠있는가
    private bool isCurrentlyActive = false;
    private GameObject currentNPCInstance = null; // 현재 떠있는 NPC 인스턴스

    [Header("떠상 NPC 부모 설정")]
    public Transform npcParent;  // 인스펙터에서 지정할 부모

    [Header("구매창 사라지게 하기 위한 참조")]
    public DialogueUI dialogueUI; // 대화 UI 참조
    public WanderingShopUI wanderingShopUI; // 떠상 상점 UI 참조

    void Update()
    {
        float currentTime = GameTimeFlow.Instance.timer;
        bool shouldBeActive = false;

        foreach (int appearTime in appearedTimes)
        {
            float disappearTime = appearTime + appearDuration;
            if (currentTime >= appearTime && currentTime < disappearTime)
            {
                shouldBeActive = true;
                break;
            }
        }

        if (shouldBeActive && !isCurrentlyActive && currentNPCInstance == null)
        {
            isCurrentlyActive = true;
            Debug.Log($"[WanderingShopNPC] 상점 NPC가 나타났습니다. 시간: {GameTimeFlow.Instance.timeString}");
            SpawnWanderingNPC();
        }
        else if (!shouldBeActive && isCurrentlyActive && currentNPCInstance != null)
        {
            isCurrentlyActive = false;
            Debug.Log($"[WanderingShopNPC] 상점 NPC가 사라졌습니다. 시간: {GameTimeFlow.Instance.timeString}");
            RemoveWanderingNPC();
        }
    }

    private void SpawnWanderingNPC()
    {
        if (npcPrefab == null || appearedTransform.Count == 0)
        {
            Debug.LogWarning("npcPrefab 또는 appearedTransform이 비어있습니다.");
            return;
        }

        Transform spawnPoint = appearedTransform[Random.Range(0, appearedTransform.Count)];

        // npcParent가 지정되어 있으면 부모 설정, 없으면 null
        currentNPCInstance = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation, npcParent);
    }

    private void RemoveWanderingNPC()
    {
        if (currentNPCInstance != null)
        {
            var shopNPC = currentNPCInstance.GetComponentInChildren<WanderingShopNPC>();
            if (shopNPC != null && shopNPC.IsInteracting)
            {
                Debug.LogWarning("[WanderingShopNPC] 상호작용 중에 NPC가 사라졌습니다. 대화 UI와 상점 UI를 비활성화합니다.");
                dialogueUI.gameObject.SetActive(false);
                wanderingShopUI.gameObject.SetActive(false);
                CameraController.Instance.SwitchTo("PlayerCamera", true, false);
                CameraController.Instance.SetTarget(PlayerManager.Instance.playerController.transform);
            }

            Destroy(currentNPCInstance);
            currentNPCInstance = null;
        }
    }

}
