using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MonsterHoverHandler : MonoBehaviour
{
    private GameObject selectImage;
    private bool isSelected = false;

    private BattleUIManager battleUIManager;

    private Monster monster;

    private void Start()
    {
        battleUIManager = UIManager.Instance.battleUIManager;
        monster = GetComponent<MonsterCharacter>().monster;
    }

    public void SetUp(GameObject image)
    {
        selectImage = image;
    }

    private bool IsValideHoverTarget()
    {
        if (SceneManager.GetActiveScene().name != "BattleScene") return false;

        if (!battleUIManager.CanHoverSelect) return false;

        if (battleUIManager.CanHoverSelect && battleUIManager.CurrentHoverTarget.Contains(monster))
        {
            return true;
        }

        return false;
    }

    public void OnMouseEnter()
    {
        if (!IsValideHoverTarget()) return;

        if (battleUIManager.CurrentHoverTarget.Contains(monster))
        {
            if (!isSelected && selectImage != null)
            {
                selectImage.SetActive(true);
            }
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

        if (battleUIManager.CurrentHoverTarget.Contains(monster))
        {
            if (selectImage != null)
            {
                selectImage.SetActive(true);
            }
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
