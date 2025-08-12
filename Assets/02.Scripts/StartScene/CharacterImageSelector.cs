using UnityEngine;
using UnityEngine.UI;

public class CharacterImageSelector : MonoBehaviour
{
    [SerializeField] public Image targetImage; // 이미지에 적용할 테두리 색
    [SerializeField] private Color selectedColor = Color.green;
    [SerializeField] private Color defaultColor = Color.white;
    public int buttonIndex; // 버튼 인덱스 (0: 남캐, 1: 여캐 등)

    public bool IsSelected { get; private set; } = false;

    public StartUI startUI;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => startUI.SelectCharacter(this));
    }

    public void SetSelected(bool selected)
    {
        targetImage.color = selected ? Color.green : Color.white;
    }

    public Image GetImage()
    {
        return targetImage;
    }
}
