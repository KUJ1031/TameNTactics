using UnityEngine;
using System.Collections;

public class PortalTeleporter : MonoBehaviour
{
    [Header("이 포탈이 순간이동시킬 목표 포탈 위치")]
    public Transform targetPortal;

    [Header("같은 포탈로 반복 순간이동을 방지하기 위한 쿨타임 (초)")]
    public float teleportCooldown = 1f;

    // 현재 포탈이 사용 중인지 여부 (쿨타임 상태)
    private bool isTeleporting = false;

    /// 플레이어가 포탈에 들어왔을 때 호출
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어이고, 쿨타임이 아닐 경우에만 텔레포트 실행
        if (!isTeleporting && other.CompareTag("Player"))
        {
            StartCoroutine(Teleport(other));
        }
    }

    // 플레이어를 목표 포탈 위치로 순간이동시키는 코루틴
    private IEnumerator Teleport(Collider2D player)
    {
        isTeleporting = true;

        // 플레이어 위치를 타겟 포탈 위치로 즉시 이동
        player.transform.position = targetPortal.position;

        // 목표 포탈 쪽에도 쿨타임 적용 (되돌아오는 루프 방지)
        var targetScript = targetPortal.GetComponent<PortalTeleporter>();
        if (targetScript != null)
        {
            targetScript.SetTeleportCooldown();
        }

        // 현재 포탈도 일정 시간 동안 비활성화
        yield return new WaitForSeconds(teleportCooldown);
        isTeleporting = false;
    }

    // 외부에서 호출해 포탈에 쿨타임을 적용
    // (다른 포탈에서 텔레포트 되었을 때 호출됨)
    public void SetTeleportCooldown()
    {
        StartCoroutine(CooldownRoutine());
    }


    // 일정 시간 동안 이 포탈을 비활성화 상태로 만드는 코루틴
    private IEnumerator CooldownRoutine()
    {
        isTeleporting = true;
        yield return new WaitForSeconds(teleportCooldown);
        isTeleporting = false;
    }
}
