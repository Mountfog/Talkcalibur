using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class HudUI : MonoBehaviour
{
    public SelectHostDlg m_selectHostDlg = null;
    public BloodUIDlg m_bloodUIDlg = null;
    public HostInfoDlg m_hostInfoDlg = null;
    public EnemyInfoDlg m_enemyInfoDlg = null;
    public GameOverDlg m_gameOverDlg = null;
    public GameWinDlg m_gameWinDlg = null;
    public TalkToHostDlg m_talkToHostDlg = null;
    public DamageEffectDlg m_damageEffectDlg = null;
    public Button m_btnTurnEnd = null;
    public CursorUIDlg m_cursorUIDlg = null;
    public StageUIDlg m_stageUIDlg = null;
    public UpgradeUIDlg m_upgradeUIDlg = null;
    public PausePanelDlg m_pausePanelDlg = null;
    public OptionsDlg m_optionsDlg = null;
    [SerializeField] private Vector3 turnEndInVec = Vector3.zero;
    [SerializeField] private Vector3 turnEndOutVec = Vector3.zero;
    public Texture2D cursorTex;
    void Awake()
    {
        float xspot = cursorTex.width / 2;
        float yspot = cursorTex.height / 2;
        Vector2 hotSpot = new Vector2(xspot - 30f, yspot - 40f); // 이 값은 알아서 해주셈
        Cursor.SetCursor(cursorTex, hotSpot, CursorMode.ForceSoftware);
    }
    public void Initialize()
    {
        HideUI(m_talkToHostDlg.gameObject);
        HideUI(m_bloodUIDlg.gameObject);
        HideUI(m_hostInfoDlg.gameObject);
        HideUI(m_gameWinDlg.gameObject);
        HideUI(m_gameOverDlg.gameObject);
        HideUI(m_damageEffectDlg.gameObject);
        HideUI(m_enemyInfoDlg.gameObject);
        HideUI(m_upgradeUIDlg.gameObject);
        m_upgradeUIDlg.isSelect = false;
        m_gameOverDlg.Initialize();
        m_gameWinDlg.Initialize();
        m_pausePanelDlg.Initialize();
    }
    public void SetReadyState()
    {
        m_talkToHostDlg.SetReadyState();
        HideUI(m_talkToHostDlg.gameObject);
        HideUI(m_bloodUIDlg.gameObject);
        HideUI(m_hostInfoDlg.gameObject);
        HideUI(m_gameWinDlg.gameObject);
        HideUI(m_gameOverDlg.gameObject);
        HideUI(m_damageEffectDlg.gameObject);
        HideUI(m_enemyInfoDlg.gameObject);
        ActivateUI(m_selectHostDlg.gameObject);
        m_stageUIDlg.SetReadyState();
        HideUI(m_stageUIDlg.gameObject);
        m_selectHostDlg.Initialize();
        ActivateUI(m_cursorUIDlg.gameObject);
        m_cursorUIDlg.Initialize();
        HideUI(m_upgradeUIDlg.gameObject);
        m_upgradeUIDlg.isSelect = false;
        HideUI(m_pausePanelDlg.gameObject);
        m_btnTurnEnd.gameObject.transform.localPosition = turnEndOutVec;
        m_btnTurnEnd.onClick.RemoveAllListeners();
        m_btnTurnEnd.onClick.AddListener(OnClick_TurnEnd);

    }
    public void SetWaveState()
    {
        HideUI(m_selectHostDlg.gameObject);
        ActivateUI(m_bloodUIDlg.gameObject);
        ActivateUI(m_damageEffectDlg.gameObject);
        ActivateUI(m_stageUIDlg.gameObject);
        ActivateUI(m_talkToHostDlg.gameObject);
        m_talkToHostDlg.SetWaveState();
        m_hostInfoDlg.Initialize();
        //m_talkToHostDlg.Initialize();
        m_stageUIDlg.SetWaveState();
        m_damageEffectDlg.Initialize();
        m_enemyInfoDlg.Initialize();
        StartCoroutine(BtnTurnEndEaseOutCor());
        if (m_upgradeUIDlg.isSelect)
        {
            ActivateUI(m_upgradeUIDlg.gameObject);
            m_upgradeUIDlg.Initialize();
        }
        else
        {
            HideUI(m_upgradeUIDlg.gameObject);
        }
    }
    public void SetActionState()
    {
        m_talkToHostDlg.SetActionState();
        
        StartCoroutine(BtnTurnEndEaseInCor());
        m_btnTurnEnd.interactable = false;
    }
    public void SetBattleState()
    {
        StartCoroutine(BtnTurnEndEaseOutCor());
    }
    public void SetResultState()
    {
        Invoke(nameof(Results), 2.0f);
    }
    public void SetTurnEndButton()
    {
        if (m_talkToHostDlg.SentenceFinished())
        {
            m_btnTurnEnd.interactable = true;
        }
        else
        {
            m_btnTurnEnd.interactable = false;
        }
    }
    public void Results()
    {
        HideUI(m_talkToHostDlg.gameObject);
        HideUI(m_bloodUIDlg.gameObject);
        StartCoroutine(BtnTurnEndEaseOutCor());
        HideUI(m_hostInfoDlg.gameObject);
        HideUI(m_damageEffectDlg.gameObject);
        HideUI(m_enemyInfoDlg.gameObject);
        HideUI(m_selectHostDlg.gameObject);
        if (GameMgr.Inst.ginfo.IsBroke())
        {
            GameMgr.Inst.gameScene.m_gameUI.PlaySFX(13);
            m_gameOverDlg.SetResultState();
            ActivateUI(m_gameOverDlg.gameObject);
        }
        else
        {
            GameMgr.Inst.gameScene.m_gameUI.PlaySFX(14);
            m_gameWinDlg.SetResultState();
            ActivateUI(m_gameWinDlg.gameObject);
        }
    }
    public void HideUI(GameObject go)
    {
        go.SetActive(false);
    }
    public void ActivateUI(GameObject go)
    {
        go.SetActive(true);
    }

    public void OnClick_TurnEnd()
    {
        if (m_talkToHostDlg.SentenceFinished())
        {
            m_btnTurnEnd.interactable = false;
            GameMgr.Inst.gameScene.m_battleFSM.SetBattleState();
        }
    }
    public void Revive()
    {

    }
    IEnumerator BtnTurnEndEaseInCor()
    {
        float currentTime = 0;
        float lerpTime = 0.5f;
        float transY = m_btnTurnEnd.transform.localPosition.y;
        while (currentTime != lerpTime)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= lerpTime)
            {
                currentTime = lerpTime;
            }
            float t = currentTime / lerpTime;
            t = EaseOutExpo(t);
            float moveTo = Mathf.Lerp(transY, turnEndInVec.y, t);
            m_btnTurnEnd.transform.localPosition = new Vector3(768, moveTo, 0);
            yield return null;
        }
        yield return null;
    }
    IEnumerator BtnTurnEndEaseOutCor()
    {
        float currentTime = 0;
        float lerpTime = 0.5f;
        float transY = m_btnTurnEnd.transform.localPosition.y;
        while (currentTime != lerpTime)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= lerpTime)
            {
                currentTime = lerpTime;
            }
            float t = currentTime / lerpTime;
            t = EaseOutExpo(t);
            float moveTo = Mathf.Lerp(transY, turnEndOutVec.y, t);
            m_btnTurnEnd.transform.localPosition = new Vector3(768, moveTo, 0);
            yield return null;
        }
        yield return null;
    }
    float EaseOutExpo(float x)
    {
        return 1 - (1 - x) * (1 - x);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            m_pausePanelDlg.PressedESC();
        }
    }
}
