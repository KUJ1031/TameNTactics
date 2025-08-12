using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class MiniGameManager : Singleton<MiniGameManager>
{
    [Header("참조")]
    [SerializeField] private RotatePoint rotatePoint;
    [SerializeField] private SuccessRangeSpawner spawner;

    [Header("성공 범위")]
    [SerializeField] private List<RotationRange> ranges = new();

    public InputActionAsset playerInputAsset; // 에디터에서 Input Action Asset을 연결

    [Header("알맞은 제스처")]
    [SerializeField] private float appropriatePercent = 50f;
    [SerializeField] private Color appropriateColor = new(255f, 88f, 230f, 255f);
    [SerializeField] private string appropriateMassage = "호감을 보이는것 같다.";
    [Header("기본 제스처")]
    [SerializeField] private float defaultPercent = 30f;
    [SerializeField] private Color defaultColor = new(0, 255f, 50f, 255f);
    [SerializeField] private string defaultMassage = "관심이 없어보인다.";
    [Header("잘못된 제스처")]
    [SerializeField] private float notAppropriatePercent = 10f;
    [SerializeField] private Color notAppropriateColor = new(255f, 0, 0, 255f);
    [SerializeField] private string notAppropriateMassage = "좋아하지 않는 것 같다.";

    BattleDialogueManager dialogue;
    private Action<bool, Monster> resultCallback;
    private Monster returnMonster;

    private string keySettingName = "Player.Minigame.0";

    private bool isCatching = false;

    private Dictionary<(string, Personality), float> personalityRule;
    private Player player;
    private void Start()
    {
        dialogue = BattleDialogueManager.Instance;
        gameObject.SetActive(false);
        DataComparer();
        player = PlayerManager.Instance.player;
    }

    private void Update()
    {
        if (player.playerKeySetting.TryGetValue(keySettingName, out string path) && isCatching == false)
        {
            string InputControlPath = path;
            var control = InputSystem.FindControl(path);
            if (control is ButtonControl button && button.wasPressedThisFrame)
            {
                isCatching = true;
                CameraController.Instance.Kick(onComplete: () =>
                {
                    rotatePoint.SetRotateSpeed(0);
                    bool result = rotatePoint.isInSuccessZone;
                    resultCallback?.Invoke(result, returnMonster);
                    resultCallback = null;
                    isCatching = false;
                    StartCoroutine(CloseAfterDelay(2f));
                });
            }
        }
    }
    IEnumerator CloseAfterDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        transform.gameObject.SetActive(false);
    }

    //랜덤 범위지정
    private void SetSuccessRanges(float value)
    {
        if (value <= 0) { Debug.Log("SetSuccessRanges의 value가 0이하 입니다."); }

        float min = UnityEngine.Random.Range(-180, 180);
        float max = min + value;
        ranges.Add(new RotationRange(min, max));
    }


    /// <summary>
    /// 미니게임을 생성합니다. 제스처와 몬스터를 받고 성공여부와 사용된 몬스터를 반환합니다.
    /// </summary>
    /// <param name="percent"></param>
    /// <param name="speed"></param>
    public void StartMiniGame(ItemData gesture, Monster targetMonster, Action<bool, Monster> callback)
    {
        transform.gameObject.SetActive(true);
        ranges.Clear();

        float speed;
        float range;
        float hpPercent;


        hpPercent = (float)targetMonster.CurHp / targetMonster.CurMaxHp;

        if (!player.playerBattleTutorialCheck)
        {
            range = 75f;
            speed = 0.9f;
        }
        else
        {
            speed = 1 - (hpPercent * 0.9f);
            range = GetRange(gesture.itemName, targetMonster.personality);
        }

        SetSuccessRanges(range);
        rotatePoint.SetRotateSpeed(speed);
        rotatePoint.SetRanges(ranges);
        Color color;
        if (range > defaultPercent)
        {
            color = appropriateColor;
            dialogue.BattleDialogueAppend(appropriateMassage);
        }
        else if (range == defaultPercent)
        {
            color = defaultColor;
            dialogue.BattleDialogueAppend(defaultMassage);
        }
        else
        {
            color = notAppropriateColor;
            dialogue.BattleDialogueAppend(notAppropriateMassage);
        }
        spawner.SetSuccessColor(color);
        spawner.SpawnRanges(ranges);

        returnMonster = targetMonster;
        resultCallback = callback;
    }

    public float GetRange(string a, Personality b)
    {
        if (personalityRule.TryGetValue((a, b), out float result))
        {

            return result;
        }
        return defaultPercent; // 일치하는 규칙이 없을 경우 반환할 값
    }

    private void DataComparer()
    {
        personalityRule = new Dictionary<(string A, Personality B), float>
        {
             //Persuading
            { ("Persuading", Personality.Thorough), appropriatePercent },
            { ("Persuading", Personality.Decisive), appropriatePercent },
            { ("Persuading", Personality.Responsible), appropriatePercent },

            { ("Persuading", Personality.Bold), notAppropriatePercent },
            { ("Persuading", Personality.Energetic), notAppropriatePercent },
            { ("Persuading", Personality.Proactive), notAppropriatePercent },

            //Scolding
            { ("Scolding", Personality.Bold), appropriatePercent },
            { ("Scolding", Personality.Passionate), appropriatePercent },

            { ("Scolding", Personality.Devoted), notAppropriatePercent },
            { ("Scolding", Personality.Decisive), notAppropriatePercent },
             
            //Boasting
            { ("Boasting", Personality.Proactive), appropriatePercent },

            { ("Boasting", Personality.Thorough), notAppropriatePercent },
            { ("Boasting", Personality.Emotional), notAppropriatePercent },
            { ("Boasting", Personality.Altruistic), notAppropriatePercent },
             
            //Ignoring
            { ("Ignoring", Personality.Cautious), appropriatePercent },
            { ("Ignoring", Personality.Cynical), appropriatePercent },

            { ("Ignoring", Personality.Passionate), notAppropriatePercent },
            { ("Ignoring", Personality.Sociable), notAppropriatePercent },
            
            //Complimenting
            { ("Complimenting", Personality.Devoted), appropriatePercent },
            { ("Complimenting", Personality.Emotional), appropriatePercent },
            { ("Complimenting", Personality.Altruistic), appropriatePercent },

            { ("Complimenting", Personality.Cynical), notAppropriatePercent },

            //Joking
            { ("Joking", Personality.Energetic), appropriatePercent },
            { ("Joking", Personality.Sociable), appropriatePercent },

            { ("Joking", Personality.Cautious), notAppropriatePercent    },
            { ("Joking", Personality.Responsible), notAppropriatePercent },
        };
    }
}
