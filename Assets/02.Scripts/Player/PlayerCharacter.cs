using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private Player playerData;
    public void Init(Player data)
    {
        if (data == null)
        {
            return;
        }
        playerData = data;
    }
}
