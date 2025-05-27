using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFSM 
{
    public delegate void DelegateFunc();

    public class CState
    {
        public DelegateFunc m_OnEnterFunc = null;
        public DelegateFunc m_OnExitFunc = null;

        public virtual void Initialize(DelegateFunc func) {
            m_OnEnterFunc = new DelegateFunc(func);
        }
        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnExit() { }
    }

    public class CReadyState : CState
    {
        public override void OnEnter()
        {
            if (m_OnEnterFunc != null)
                m_OnEnterFunc();
        }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }
    public class CWaveState : CState
    {
        public override void OnEnter(){
            if (m_OnEnterFunc != null)
                m_OnEnterFunc();
        }
        public override void OnUpdate()   {     }
        public override void OnExit()     {     }

    }
    public class CActionState : CState
    {
        public override void OnEnter()
        {
            if (m_OnEnterFunc != null)
                m_OnEnterFunc();
        }
        public override void OnUpdate() { }
        public override void OnExit() { }

    }
    public class CBattleState : CState
    {
        public override void OnEnter()
        {
            if (m_OnEnterFunc != null)
                m_OnEnterFunc();
        }
        public override void OnUpdate() { }
        public override void OnExit() {
            if (m_OnExitFunc != null)
                m_OnExitFunc();
        }

    }
    public class CResultState : CState
    {
        public override void OnEnter()
        {
            if (m_OnEnterFunc != null)
                m_OnEnterFunc();
        }
        public override void OnUpdate() { }
        public override void OnExit() { }
    }


    private CState m_curState = null;
    private CState m_newState = null;

    private CState m_kReady = new CReadyState();
    private CState m_kWave = new CWaveState();
    private CState m_kAction = new CActionState();
    private CState m_kBattle = new CBattleState();
    private CState m_kResult = new CResultState();


    public void Initialize( DelegateFunc kReady, DelegateFunc kWave,DelegateFunc kAction, DelegateFunc kBattle, DelegateFunc kResult)
    {
        m_kReady.Initialize(kReady);
        m_kWave.Initialize(kWave);
        m_kAction.Initialize(kAction);
        m_kBattle.Initialize(kBattle);
        m_kResult.Initialize(kResult);
    }

    // 상태 변화 셋팅
    public void SetState( CState kState )
    {
        m_newState = kState;
    }

    // 업데이트
    public void OnUpdate()
    {
        if (m_newState != null)
        {
            if (m_newState != m_curState)
            {
                if (m_curState != null)
                    m_curState.OnExit();

                m_curState = m_newState;
                m_newState = null;
                m_curState.OnEnter();
            }
            else
            {
                m_newState = null;
            }
        }

        if (m_curState != null)
            m_curState.OnUpdate();
    }


    public void SetReadyState()    {
        SetState(m_kReady);
    }
    public void SetWaveState()    {
        SetState(m_kWave);
    }
    public void SetActionState()
    {
        SetState(m_kAction);
    }
    public void SetBattleState()    {
        SetState(m_kBattle);
    }
    public void SetResultState()   {
        SetState(m_kResult);
    }

    
    public bool IsCurState( CState kState )
    {
        if (m_curState == null) 
            return false;

        return (m_curState == kState) ? true : false;
    }


    public CState GetCurState() {
        return m_curState;
    }

    public void SetNoneState()
    {
        m_newState = null;
        m_curState = null;
    }

    public bool IsWaveState()
    {
        return m_curState == m_kWave;
    }
    public bool IsActionState()
    {
        return m_curState == m_kAction;
    }
    public bool IsResultState(){
        return (m_curState == m_kResult);
    }
    public bool IsBattleState(){
        return (m_curState == m_kBattle);
    }
    public bool IsReadyState(){
        return (m_curState == m_kReady);
    }

    public bool IsGameState()
    {
        return IsWaveState() || IsBattleState() || IsActionState();
    }

    public bool IsNoneState(){
        return (m_curState == null);
    }


    public void Initialize_GameExitFunc(DelegateFunc func)
    {
        m_kBattle.m_OnExitFunc = new DelegateFunc(func);  
    }
}
