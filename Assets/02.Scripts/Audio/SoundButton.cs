using UnityEngine;
using UnityEngine.EventSystems;

public class SoundButton : MonoBehaviour,  IPointerClickHandler
{
    //public string hoverSoundName = "Hover";
    public string clickSoundName = "Click";

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    AudioManager.Instance.PlaySFX(hoverSoundName);
    //}

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(clickSoundName);
    }

}
