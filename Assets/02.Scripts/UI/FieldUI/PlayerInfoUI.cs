using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : FieldMenuBaseUI
{
    [SerializeField] private Image PlayerImage;
    [SerializeField] private TextMeshProUGUI PlayerNameText;
    [SerializeField] private TextMeshProUGUI PlayTimeText;
    [SerializeField] private TextMeshProUGUI CatchMonsterText;
    [SerializeField] private TextMeshProUGUI EquipItemText;
    [SerializeField] private TextMeshProUGUI CurrenAreaText;
    [SerializeField] private Button closeMenuButton;
    private void Awake()
    {
        closeMenuButton.onClick.AddListener(OnClickCloseMenuButton);
    }

    public void OnClickCloseMenuButton()
    {
        FieldUIManager.Instance.CloseAllUI();
    }

    private void OnEnable()
    {

        var player = PlayerManager.Instance.player;
        PlayerImage.sprite = PlayerManager.Instance.playerImage[player.playerGender];
        PlayerNameText.text = player.playerName;
        CatchMonsterText.text = $"{player.ownedMonsters.Count}명";
        CurrenAreaText.text = $"{player.playerLastStage}";
        EquipItemText.text = (player.playerEquipment.Count > 0 && player.playerEquipment[0]?.data != null) ? $"<color=#FF4444>{player.playerEquipment[0].data.itemName}</color> ({player.playerEquipment[0].data.description})" : "<color=#888888>없음</color>";
    }
}
