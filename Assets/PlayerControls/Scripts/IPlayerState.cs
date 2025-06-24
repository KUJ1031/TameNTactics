using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    void OnEnter(PlayerController player);
    void OnExit(PlayerController player);
    void OnHandlelnput(PlayerController player,PlayerinputAction inputs);
    void OnUpdate(PlayerController player);
}
