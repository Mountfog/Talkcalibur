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
                str = "ü��";
                break;
            case 1:
                str = "���ݷ�";
                break;
            case 2:
                str = "����";
                break;
            case 3:
                str = "�ŷڵ�";
                break;
            case 4:
                str = "��";
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
                str = "0�� �Ǹ� ���ӿ����ȴ�. �������� �ϴ� ������ ü���� ȸ���� �� �ִ�.";
                break;
            case 1:
                str = "������ �������� �� ���� ������ ������ �����ȴ�.\n�� : 5-12�� ���, 5�� 12 ������ ���� ������ ���� �������� �ȴ�.";
                break;
            case 2:
                str = "���� ���̸� ����ġ�� ���, ����ġ�� ���� ������ �������� �Ͽ� ü���� �ø��ų� ���ݷ��� ��ȭ�� �� �ִ�";
                break;
            case 3:
                str = "���� ���̰ų�, ���� ���� ���ִ� ������ �ŷڵ��� �����Ѵ�. �ŷڵ��� ������ ������ �ΰ�ȿ���� ���� ���� �ִ�.";
                break;
            case 4:
                str = "�÷��̾ ������ ��� �������� �Ƿ� ȯ��ȴ�. ������ ������ �� ��ŷ�� ������ �ȴ�.";
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
