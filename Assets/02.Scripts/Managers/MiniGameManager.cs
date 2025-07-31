using System;
using System.Collections;
using System.Collections.Generic;
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

    private bool isCatting = false;
    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (PlayerManager.Instance.player.playerKeySetting.TryGetValue(keySettingName, out string path) && isCatting == false)
        {
            string InputControlPath = path;
            var control = InputSystem.FindControl(path);
            if (control is ButtonControl button && button.wasPressedThisFrame)
            {
                isCatting = true;
                CameraController.Instance.Kick(onComplete: () =>
                {
                    rotatePoint.SetRotateSpeed(0);
                    bool result = rotatePoint.isInSuccessZone;
                    resultCallback?.Invoke(result, returnMonster);
                    resultCallback = null;
                    isCatting = false;
                    if (PlayerManager.Instance.player.playerTutorialCheck)
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
    /// 미니게임을 생성합니다. 몬스터를 받고 성공여부와 사용된 몬스터를 반환합니다.
    /// </summary>
    /// <param name="percent"></param>
    /// <param name="speed"></param>
    public void StartMiniGame(Monster targetMonster, Action<bool, Monster> callback)
    {
        transform.gameObject.SetActive(true);
        ranges.Clear();

        float speed;
        float range;
        float hpPercent;

        //Debug.Log("현재 체력 : " + targetMonster.CurHp + "최대 체력 : " + targetMonster.CurMaxHp);
        hpPercent = (float)targetMonster.CurHp / targetMonster.CurMaxHp;
        speed = 1 - (hpPercent * 0.9f);

        range = 30;//추후 제스쳐 확인 후 범위 수정 추가
        //Debug.Log("몬스터 체력퍼 : " + hpPercent + "속도 : " + speed);
        SetSuccessRanges(range);
        rotatePoint.SetRotateSpeed(speed);
        rotatePoint.SetRanges(ranges);
        spawner.SpawnRanges(ranges);

        returnMonster = targetMonster;
        resultCallback = callback;
    }

    /// <summary>
    /// 튜토리얼용 미니게임입니다. 속도가 지정되어 있습니다.
    /// </summary>
    /// <param name="percent"></param>
    /// <param name="speed"></param>
    public void StartMiniGame(Monster targetMonster, Action<bool, Monster> callback, float customSpeed)
    {
        transform.gameObject.SetActive(true);
        ranges.Clear();

        float range;

        range = 75;
        SetSuccessRanges(range);
        rotatePoint.SetRotateSpeed(customSpeed);
        rotatePoint.SetRanges(ranges);
        spawner.SpawnRanges(ranges);

        returnMonster = targetMonster;
        resultCallback = callback;
    }

}
