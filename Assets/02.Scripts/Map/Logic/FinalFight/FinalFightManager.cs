using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalFightManager : Singleton<FinalFightManager>
{
    public List<MonsterData> dean = new List<MonsterData>();
    public List<MonsterData> eisen = new List<MonsterData>();
    public List<MonsterData> dolan = new List<MonsterData>();
    public List<MonsterData> boss = new List<MonsterData>();

    public GameObject deanObj;
    public GameObject eisenObj;
    public GameObject dolanObj;
    public GameObject bossObj;

    private void Start()
    {
        var player = PlayerManager.Instance.player;

        if (player.playerEliteClearCheck[0]) Destroy(deanObj);
        if (player.playerEliteClearCheck[1]) Destroy(eisenObj);
        if (player.playerEliteClearCheck[2]) Destroy(dolanObj);

        if (player.battleEntry.Count > 0 && player.battleEntry[0] != null)
        {
            player.battleEntry[0].AddExp(300000);
        }

        StartCoroutine(WaitUntilDialogueLoadedAndStart());
    }

    private IEnumerator WaitUntilDialogueLoadedAndStart()
    {
        yield return new WaitUntil(() => DialogueManager.Instance.IsLoaded);
        Debug.Log("FinalFightManager: 대화가 로드되었습니다. 대화 시작합니다.");

        var player = PlayerManager.Instance.player;
        var saveManager = PlayerSaveManager.Instance;

        for (int i = 2; i >= 0; i--)
        {
            if (CheckEliteClears(i + 1) && player.playerEliteStartCheck[i])
            {
                TriggerEliteDialogue(GetEliteName(i), GetEliteSprite(i), GetDialogueID(i), i);
                saveManager.SavePlayerData(player);
                break;
            }
        }
    }

    bool CheckEliteClears(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (!PlayerManager.Instance.player.playerEliteClearCheck[i])
                return false;
        }
        return true;
    }

    void TriggerEliteDialogue(string name, Sprite image, int dialogueId, int index)
    {
        Debug.Log($"FinalFightManager: {name}과의 대화 시작");
        PlayerManager.Instance.player.playerEliteStartCheck[index] = false;
        DialogueManager.Instance.StartDialogue(name, image, dialogueId);
    }

    string GetEliteName(int index)
    {
        return index switch
        {
            0 => "딘",
            1 => "에이센",
            2 => "돌란",
            _ => "Unknown"
        };
    }

    Sprite GetEliteSprite(int index)
    {
        return index switch
        {
            0 => dean[0].monsterImage,
            1 => eisen[0].monsterImage,
            2 => dolan[0].monsterImage,
            _ => null
        };
    }

    int GetDialogueID(int index)
    {
        return index switch
        {
            0 => 1530,
            1 => 1535,
            2 => 1538,
            _ => -1
        };
    }

    public void Fight_Dean() => StartFight(0, dean);
    public void Fight_Eisen() => StartFight(1, eisen);
    public void Fight_Dolan() => StartFight(2, dolan);
    public void Fight_Boss() => StartFight(-1, boss, 20);

    private void StartFight(int index, List<MonsterData> teamData, int levelMin = 1, int levelMax = 1)
    {
        if (index >= 0)
        {
            PlayerManager.Instance.player.playerEliteStartCheck[index] = true;
        }

        var generator = new UnknownForestEnemyTeamGenerator(teamData, levelMin, levelMax);
        var enemyTeam = generator.GenerateRandomTeam(Random.Range(1, 1));

        BattleManager.Instance.enemyTeam = enemyTeam;
        RuntimePlayerSaveManager.Instance.SaveCurrentGameState(PlayerManager.Instance.player);

        SceneManager.sceneLoaded += OnBattleSceneLoaded;
        SceneManager.LoadScene("BattleScene");
    }

    private void OnBattleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "BattleScene")
        {
            GameObject attackObj = GameObject.Find("AttackPosition");
            if (attackObj != null)
            {
                BattleManager.Instance.AttackPosition = attackObj.transform;
            }
            else
            {
                Debug.LogWarning("AttackPosition 오브젝트를 찾지 못했음.");
            }

            SceneManager.sceneLoaded -= OnBattleSceneLoaded;
        }
    }
}