using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverDlg : MonoBehaviour
{
    public Text m_gradeText = null;
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
        m_bloodText.text = string.Format("��Ȯ�� �� : \n{0}", blood);
        m_gradeText.text = GradeString(blood);
    }
    public void OnClick_Restart()
    {
        GameMgr.Inst.gameScene.m_battleFSM.SetReadyState();
    }
    public void OnClick_Exit()
    {
        SceneManager.LoadScene("TitleScene");
    }
    string GradeString(int krand)
    {
        string str = "None";
        if (krand <= 400) // 0~3
            str = "�̰͵� �Ƕ�� ��ƿԳ�";
        else if (krand <= 800) //4~6
            str = "�� ��� ������ �ǰڳ�";
        else if (krand <= 1200) //7~9
            str = "������ �ʳ�";
        else if (krand <= 1600) //10~12
            str = "���ݸ� �� ������ �׷���";
        else if (krand <= 2000) //13~15
            str = "�װ� �̰��";
        return str;
    }
}
