using UnityEngine;

public class DropItem : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ItemManager.Instance.item = this.gameObject;
            Debug.Log($"아이템 획득 가능: {ItemManager.Instance.item.name}");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ItemManager.Instance.item = null;

            if (ItemManager.Instance.item != null)
                Debug.Log($"아이템 획득 불가: {ItemManager.Instance.item.name}");
            else
                Debug.Log($"아이템 획득 불가: null");
        }
    }
}
