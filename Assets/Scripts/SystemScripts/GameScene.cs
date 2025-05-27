using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public GameUI m_gameUI = null;
    public HudUI m_hudUI = null;

    [HideInInspector] public BattleFSM m_battleFSM = new BattleFSM();

    private void Awake()
    {
        Screen.SetResolution(1920, 1080, true);
        AssetMgr.Inst.Initialize();
        GameMgr.Inst.gameScene = this;
        GameMgr.Inst.Initialize();
    }
    private void Start()
    {
        GameMgr.Inst.ginfo.Initialize();
        m_hudUI.Initialize();
        m_battleFSM.Initialize(CB_Ready, CB_Wave, CB_Action, CB_Battle, CB_Result);
        m_battleFSM.SetReadyState();
    }
    void CB_Ready()
    {
        GameMgr.Inst.ginfo.Initialize();
        m_hudUI.SetReadyState();
        m_gameUI.SetReadyState();
    }
    void CB_Wave()
    {
        m_hudUI.SetWaveState();
        m_gameUI.SetWaveState();
    }
    void CB_Action()
    {
        m_hudUI.SetActionState();
        m_gameUI.SetActionState();
    }
    void CB_Battle()
    {
        m_hudUI.SetBattleState();
        m_gameUI.SetBattleState();
    }
    void CB_Result()
    {
        m_hudUI.SetResultState();
    }
    private void Update()
    {
        if (m_battleFSM != null)
            m_battleFSM.OnUpdate();
    }
}
