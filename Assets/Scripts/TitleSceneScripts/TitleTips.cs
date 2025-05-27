using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleTips : MonoBehaviour
{
    public Button m_btnNext = null;
    public Button m_btnPrev = null;
    public Button m_btnExitTips = null;
    public TitleSceneDlg m_titleSceneDlg = null;
    public GameObject[] m_panelTips = new GameObject[4];
    int m_tipsNum = 0;

    public void Initialize()
    {
        foreach (GameObject go in m_panelTips)
        {
            HideUI(go);
        }
        m_tipsNum = 0;
        HideUI(m_btnNext.gameObject);
        HideUI(m_btnPrev.gameObject);
        HideUI(m_btnExitTips.gameObject);
        m_btnNext.onClick.AddListener(OnClick_Next);
        m_btnPrev.onClick.AddListener(OnClick_Prev);
        m_btnExitTips.onClick.AddListener(OnClick_HideTips);
    }

    public void HideUI(GameObject go)
    {
        go.SetActive(false);
    }
    public void ActivateUI(GameObject go)
    {
        go.SetActive(true);
    }
    public void OnClick_Tips()
    {
        ActivateUI(this.gameObject);
        m_tipsNum = 0;
        ActivateUI(m_panelTips[m_tipsNum]);
        ActivateUI(m_btnExitTips.gameObject);
        TipsCheck();
    }
    public void OnClick_Next()
    {
        m_titleSceneDlg.PlaySFX(2);
        HideUI(m_panelTips[m_tipsNum]);
        m_tipsNum++;
        ActivateUI(m_panelTips[m_tipsNum]);
        TipsCheck();
    }
    public void OnClick_Prev()
    {
        m_titleSceneDlg.PlaySFX(2);
        HideUI(m_panelTips[m_tipsNum]);
        m_tipsNum--;
        ActivateUI(m_panelTips[m_tipsNum]);
        TipsCheck();
    }
    public void TipsCheck()
    {
        if (m_tipsNum == m_panelTips.Length - 1)
        {
            HideUI(m_btnNext.gameObject);
            ActivateUI(m_btnPrev.gameObject);
        }
        else if (m_tipsNum == 0)
        {
            HideUI(m_btnPrev.gameObject);
            ActivateUI(m_btnNext.gameObject);
        }
        else
        {
            ActivateUI(m_btnNext.gameObject);
            ActivateUI(m_btnPrev.gameObject);
        }
    }
    public void OnClick_HideTips()
    {
        m_titleSceneDlg.PlaySFX(3);
        HideUI(this.gameObject);
        foreach (GameObject go in m_panelTips)
        {
            HideUI(go);
        }
        HideUI(m_btnNext.gameObject);
        HideUI(m_btnPrev.gameObject);
        HideUI(m_btnExitTips.gameObject);
    }
}
