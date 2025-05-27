using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CUpgradeObj : MonoBehaviour
{
    public delegate void DelegateFunc(int kselect);
    public DelegateFunc m_onBtnClick = null;
    public Image m_iconImage = null;
    public Text m_txtTitle = null;
    public Text m_txtDesc = null;
    public int icon = -1;

    public void Initialize(int rand, int k)
    {
        icon = rand;
        m_txtTitle.text = TitleString(rand);
        m_txtDesc.text = DescString(rand);
        gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        gameObject.GetComponent<Button>().onClick.AddListener(() => OnClick_Host(k));
    }
    public void AddLinstner(DelegateFunc func)
    {
        m_onBtnClick = new DelegateFunc(func);
    }
    private void OnClick_Host(int k)
    {
        if (m_onBtnClick != null)
            m_onBtnClick(k);
    }

    string TitleString(int rand)
    {
        string str = "";
        switch (rand)
        {
            case 0:
                str = "지식의 습득";
                break;
            case 1:
                str = "안정적인 공격";
                break;
            case 2:
                str = "치명적인 공격";
                break;
            case 3:
                str = "광기";
                break;
        }
        return str;
    }
    string DescString(int rand)
    {
        string str = "";
        switch (rand)
        {
            case 0:
                str = "경험치를 추가 획득합니다.";
                break;
            case 1:
                str = "최소 공격 데미지가 약간 증가합니다.";
                break;
            case 2:
                str = "최대 공격 데미지가 약간 증가합니다.";
                break;
            case 3:
                str = "최소 공격 데미지가 감소하고, 최대 공격 데미지가 증가합니다.";
                break;
        }
        return str;
    }

}
