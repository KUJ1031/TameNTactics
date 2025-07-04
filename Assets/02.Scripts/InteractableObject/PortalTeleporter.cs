using UnityEngine;
using System.Collections;

public class PortalTeleporter : MonoBehaviour
{
    [Tooltip("이 포탈이 이동시킬 대상 포탈")]
    public Transform targetPortal;

    [Tooltip("다시 이동되는 것을 방지하는 쿨타임")]
    public float teleportCooldown = 1f;

    private bool isTeleporting = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTeleporting && other.CompareTag("Player"))
        {
            StartCoroutine(Teleport(other));
        }
    }

    private IEnumerator Teleport(Collider2D player)
    {
        isTeleporting = true;

        // 순간 이동
        player.transform.position = targetPortal.position;

        // 포탈의 쌍에서도 쿨타임을 걸어줘야 함
        var targetScript = targetPortal.GetComponent<PortalTeleporter>();
        if (targetScript != null)
            targetScript.SetTeleportCooldown();

        yield return new WaitForSeconds(teleportCooldown);
        isTeleporting = false;
    }

    public void SetTeleportCooldown()
    {
        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        isTeleporting = true;
        yield return new WaitForSeconds(teleportCooldown);
        isTeleporting = false;
    }
}
