using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MonsterHoverHandler : MonoBehaviour
{
    private GameObject selectImage;
    private bool isSelected = false;

    public void SetUp(GameObject image)
    {
        selectImage = image;
    }

    public void OnMouseEnter()
    {
        if (!isSelected && selectImage != null)
        {
            selectImage.SetActive(true);
        }
    }

    public void OnMouseExit()
    {
        if (!isSelected && selectImage != null)
        {
            selectImage.SetActive(false);
        }
    }

    public void OnMouseDown()
    {
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
