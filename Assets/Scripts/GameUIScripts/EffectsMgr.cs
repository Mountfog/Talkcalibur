using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsMgr : MonoBehaviour
{
    public Animator m_effectPlayerBlood = null;
    public Animator m_effectEnemyBlood = null;
    public Animator m_effectPlayerAttack = null;
    public Animator m_effectEnemyAttack = null;
    public Animator m_effectShield = null;
    public Animator m_effectBossAttack = null;
    public void ShieldEffect()
    {
        m_effectShield.SetTrigger("shieldTrig");
    }
    public void ShieldCloseEffect()
    {
        m_effectShield.SetTrigger("shieldCloseTrig");
    }
    void PlayerBlood()
    {
        int krand = Random.Range(0, 5);
        m_effectPlayerBlood.SetInteger("bloodrand", krand);
        m_effectPlayerBlood.SetTrigger("bloodTrigger");
    }
    void EnemyBlood()
    {
        int krand = Random.Range(0, 5);
        m_effectEnemyBlood.SetInteger("bloodrand", krand);
        m_effectEnemyBlood.SetTrigger("bloodTrigger");
    }
    public void PlayerBloodEffect()
    {
        Invoke(nameof(PlayerBlood), 0.2f);
    }
    public void EnemyBloodEffect()
    {
        Invoke(nameof(EnemyBlood), 0.2f);
    }
    public void BossAttackEffect()
    {
        m_effectBossAttack.SetInteger("boss", GameMgr.Inst.ginfo.CurStage - 1);
        m_effectBossAttack.SetTrigger("bomb");
        Invoke(nameof(Soundadsf), 0.12f);
    }
    public void Soundadsf()
    {
        GameMgr.Inst.gameScene.m_gameUI.PlaySFX(12);
    }
    public void PlayerAttackEffect(int attackType, int attackRand)
    {
        m_effectPlayerAttack.SetInteger("attackType", attackType);
        m_effectPlayerAttack.SetInteger("slashRand", attackRand);
        m_effectPlayerAttack.SetTrigger("attacktrig");
    }






}
