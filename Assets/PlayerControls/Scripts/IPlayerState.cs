using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IPlayerState
{
    void OnEnter(PlayerController player);
    void OnExit(PlayerController player);
    void OnHandlelnput(PlayerController player);
    void OnUpdate(PlayerController player);
}
