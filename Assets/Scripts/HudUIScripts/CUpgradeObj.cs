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
                str = "������ ����";
                break;
            case 1:
                str = "�������� ����";
                break;
            case 2:
                str = "ġ������ ����";
                break;
            case 3:
                str = "����";
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
                str = "����ġ�� �߰� ȹ���մϴ�.";
                break;
            case 1:
                str = "�ּ� ���� �������� �ణ �����մϴ�.";
                break;
            case 2:
                str = "�ִ� ���� �������� �ణ �����մϴ�.";
                break;
            case 3:
                str = "�ּ� ���� �������� �����ϰ�, �ִ� ���� �������� �����մϴ�.";
                break;
        }
        return str;
    }

}
