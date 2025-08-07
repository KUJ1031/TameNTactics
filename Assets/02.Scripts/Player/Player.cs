using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

[System.Serializable]
public class Player
{
    [Header("나의 전체 몬스터")]
    public List<Monster> ownedMonsters = new();

    [Header("엔트리 등록 몬스터 (최대 5)")]
    public List<Monster> entryMonsters = new();

    [Header("전투 출전 몬스터 (최대 3)")]
    public List<Monster> battleEntry = new();

    [Header("벤치 몬스터")]//(entryMonsters-battleEntry)
    public List<Monster> benchEntry = new();

    public int maxEntryCount = 5;
    public int maxBattleCount = 3;

    [Header("인벤토리/골드")]
    public List<ItemInstance> items = new List<ItemInstance>();
    [NonSerialized] public List<ItemInstance> consumableItems = new List<ItemInstance>(); // 소모 아이템 인스턴스
    [NonSerialized] public List<ItemInstance> equipableItems = new List<ItemInstance>(); // 인벤토리 아이템 인스턴스
    [NonSerialized] public List<ItemInstance> gestureItems = new List<ItemInstance>(); // 제스쳐 아이템 인스턴스
    public int gold;

    [Header("기본 정보")]
    public string playerName;
    public int totalPlaytime;
    public float playerLastGameTime;
    public Vector3 playerLastPosition;
    public string playerLastStage;
    public int playerGetMonsterCount;
    public int playerGender;
    public List<ItemInstance> playerEquipment = new(); // 장착 아이템 목록

    [Header("진행 정보")]
    public bool playerBattleTutorialCheck = false; // 배틀 클리어 체크
    public bool playerAllTutorialCheck = false; // 모든 튜토리얼 체크

    public SerializableDictionary<int, bool> playerQuestStartCheck = new();
    public SerializableDictionary<int, bool> playerQuestClearCheck = new();

    public SerializableDictionary<int, bool> playerEliteStartCheck = new(); // 스테이지 클리어 체크
    public SerializableDictionary<int, bool> playerEliteClearCheck = new();
    public SerializableDictionary<int, bool> playerBossClearCheck = new();

    public SerializableDictionary<int, bool> playerPuzzleClearCheck = new();

    [Header("설정 정보")]
    public SerializableDictionary<string, string> playerKeySetting = new();


    //monster Null 체크
    private bool CheckMonster(Monster monster)
    {
        return monster != null && monster.monsterData != null;
    }

    // Owned에 추가
    public bool AddOwnedMonster(Monster monster)
    {
        if (!CheckMonster(monster)) return false;

        ownedMonsters.Add(monster);
        monster.monsterData.captureCount++;
        playerGetMonsterCount++;
        return true;
    }

    //Owned에서 제거
    public void TryRemoveOwnedMonster(Monster monster, Action<bool> onCompleted)
    {
        if (!CheckMonster(monster))
        {
            onCompleted?.Invoke(false);
            return;
        }
        if(entryMonsters.Count == 1 && entryMonsters.Contains(monster))
        {
            string warningMessage = "배틀 엔트리에는 최소 한마리의 몬스터는 존재해야 합니다!";
            FieldUIManager.Instance.OpenConfirmPopup(PopupType.Warning, warningMessage, (_) => { });
            return;
        }
        //정말 내보낼까요? 팝업띄우고 ok했을때 내보내기
        string message = "정말 내보내겠습니까?";
        FieldUIManager.Instance.OpenConfirmPopup(PopupType.Confirm, message, (isOK) =>
        {
            if (isOK)
            {
      
                if (ownedMonsters.Remove(monster))
                {
                    if (entryMonsters.Remove(monster))
                    {
                        battleEntry.Remove(monster);
                        benchEntry.Remove(monster);
                    }
                    onCompleted?.Invoke(true);
                }
                else
                {
                    Debug.LogWarning("몬스터 제거 실패(소유 목록에 없거나 이미 제거됨)");
                    onCompleted?.Invoke(false);
                }
            }
            else
            {
                onCompleted?.Invoke(false);
            }
        });

    }

    //Entry에 추가
    public void TryAddEntryMonster(Monster newMonster, Action<Monster, Monster> onCompleted)
    {
        if (!CheckMonster(newMonster))
        {
            onCompleted?.Invoke(null, null);
            return;
        }

        if (entryMonsters.Contains(newMonster))
        {
            onCompleted?.Invoke(null, null);
            return;
        }

        if (entryMonsters.Count < maxEntryCount)
        {
            entryMonsters.Add(newMonster);

            if (battleEntry.Count < maxBattleCount)
                battleEntry.Add(newMonster);
            else
                benchEntry.Add(newMonster);

            onCompleted?.Invoke(null, newMonster); //교체된 몬스터 없음
        }
        else //최대 수 보다 많을 땐 교체진행
        {
            FieldUIManager.Instance.OpenEntrySwapPopup((oldMonster) =>
            {
                if (oldMonster == null || !entryMonsters.Contains(oldMonster))
                {
                    onCompleted?.Invoke(null, null);
                    return;
                }

                entryMonsters.Remove(oldMonster);
                entryMonsters.Add(newMonster);

                if (battleEntry.Contains(oldMonster))
                {
                    battleEntry.Remove(oldMonster);
                    battleEntry.Add(newMonster);
                }
                else
                {
                    benchEntry.Remove(oldMonster);
                    benchEntry.Add(newMonster);
                }

                onCompleted?.Invoke(oldMonster, newMonster); //교체된 몬스터와 새 몬스터 전달
            });
        }
    }

    //Entry에 제거
    public void RemoveEntryMonster(Monster monster)
    {
        if (!CheckMonster(monster)) return;
        if (!entryMonsters.Contains(monster)) return;
        if (entryMonsters.Count == 1)
        {
            string message = "엔트리에는 최소 한마리의 몬스터가 존재해야 합니다.";
            FieldUIManager.Instance.OpenConfirmPopup(PopupType.Warning, message, (_) => { return; });
            return;
        }

        //엔트리에서 제거
        if (entryMonsters.Remove(monster))
        {   
            //베틀 엔트리에 있다면 제거
            if (battleEntry.Contains(monster))
            {
                battleEntry.Remove(monster);
            }
            else
            {
                benchEntry.Remove(monster);
            }

            //베틀 엔트리 비어있으면 보충
            if (benchEntry.Count > 0 && battleEntry.Count < maxBattleCount)
            {
                Monster promoted = benchEntry[0];
                benchEntry.RemoveAt(0);
                battleEntry.Add(promoted);
            }
        }
        return;
    }

    //battleEntry에 추가
    public void AddBattleEntry(Monster monster)
    {
        if (!CheckMonster(monster)) return ;
        if (battleEntry.Contains(monster)) return ;
        if (!entryMonsters.Contains(monster)) return ;
        if (battleEntry.Count >= maxBattleCount) {
            MoveToBenchEntry(battleEntry[battleEntry.Count - 1]);
        } 
        benchEntry.Remove(monster);
        battleEntry.Add(monster);
    }

    // battleEntry -> benchEntry 이동
    public void MoveToBenchEntry(Monster monster) { 
    
        if (!CheckMonster(monster)) return ;
        if (!battleEntry.Remove(monster)) return ;
        benchEntry.Add(monster);
        Debug.Log(monster.monsterName + "이 벤치로 이동");
    }



    //전체 엔트리 초기화
    public void ClearEntry()
    {
        entryMonsters.Clear();
        battleEntry.Clear();
        benchEntry.Clear();
    }

    // 출전 상태 확인
    public bool IsInEntry(Monster monster) => entryMonsters.Contains(monster);
    public bool IsInBattle(Monster monster) => battleEntry.Contains(monster);

    // MonsterData 기준으로 검색할 때 사용
    public Monster FindMonsterByData(MonsterData data)
    {
        return ownedMonsters.Find(m => m.monsterData == data);
    }

    public int GetTotalEffectBonus(ItemEffectType effectType)
    {
        return playerEquipment
            .Where(item => item != null && item.isEquipped)
            .SelectMany(item => item.data.itemEffects)
            .Where(effect => effect.type == effectType)
            .Sum(effect => effect.value);
    }

    public void UpdateCategorizedItemLists()
    {
        equipableItems.Clear();
        consumableItems.Clear();
        gestureItems.Clear();

        // itemID 기준으로 정렬
        var sortedItems = items.OrderBy(item => item.data.itemId);

        foreach (var item in sortedItems)
        {
            switch (item.data.type)
            {
                case ItemType.equipment:
                    equipableItems.Add(item);
                    break;
                case ItemType.consumable:
                    consumableItems.Add(item);
                    break;
                case ItemType.gesture:
                    gestureItems.Add(item);
                    break;
                default:
                    Debug.LogWarning($"[ItemManager] 알 수 없는 타입: {item.data.itemName} / {item.data.type}");
                    break;
            }
        }
    }


    // 아이템 추가
    public void AddItem(ItemData item, int quantity = 1)
    {
        var existing = items.Find(i => i.data.itemName == item.itemName);

        if (existing != null)
            existing.quantity += quantity;
        else
            items.Add((new ItemInstance(item, quantity)));

        // 자동 분류 갱신
        UpdateCategorizedItemLists();

    }

    public void AddItem(string itemName, int quantity)
    {
        ItemData item = ItemManager.Instance.GetItemByName(itemName);
        if (item != null)
        {
            AddItem(item, quantity);
            if (PlayerManager.Instance.player.playerAllTutorialCheck) EventAlertManager.Instance.SetEventAlert(EventAlertType.GetItem, item ,itemName, quantity);
        }
        else
        {
            Debug.LogWarning($"'{itemName}'이라는 이름의 아이템을 찾을 수 없습니다.");
        }
    }


    // 아이템 제거
    public void RemoveItem(ItemInstance item, int quantity)
    {
        if (!items.Contains(item)) return;

        item.quantity -= quantity;

        if (item.quantity <= 0)
            items.Remove(item);

        UpdateCategorizedItemLists();
    }

    public void RemoveItem(string itemName, int quantity)
    {
        var item = items.Find(i => i.data.itemName == itemName);
        if (item != null)
        {
            RemoveItem(item, quantity);
            EventAlertManager.Instance.SetEventAlert(EventAlertType.RemoveItem, item.data, itemName, quantity);
        }
        else
        {
            Debug.LogWarning($"'{itemName}'이라는 이름의 아이템을 찾을 수 없습니다.");
        }
    }

    public void SendItem(string itemName, int quantity)
    {
        var item = items.Find(i => i.data.itemName == itemName);
        if (item != null)
        {
            RemoveItem(item, quantity);
            EventAlertManager.Instance.SetEventAlert(EventAlertType.SendItem, item.data, itemName, quantity);
        }
        else
        {
            Debug.LogWarning($"'{itemName}'이라는 이름의 아이템을 찾을 수 없습니다.");
        }
    }

    // 골드 추가
    public void AddGold(int amount)
    {
        gold += amount;
    }

    // 골드 사용
    public bool UseGold(int amount)
    {
        if (gold < amount) return false;
        gold -= amount;
        return true;
    }

    // 이름 설정
    public bool SetName(string name)
    {
        if (string.IsNullOrEmpty(name)) return false;
        playerName = name;
        return true;
    }

    public void SetPlayerLastGameTime(string time)
    {
        if (int.TryParse(time, out var result))
            playerLastGameTime = result;
    }

    public void SetPlayerLastPosition(string pos)
    {
        var split = pos.Split(',');
        if (split.Length != 3) return;

        if (float.TryParse(split[0], out float x) &&
            float.TryParse(split[1], out float y) &&
            float.TryParse(split[2], out float z))
        {
            playerLastPosition = new Vector3(x, y, z);
        }
    }

    public void SetPlayerBossClearCheck(int bossId)
    {
        playerBossClearCheck[bossId] = true;
    }

    public void SetPlayerQuestClearCheck(int questId)
    {
        playerQuestClearCheck[questId] = true;
    }

    public void SetPlayerPuzzleClearCheck(int puzzleId)
    {
        playerPuzzleClearCheck[puzzleId] = true;
    }

    public bool HasItem(string itemName)
    {
        return items.Any(i => i.data.itemName == itemName && i.quantity > 0);
    }

}
