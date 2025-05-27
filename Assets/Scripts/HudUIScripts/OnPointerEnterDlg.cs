using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnPointerEnterDlg : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler 
{
    public GameUI gameUI;
    // Start is called before the first frame update
    void Start()
    {
        gameUI = GameMgr.Inst.gameScene.m_gameUI;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameUI.PlaySFX(6);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        gameUI.PlaySFX(7);
    }
}
