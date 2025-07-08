using UnityEngine;

public abstract class FieldMenuBaseUI : MonoBehaviour
{
    public virtual void Open() => gameObject.SetActive(true);
    public virtual void Close() => gameObject.SetActive(false);
}

