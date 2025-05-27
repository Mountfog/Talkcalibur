using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameInfo
{
    private int m_stage = 0;
    private int m_enemiesTillBoss = 0;

    public int hostID = 0;
    private int hostHp = 100;
    public int shield = 0;

    public int hostMaxHp = 100;
    public int minHostAttack = 8;
    public int maxHostAttack = 16;
    public int hostTrust = 0;
    public int hostLevel = 0;
    public int EXPNeededForLevelUp = 100;

    public int hostEXP = 0;
    public int extraDamage = 0;


    public int blood = 0;
    public void Initialize()
    {
        m_stage = 1;
        m_enemiesTillBoss = 5;
        blood = 0;
        extraDamage = 0;
    }

    public void SetHostInfo(int khostLevel, int minAtk, int maxAtk, int trust,int khostID, int khostEXP)
    {
        hostID = khostID;
        hostLevel = khostLevel;
        EXPNeededForLevelUp = 30 + (hostLevel * 15);
        hostMaxHp = hostMaxHp = 50 + (hostLevel * 10);
        hostHp = hostMaxHp;
        minHostAttack = minAtk;
        maxHostAttack = maxAtk;
        hostTrust = trust;
        hostEXP = 0;
    }
    public int PlayerDamage()
    {
        int damage = 0;
        damage =  Random.Range((int)minHostAttack , (int)maxHostAttack + 1);
        damage += extraDamage;
        extraDamage = 0;
        return damage;
    }
    public int PlayerDamage(int extraMin,int extraMax)
    {
        int damage = 0;
        damage = Random.Range((int)minHostAttack + extraMin, (int)maxHostAttack + extraMax + 1);
        damage += extraDamage;
        extraDamage = 0;
        return damage;
    }
    public void ShieldAdd(int amount)
    {
        shield+=amount;
    }
    public void DamageHost(int damage)
    {
        if(shield > 0)
        {
            damage -= shield;
        }
        if(damage > 0)
        {
            hostHp -= damage;
        }
        shield = 0;
    }
    public void BloodGained(int kblood)
    {
        blood += kblood;
    }
    public void ExpGained(int exp)
    {
        hostEXP += exp;
        hostTrust = (hostTrust >= 15) ? 15 : ++hostTrust; //수정필요
    }
    public void HostLevelUp()
    {
        hostLevel++;
        hostEXP -= EXPNeededForLevelUp;
        EXPNeededForLevelUp = 30 + (hostLevel * 15);
        hostMaxHp = 50 + (hostLevel * 10);
        hostHp = hostMaxHp;
        if(hostLevel % 2 == 0)
            minHostAttack+=1;
        else
            maxHostAttack+=2;    
        if((hostLevel % 5 == 0))
        {
            maxHostAttack++;
        }
        //체력 업데이트
        //공격 업데이트
        //레벨 업데이트
    }
    public void AttackClean()
    {
        if (minHostAttack < 0) minHostAttack = 0;
        if (minHostAttack > maxHostAttack)
        {
            int j = maxHostAttack;
            maxHostAttack = minHostAttack;
            minHostAttack = j;
        }
    }
    public void NextStage()
    {
        m_stage++;
        m_enemiesTillBoss = 5;
    }
    public void SacrificeHost(int a)
    {
        float f = hostMaxHp * (float)a / 100f;
        int sHP = Mathf.FloorToInt(f);
        hostHp -= sHP;
        extraDamage += sHP;
        blood += sHP;
    }
    public void EnemySpawned()
    {
        m_enemiesTillBoss--;
    }
    public bool IsSacrificePossible(int a)
    {
        float per = (float)hostHp / (float)hostMaxHp * 100;
        return per > a;
    }
    
    public void CheatCode()
    {
        m_enemiesTillBoss = 0;
    }
    public void LastBoss()
    {
        m_enemiesTillBoss = 0;
        m_stage = 3;
    }
    public bool IsLevelUpPossible()
    {
        return hostEXP >= EXPNeededForLevelUp;
    }
    public bool IsBroke()
    {
        return (hostHp <= 0);
    }
    public bool BossSpawnNeeded()
    {
        return m_enemiesTillBoss <= 0;
    }
    public int EnemiesTillBoss { get { return m_enemiesTillBoss; } }
    public int HostHp { get { return (hostHp < 0) ? 0 : hostHp; } }
    public int CurStage { get { return m_stage; } }
}
