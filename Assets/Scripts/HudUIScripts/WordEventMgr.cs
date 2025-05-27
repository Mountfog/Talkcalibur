using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WordEventMgr : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int wordId = 0;
    public CursorUIDlg cursorUIDlg = null;
    private void Start()
    {
        cursorUIDlg = GameMgr.Inst.gameScene.m_hudUI.m_cursorUIDlg;
    }

    string WordInfo(int i)
    {
        AssetWord word = AssetMgr.Inst.GetAssetWord(i);
        string str = "word";
        return str;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cursorUIDlg.OnHoverWord(wordId);
        gameObject.GetComponent<CWordObj>().OnHover();
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        cursorUIDlg.OnExitWord();
        gameObject.GetComponent<CWordObj>().OnHoverExit();
    }
}
