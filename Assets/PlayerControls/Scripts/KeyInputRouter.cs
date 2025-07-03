using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyInputRouter : MonoBehaviour
{
    private InputAction Interaction;
    private InputAction Inventory;
    private InputAction Menu;
    private InputAction Minigam;
    private InputAction Select;
    private InputAction Map;
    private InputAction Playerinformation;
    private InputAction Encyclopedia;
    private InputAction Holdinglist;
    private InputAction Currentlist;
    public InputActionAsset inputActions;

    private void OnEnable()
    {
        var map = inputActions.FindActionMap("Player");
        Interaction = map.FindAction("Interaction");
        Inventory = map.FindAction("Inventory");
        Menu = map.FindAction("Menu");
        Minigam = map.FindAction("Minigam");
        Select = map.FindAction("Select");
        Map = map.FindAction("Map");
        Playerinformation = map.FindAction("Playerinformation");
        Encyclopedia = map.FindAction("Encyclopedia");
        Holdinglist = map.FindAction("Holdinglist");
        Currentlist = map.FindAction("Currentlist");
        Interaction.Enable();
        Inventory.Enable();
        Menu.Enable();
        Minigam.Enable();
        Select.Enable();
    }

    private void Interactionkey()
    {
        if(Interaction.triggered)
        {
            Debug.Log("상호작용");
        }
    }

    private void Inventorykey()
    {
        if (Inventory.triggered)
        {
            Debug.Log("인벤토리");
        }
    }

    private void Menukey()
    {
        if (Menu.triggered)
        {
            Debug.Log("메뉴 등 설정");
        }
    }

    private void Minigamkey()
    {
        if (Minigam.triggered)
        {
            Debug.Log("미니게임");
        }
    }

    private void Selectkey()
    {
        if (Select.triggered)
        {
            Debug.Log("선택");
        }
    }

    private void Mapkey()
    {
        if (Map.triggered)
        {
            Debug.Log("지도");
        }
    }

    private void Playerinformationkey()
    {
        if (Playerinformation.triggered)
        {
            Debug.Log("플레이어 정보");
        }
    }

    private void Encyclopediakey()
    {
        if (Encyclopedia.triggered)
        {
            Debug.Log("도감");
        }
    }

    private void Holdinglistkey()
    {
        if (Holdinglist.triggered)
        {
            Debug.Log("보유 몬스터 목록");
        }
    }

    private void Currentlistkey()
    {
        if (Currentlist.triggered)
        {
            Debug.Log("현재 몬스터 목록");
        }
    }

    private void Update()
    {
        Interactionkey();
        Inventorykey();
        Menukey();
        Minigamkey();
        Selectkey();
        Mapkey();
        Playerinformationkey();
        Encyclopediakey();
        Holdinglistkey();
        Currentlistkey();
    }
}
