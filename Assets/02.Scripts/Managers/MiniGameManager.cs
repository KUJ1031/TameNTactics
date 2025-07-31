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


    private Action<bool, Monster> resultCallback;
    private Monster returnMonster;

    private string keySettingName = "Player.Minigame.0";

    private bool isCatching = false;

    private Dictionary<(string, Personality), float> personalityRule;
    private Player player;
    private void Start()
    {
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
        else { 
            speed = 1 - (hpPercent * 0.9f);
            range = 30;
            //range = GetRange(gesture.itemName, targetMonster.personality);
        }

        SetSuccessRanges(range);
        rotatePoint.SetRotateSpeed(speed);
        rotatePoint.SetRanges(ranges);
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
        return 30f; // 일치하는 규칙이 없을 경우 반환할 값
    }

    private void DataComparer()
    {
        personalityRule = new Dictionary<(string A, Personality B), float>
        {
             //Persuading
            { ("Persuading", Personality.Thorough), 50f },
            { ("Persuading", Personality.Decisive), 50f },
            { ("Persuading", Personality.Responsible), 50f },

            { ("Persuading", Personality.Bold), 10f },
            { ("Persuading", Personality.Energetic), 10f },
            { ("Persuading", Personality.Proactive), 10f },

            //Scolding
            { ("Scolding", Personality.Bold), 50f },
            { ("Scolding", Personality.Passionate), 50f },

            { ("Scolding", Personality.Devoted), 10f },
            { ("Scolding", Personality.Decisive), 10f },
             
            //Boasting
            { ("Boasting", Personality.Proactive), 50f },

            { ("Boasting", Personality.Thorough), 10f },
            { ("Boasting", Personality.Emotional), 10f },
            { ("Boasting", Personality.Altruistic), 10f },
             
            //Ignoring
            { ("Ignoring", Personality.Cautious), 50f },
            { ("Ignoring", Personality.Cynical), 50f },

            { ("Ignoring", Personality.Passionate), 10f },
            { ("Ignoring", Personality.Sociable), 10f },
            
            //Complimenting
            { ("Complimenting", Personality.Devoted), 50f },
            { ("Complimenting", Personality.Emotional), 50f },
            { ("Complimenting", Personality.Altruistic), 50f },

            { ("Complimenting", Personality.Cynical), 10f },

            //Joking
            { ("Joking", Personality.Energetic), 50f },
            { ("Joking", Personality.Sociable), 50f },

            { ("Joking", Personality.Cautious), 10f },
            { ("Joking", Personality.Responsible), 10f },
        };
    }
}
