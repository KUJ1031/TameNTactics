[System.Serializable]
public class DialogueNode
{
    public int ID;
    public string Speaker;
    public string Text;
    public string Choice1;
    public int Choice1Next;
    public string Choice2;
    public int Choice2Next;
    public string Choice3;
    public int Choice3Next;
    public int Next; // -1이면 종료

    public string EventKey; // 이벤트 키 추가 (선택적으로 입력 가능)

}