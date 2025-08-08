using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionHold : MonoBehaviour
{
    private bool _active;
    private Vector3 _holdPos;

    public void HoldHere()
    {
        _holdPos = transform.position;
        _active = true;
    }

    public void Release()
    {
        _active = false;
    }

    private void LateUpdate()
    {
        if (_active)
        {
            transform.position = _holdPos;
        }
    }
}
