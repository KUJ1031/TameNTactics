using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum HoverTargetType
{
    None,
    PlayerTeam,
    EnemyTeam
}

public class MonsterHoverHandler : MonoBehaviour
{
    private GameObject selectImage;
    private bool isSelected = false;

    private BattleUIManager battleUIManager;

    private MonsterCharacter monsterCharacter;

    private void Start()
    {
        battleUIManager = UIManager.Instance.battleUIManager;
        monsterCharacter = GetComponent<MonsterCharacter>();
    }

    public void SetUp(GameObject image)
    {
        selectImage = image;
    }

    private bool IsValideHoverTarget()
    {
        if (SceneManager.GetActiveScene().name != "BattleScene") return false;

        if (!battleUIManager.CanHoverSelect) return false;

        if (battleUIManager.CurrentHoverTarget == HoverTargetType.PlayerTeam &&
            BattleManager.Instance.BattleEntryTeam.Contains(monsterCharacter.monster))
        {
            return true;
        }

        if (battleUIManager.CurrentHoverTarget == HoverTargetType.EnemyTeam &&
            BattleManager.Instance.BattleEnemyTeam.Contains(monsterCharacter.monster))
        {
            return true;
        }

        return false;
    }

    public void OnMouseEnter()
    {
        bool isPossibleAct = BattleManager.Instance.possibleActPlayerMonsters.Contains(monsterCharacter.monster);
        
        if (!IsValideHoverTarget() && !isPossibleAct) return;

        if (!isSelected && selectImage != null && isPossibleAct)
        {
            selectImage.SetActive(true);
        }
    }

    public void OnMouseExit()
    {
        if (!IsValideHoverTarget()) return;

        if (!isSelected && selectImage != null)
        {
            selectImage.SetActive(false);
        }
    }

    public void OnMouseDown()
    {
        if (!IsValideHoverTarget()) return;

        isSelected = true;

        if (selectImage != null)
        {
            selectImage.SetActive(true);
        }
    }

    public void Deselect()
    {
        isSelected = false;

        if (selectImage != null)
        {
            selectImage.SetActive(false);
        }
    }
}
