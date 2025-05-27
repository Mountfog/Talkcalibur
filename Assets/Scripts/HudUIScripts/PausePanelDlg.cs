using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausePanelDlg : MonoBehaviour
{
    public GameObject m_OptionsPanel = null;
    public Button m_btnResume = null;
    public Button m_btnOption = null;
    public Button m_btnTitle = null;
    public Button m_btnRestart = null;

    bool isPause = false;


    public void Initialize()
    {
        m_btnResume.onClick.AddListener(OnClick_Resume);
        m_btnOption.onClick.AddListener(OnClick_Option);
        m_btnTitle.onClick.AddListener(OnClick_Title);
        m_btnRestart.onClick.AddListener(OnClick_Restart);
        m_OptionsPanel.GetComponent<OptionsDlg>().Initialize();
    }
    public void PressedESC()
    {
        if(isPause)
        {
            Time.timeScale = 1.0f;
            gameObject.SetActive(false);
            isPause = false;
            GameMgr.Inst.gameScene.m_gameUI.PlaySFX(4);
        }
        else
        {
            Time.timeScale = 0f;
            gameObject.SetActive(true);
            isPause = true;
            GameMgr.Inst.gameScene.m_gameUI.PlaySFX(3);
        }
    }
    public void OnClick_Resume()
    {
        if (isPause)
        {
            Time.timeScale = 1.0f;
            gameObject.SetActive(false);
            isPause = false;
            GameMgr.Inst.gameScene.m_gameUI.PlaySFX(4);
        }
    }
    public void OnClick_Restart()
    {
        if (isPause)
        {
            Time.timeScale = 1.0f;
            isPause = false;
        }
        GameMgr.Inst.gameScene.m_battleFSM.SetNoneState();
        Invoke(nameof(Restart), 0.05f);
    }
    void Restart()
    {
        gameObject.SetActive(false);
        GameMgr.Inst.gameScene.m_battleFSM.SetReadyState();
    }
    public void OnClick_Option()
    {
        m_OptionsPanel.SetActive(true);
    }
    public void OnClick_Title()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
