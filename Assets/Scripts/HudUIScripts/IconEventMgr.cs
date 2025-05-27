using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
public class IconEventMgr : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    public IconType curIconType = IconType.None;
    public CursorUIDlg cursorUIDlg = null;
    private void Start()
    {
        cursorUIDlg = GameMgr.Inst.gameScene.m_hudUI.m_cursorUIDlg;
    }
    public enum IconType
    {
        None = -1,
        HP = 0,
        Attack = 1,
        Level = 2,
        Trust = 3,
        Blood = 4,
    }
    string IconTypeTitle(int i)
    {
        string str = "";
        switch (i)
        {
            case 0:
                str = "체력";
                break;
            case 1:
                str = "공격력";
                break;
            case 2:
                str = "레벨";
                break;
            case 3:
                str = "신뢰도";
                break;
            case 4:
                str = "피";
                break;
        }
        return str;
    }
    string IconTypeInfo(int i)
    {
        string str = "";
        switch (i)
        {
            case 0:
                str = "0이 되면 게임오버된다. 레벨업을 하는 것으로 체력을 회복할 수 있다.";
                break;
            case 1:
                str = "공격의 데미지는 두 숫자 사이의 값으로 결정된다.\n예 : 5-12일 경우, 5와 12 사이의 값중 랜덤한 값이 데미지가 된다.";
                break;
            case 2:
                str = "적을 죽이면 경험치를 얻고, 경험치를 많이 모으면 레벨업을 하여 체력을 늘리거나 공격력을 강화할 수 있다";
                break;
            case 3:
                str = "적을 죽이거나, 좋은 말을 해주는 것으로 신뢰도가 증가한다. 신뢰도가 높으면 공격의 부가효과를 누릴 수도 있다.";
                break;
            case 4:
                str = "플레이어가 입히는 모든 데미지는 피로 환산된다. 게임이 끝났을 때 랭킹의 기준이 된다.";
                break;
        }
        return str;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        cursorUIDlg.OnHoverUI((int)curIconType, IconTypeTitle((int)curIconType),IconTypeInfo((int)curIconType));
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        cursorUIDlg.OnHoverExitUI();
    }
}
