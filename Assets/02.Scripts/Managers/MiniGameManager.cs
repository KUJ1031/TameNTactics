using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
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

    public string keySettingName = "Player.Minigame.0";


    private void Update()
    {

        //여기서 확인

            if (true)
            {
                rotatePoint.SetRotateSpeed(0);
                bool result = rotatePoint.isInSuccessZone;
                resultCallback?.Invoke(result,returnMonster);
                resultCallback = null;
                transform.gameObject.SetActive(false);
            }

        
    }

    //랜덤 범위지정
    private void SetSuccessRanges(float value)
    {
        if (value <= 0) { Debug.Log("SetSuccessRanges의 value가 0이하 입니다."); }

        float min = UnityEngine.Random.Range(-180, 180);
        float max = min + value;
        ranges.Add(new RotationRange(min, max));
    }

    //범위 0~100
    /// <summary>
    /// 미니게임을 생성합니다. 몬스터를 받고 성공여부와 사용된 몬스터를 반환합니다.
    /// </summary>
    /// <param name="percent"></param>
    /// <param name="speed"></param>
    public void StartMiniGame(Monster targetMonster, Action<bool,Monster> callback)
    {
        transform.gameObject.SetActive(true);
        ranges.Clear();

        //확률 계산
        //float p = percent / 100f * 360f;
        //SetSuccessRanges(p);
        SetSuccessRanges(10);
        rotatePoint.SetRotateSpeed(1);
        rotatePoint.SetRanges(ranges);
        spawner.SpawnRanges(ranges);

        // 콜백 저장
        resultCallback = callback;
    }
}
