using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnPointerEnterSoundEffect : MonoBehaviour, IPointerEnterHandler
{
    public TitleSceneDlg titleSceneDlg;
    public void OnPointerEnter(PointerEventData eventData)
    {
        titleSceneDlg.PlaySFX(0);
    }
}
