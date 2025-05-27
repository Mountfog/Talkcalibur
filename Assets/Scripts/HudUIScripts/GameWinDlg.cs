using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameWinDlg : MonoBehaviour
{
    public Text m_bloodText = null;
    public Button m_btnRestart = null;
    public Button m_btnExit = null;

    public void Initialize()
    {
        m_btnRestart.onClick.RemoveAllListeners();
        m_btnExit.onClick.RemoveAllListeners();
        m_btnRestart.onClick.AddListener(OnClick_Restart);
        m_btnExit.onClick.AddListener(OnClick_Exit);
    }
    public void SetResultState()
    {
        int blood = GameMgr.Inst.ginfo.blood;
        m_bloodText.text = string.Format("수확한 피 : \n{0}",blood);
    }
    public void OnClick_Restart()
    {
        GameMgr.Inst.gameScene.m_battleFSM.SetReadyState();
    }
    public void OnClick_Exit()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
