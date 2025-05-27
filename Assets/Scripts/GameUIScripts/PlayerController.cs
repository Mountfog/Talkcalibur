using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] SpriteRenderer hostSprite = null;
    [SerializeField] float attackForce = 5f;

    public SpriteRenderer m_hpBar = null;
    public List<Sprite> m_hpBarSprites = new List<Sprite>();
    public ParticleSystem m_effectSacrifice = null;
    public ParticleSystem m_effectLevelUp = null;
    public ParticleSystem m_effectTalk = null;
    public Image m_blackFade = null;
    

    public void SetReadyState()
    {
        m_effectSacrifice.gameObject.SetActive(false);
        m_effectLevelUp.gameObject.SetActive(false);
        m_effectTalk.gameObject.SetActive(false);
        m_blackFade.gameObject.SetActive(false);
        transform.position = new Vector3(-11.55f,-1.8f,0);
        m_hpBar.sprite = m_hpBarSprites[0];
        StartCoroutine(ReadyCor());
    }
    public void SetActionState()
    {

    }
    public void SetBattleState()
    {
        Enemy kenemy = GameMgr.Inst.gameScene.m_gameUI.m_enemyMgr.m_curEnemy;
        string firststr = GameMgr.Inst.gameScene.m_hudUI.m_talkToHostDlg.firstWord;
        string secondstr = GameMgr.Inst.gameScene.m_hudUI.m_talkToHostDlg.secondWord;
        AssetWord firstAssetWord = AssetMgr.Inst.GetAssetWord(firststr);
        AssetWord secondAssetWord = AssetMgr.Inst.GetAssetWord(secondstr);
        bool isAttack = firstAssetWord.wordType == 1 || secondAssetWord.wordType == 1;
        bool isShield = firstAssetWord.wordType == 2 || secondAssetWord.wordType == 2;
        if (isAttack)
        {
            GetComponentInChildren<Animator>().SetTrigger("playerAttack");
            int rand = (firstAssetWord.wordType == 1) ? firstAssetWord.id : secondAssetWord.id;
            StartCoroutine(AttackCor(rand));
        }
        if (isShield)
        {
            GameMgr.Inst.gameScene.m_gameUI.effectsMgr.ShieldEffect();
        }
        kenemy.EnemyAttack();
        StartCoroutine(DamageCalculate(kenemy));
    }
    public void Initialize()
    {

    }
    public void SetHostSprite()
    {
        m_hpBar.gameObject.SetActive(true);
        int hostID = GameMgr.Inst.ginfo.hostID;
        hostSprite.sprite = GameMgr.Inst.gameScene.m_gameUI.m_hostSprites[hostID];
        GetComponentInChildren<Animator>().SetTrigger("isRevive");
    }
    IEnumerator ReadyCor()
    {
        yield return new WaitUntil(()=>GameMgr.Inst.gameScene.m_battleFSM.IsWaveState());
        SetHostSprite();
        StartCoroutine(MoveCor());
        float currentTime = 0;
        float lerpTime = 4f;
        float transX = transform.position.x;
        while (currentTime != lerpTime)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= lerpTime)
            {
                currentTime = lerpTime;
            }
            float t = currentTime / lerpTime;
            t = EaseOutExpo(t);
            float moveTo = Mathf.Lerp(transX, -7f, t);
            float kmove = moveTo - transform.position.x;
            transform.Translate(new Vector3(kmove, 0, 0), Space.World);
            yield return null;
        }
        yield return null;
    }
    IEnumerator MoveCor()
    {
        while (GameMgr.Inst.gameScene.m_battleFSM.IsGameState())
        {
            if (GameMgr.Inst.gameScene.m_battleFSM.IsWaveState())
            {
                float rand = Random.Range(0.1f, 0.3f);
                transform.Translate(0, rand, 0, Space.World);
                yield return new WaitForSeconds(0.1f);
                transform.Translate(0, rand * -1, 0, Space.World);
                GameMgr.Inst.gameScene.m_gameUI.PlaySFX(11,0.4f);
                yield return new WaitForSeconds(0.1f);
            }
            yield return null;
        }
        yield return null;
    }
    IEnumerator DamageCalculate(Enemy kenemy)
    {
        yield return new WaitForSeconds(0.5f);
        HudUI khud = GameMgr.Inst.gameScene.m_hudUI;
        string firststr = khud.m_talkToHostDlg.firstWord;
        string secondstr = khud.m_talkToHostDlg.secondWord;
        AssetWord firstAssetWord = AssetMgr.Inst.GetAssetWord(firststr);
        AssetWord secondAssetWord = AssetMgr.Inst.GetAssetWord(secondstr);
        GameInfo gInfo = GameMgr.Inst.ginfo;
        bool isAttack = firstAssetWord.wordType == 1 || secondAssetWord.wordType == 1;
        bool isShield = firstAssetWord.wordType == 2 || secondAssetWord.wordType == 2;
        if (isAttack)
        {
            GameMgr.Inst.gameScene.m_gameUI.effectsMgr.EnemyBloodEffect();
            GameMgr.Inst.gameScene.m_gameUI.m_sword.AttackEnd();
        }
        GameMgr.Inst.gameScene.m_gameUI.effectsMgr.PlayerBloodEffect();
        GameMgr.Inst.gameScene.m_gameUI.PlaySFX(1);
        int curPlayerDamage = 0;
        int extraMinAttack = 0;
        int extraMaxAttack = 0;
        if(isAttack)
        {
            if(firstAssetWord.wordType == 1)
            {
                extraMinAttack += firstAssetWord.extraMinValue;
                extraMaxAttack += firstAssetWord.extraMaxValue;
            }
            else
            {
                if(firstAssetWord.wordType == 3)
                {
                    extraMinAttack += firstAssetWord.extraMinValue;
                    extraMaxAttack += firstAssetWord.extraMaxValue;
                }
                extraMinAttack += secondAssetWord.extraMinValue;
                extraMaxAttack += secondAssetWord.extraMaxValue;
            }
        }
        if (extraMinAttack < 0) extraMinAttack = 0;
        if(extraMaxAttack <= extraMinAttack) extraMaxAttack = extraMinAttack + 1;
        int shield = 0;
        if (isShield)
        {
            if (firstAssetWord.wordType == 2)
            {
                extraMinAttack += firstAssetWord.extraMinValue;
                extraMaxAttack += firstAssetWord.extraMaxValue;
                shield = Random.Range(firstAssetWord.minValue, firstAssetWord.maxValue + 1);
            }
            else
            {
                extraMinAttack += secondAssetWord.extraMinValue;
                extraMaxAttack += secondAssetWord.extraMaxValue;
                int minShield = secondAssetWord.minValue;
                int maxShield = secondAssetWord.maxValue  + 1;
                if (firstAssetWord.wordType == 3)
                {
                    minShield += firstAssetWord.extraMinValue;
                    maxShield += firstAssetWord.extraMaxValue;
                }
                if (minShield < 0) minShield = 0;
                if (maxShield <= minShield) maxShield = minShield + 1;
                shield = Random.Range(minShield, maxShield);
            }
            gInfo.ShieldAdd(shield);
        }
        curPlayerDamage = gInfo.PlayerDamage(extraMinAttack,extraMaxAttack);
        int curEnemyDamage = kenemy.EnemyDamage();
        int curEnemyBlood = curEnemyDamage - gInfo.shield;
        gInfo.DamageHost(curEnemyDamage);
        if (isAttack)
        {
            kenemy.EnemyHurt(curPlayerDamage);
            gInfo.BloodGained(curPlayerDamage);
            khud.m_damageEffectDlg.EnemyDamage(curPlayerDamage);
        }
        HpBarUpdate();
        gInfo.BloodGained(curEnemyBlood);
        khud.m_bloodUIDlg.BloodUpdate();
        khud.m_damageEffectDlg.PlayerDamage(curEnemyDamage,shield);
        if (gInfo.IsBroke())
        {
            yield return new WaitForSeconds(1.0f);
            m_hpBar.gameObject.SetActive(false);
            GetComponentInChildren<Animator>().SetTrigger("isDead");
            GameMgr.Inst.gameScene.m_gameUI.m_sword.HostDead();
            GameMgr.Inst.gameScene.m_battleFSM.SetResultState();
            if (kenemy.IsEnemyDead())
            {
                kenemy.EnemyDead();
            }
        }
        else if (kenemy.IsEnemyDead())
        {
            yield return new WaitForSeconds(1.0f);
            int exp = kenemy.DropExp();
            StartCoroutine(EnemyDeadCor(exp));
            kenemy.EnemyDead();
            GameMgr.Inst.gameScene.m_hudUI.m_upgradeUIDlg.isSelect = true;
            GameMgr.Inst.gameScene.m_gameUI.effectsMgr.ShieldCloseEffect();
            if (kenemy.IsLastBoss())
            {
                kenemy.isMove = false;
                yield return new WaitForSeconds(1.0f);
                GameMgr.Inst.gameScene.m_battleFSM.SetResultState();
            }
            else
            {
                GameMgr.Inst.gameScene.m_battleFSM.SetWaveState();
            }
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
            GameMgr.Inst.gameScene.m_battleFSM.SetActionState();
            GameMgr.Inst.gameScene.m_gameUI.effectsMgr.ShieldCloseEffect();
        }
        yield return null;
    }
    IEnumerator EnemyTalkCor()
    {
        Enemy kenemy = GameMgr.Inst.gameScene.m_gameUI.m_enemyMgr.m_curEnemy;
        yield return new WaitForSeconds(1f);
        StartCoroutine(TalkEffect());
        int kenemyId = kenemy.enemyID;
        int kenemyLevel = kenemy.enemyLevel;
        int kminAttack = kenemy.minEnemyAttack + Random.Range(3,10);
        int kmaxAttack = kenemy.maxEnemyAttack + Random.Range(3,10);
        int ktrust = Random.Range(6, 10);
        int kEXP = (GameMgr.Inst.ginfo.hostEXP - 7 <= 0) ? 0 : GameMgr.Inst.ginfo.hostEXP - 7;
        yield return new WaitForSeconds(4f);
        GameMgr.Inst.ginfo.SetHostInfo(kenemyLevel, kminAttack, kmaxAttack, ktrust, kenemyId, kEXP);
        //여기 부활
        GetComponentInChildren<Animator>().SetTrigger("isRevive");
        SetHostSprite();
        m_hpBar.gameObject.SetActive(true);
        HpBarUpdate();
        GameMgr.Inst.gameScene.m_gameUI.m_sword.Revive();
        GameMgr.Inst.gameScene.m_hudUI.m_hostInfoDlg.HostInfoUpdate();
        GameMgr.Inst.gameScene.m_hudUI.Revive();
        GameMgr.Inst.gameScene.m_gameUI.m_enemyMgr.DeleteEnemy();
        yield return null;
    }
    IEnumerator TalkEffect()
    {
        m_effectTalk.gameObject.SetActive(true);
        m_effectTalk.Play();
        yield return new WaitForSeconds(2f);
        GameMgr.Inst.gameScene.m_gameUI.m_sword.ReviveEffect();
        yield return new WaitForSeconds(1.5f);
        m_blackFade.color = new Color(0, 0, 0, 0);
        m_blackFade.gameObject.SetActive(true);
        float currentTime = 0;
        float lerpTime = 0.3f;
        while (currentTime != lerpTime)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= lerpTime)
            {
                currentTime = lerpTime;
            }
            float t = currentTime / lerpTime;
            t = EaseOutExpo(t);
            float moveTo = Mathf.Lerp(0, 1, t);
            m_blackFade.color = new Color(0, 0, 0, moveTo);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        GameMgr.Inst.gameScene.m_gameUI.PlaySFX(0);
        m_effectTalk.Stop();
        m_effectTalk.gameObject.SetActive(false);
        currentTime = 0;
        lerpTime = 0.3f;
        while (currentTime != lerpTime)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= lerpTime)
            {
                currentTime = lerpTime;
            }
            float t = currentTime / lerpTime;
            t = EaseOutExpo(t);
            float moveTo = Mathf.Lerp(1, 0, t);
            m_blackFade.color = new Color(0, 0, 0, moveTo);
            yield return null;
        }
        m_blackFade.color = new Color(0, 0, 0, 0);
        m_blackFade.gameObject.SetActive(false);
        yield return null;
    }
    IEnumerator EnemyDeadCor(int exp)
    {
        yield return new WaitUntil(() => GameMgr.Inst.gameScene.m_gameUI.m_enemyMgr.m_curEnemy.enemyDead_Anime);
        GameInfo gInfo = GameMgr.Inst.ginfo;
        gInfo.ExpGained(exp);
        if (gInfo.IsLevelUpPossible())
        {
            gInfo.HostLevelUp();
            HpBarUpdate();
            HudUI khud = GameMgr.Inst.gameScene.m_hudUI;
            m_effectLevelUp.gameObject.SetActive(true);
            m_effectLevelUp.Play();
            yield return new WaitForSeconds(0.5f);
            m_effectLevelUp.Stop();
            yield return new WaitForSeconds(0.5f);
            m_effectLevelUp.gameObject.SetActive(false);
        }
        yield return null;
    }
    IEnumerator AttackCor(int rand)
    {
        GameMgr.Inst.gameScene.m_gameUI.m_sword.Attack(rand);
        yield return null;
    }
    public void HpBarUpdate()
    {
        float curHp = GameMgr.Inst.ginfo.HostHp;
        float maxHp = GameMgr.Inst.ginfo.hostMaxHp;
        float f = (curHp / maxHp) * 100f;

        if (f == 100)
        {
            m_hpBar.sprite = m_hpBarSprites[0];
        }
        else if (f >= 90)
        {
            m_hpBar.sprite = m_hpBarSprites[1];
        }
        else if (f >= 80)
        {
            m_hpBar.sprite = m_hpBarSprites[2];
        }
        else if (f >= 70)
        {
            m_hpBar.sprite = m_hpBarSprites[3];
        }
        else if (f >= 60)
        {
            m_hpBar.sprite = m_hpBarSprites[4];
        }
        else if (f >= 50)
        {
            m_hpBar.sprite = m_hpBarSprites[5];
        }
        else if (f >= 40)
        {
            m_hpBar.sprite = m_hpBarSprites[6];
        }
        else if (f >= 30)
        {
            m_hpBar.sprite = m_hpBarSprites[7];
        }
        else if (f >= 20)
        {
            m_hpBar.sprite = m_hpBarSprites[8];
        }
        else if (f >= 10)
        {
            m_hpBar.sprite = m_hpBarSprites[9];
        }
        else if (f > 0)
        {
            m_hpBar.sprite = m_hpBarSprites[10];
        }
        else
        {
            m_hpBar.sprite = m_hpBarSprites[11];
        }
    }
    float EaseOutExpo(float x)
    {
        return 1 - (1 - x) * (1 - x);
    }
    public void EnchantSword()
    {
        StartCoroutine(EnchantSwordCor());
        HpBarUpdate();
    }
    IEnumerator EnchantSwordCor()
    {
        m_effectSacrifice.gameObject.SetActive(true);
        m_effectSacrifice.Play();
        yield return new WaitForSeconds(0.5f);
        m_effectSacrifice.Stop();
        yield return new WaitForSeconds(0.3f);
        m_effectSacrifice.gameObject.SetActive(false);
        yield return null;
    }
    public void LevelUp()
    {
        StartCoroutine(LevelUP());
    }
    IEnumerator LevelUP()
    {
        m_effectLevelUp.Play();
        yield return new WaitForSeconds(0.5f);
        m_effectLevelUp.Stop();
        yield return new WaitForSeconds(0.5f);
        m_effectLevelUp.gameObject.SetActive(false);
        yield return null;
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Alpha9) && Input.GetKey(KeyCode.Alpha8) && Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            CheatCode1();
        }
        if (Input.GetKey(KeyCode.Alpha9) && Input.GetKey(KeyCode.Alpha8) && Input.GetKeyDown(KeyCode.Alpha2))
        {
            CheatCode2();
        }
        if (Input.GetKey(KeyCode.Alpha9) && Input.GetKey(KeyCode.Alpha8) && Input.GetKeyDown(KeyCode.Alpha3))
        {
            LastBoss();
        }
    }
    void CheatCode1()
    {
        GameMgr.Inst.ginfo.CheatCode();
        GameMgr.Inst.gameScene.m_gameUI.m_enemyMgr.DeleteEnemy();
        GameMgr.Inst.gameScene.m_battleFSM.SetWaveState();
    }
    void CheatCode2()
    {
        GameMgr.Inst.gameScene.m_gameUI.m_enemyMgr.DeleteEnemy();
        GameMgr.Inst.gameScene.m_battleFSM.SetWaveState();
    }
    void LastBoss()
    {
        GameMgr.Inst.ginfo.LastBoss();
        for(int i = 0; i < 10; i++)
        {
            GameMgr.Inst.ginfo.HostLevelUp();
        }
        GameMgr.Inst.ginfo.minHostAttack = 100;
        GameMgr.Inst.ginfo.maxHostAttack = 200;
        GameMgr.Inst.gameScene.m_gameUI.m_enemyMgr.DeleteEnemy();
        GameMgr.Inst.gameScene.m_battleFSM.SetWaveState();
    }
}
