using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int enemyID = 0;
    public int enemyLevel = 0;
    public int enemyHp = 100;
    public int enemyMaxHp = 100;
    public int minEnemyAttack = 5;
    public int maxEnemyAttack = 10;
    public float enemySpeed = 1f;

    public SpriteRenderer m_hpBar = null;
    public List<Sprite> m_hpBarSprites = new List<Sprite>();
    [HideInInspector]public bool enemyDead_Anime = false;
    bool isKnockback = false;
    public SpriteRenderer boss = null;

    public bool isMove = true;
    public void Initialize(int kenemyID, int kLevel, int minAtk, int maxAtk)
    {
        SpriteRenderer sr = gameObject.GetComponentInChildren<SpriteRenderer>();
        sr.sprite = GameMgr.Inst.gameScene.m_gameUI.m_hostSprites[kenemyID];
        enemyID = kenemyID;
        enemyLevel = kLevel;
        if(kenemyID == 4)
        {
            boss.gameObject.SetActive(true);
        }
        enemyMaxHp = 35 + (15 * kLevel);
        enemyHp = enemyMaxHp;
        isKnockback = false;
        minEnemyAttack = minAtk;
        maxEnemyAttack = maxAtk;
        m_hpBar.sprite = m_hpBarSprites[0];
        Debug.Log(string.Format("Hp : {0}, Atk : {1}-{2}",enemyMaxHp,minEnemyAttack,maxAtk));
        StartCoroutine(EnemyMoveCor());
        StartCoroutine(MoveCor());
        enemyDead_Anime = false;
    }
    IEnumerator EnemyMoveCor()
    {
        while(GameMgr.Inst.gameScene.m_battleFSM.IsWaveState())
        {
            transform.Translate(enemySpeed * Time.deltaTime * Vector2.left, Space.World);

            if(transform.position.x < 5.5f)
            {
                GameMgr.Inst.gameScene.m_battleFSM.SetActionState();
            }
            yield return null;
        }
        yield return null;
    }
    IEnumerator MoveCor()
    {
        while (GameMgr.Inst.gameScene.m_battleFSM.IsWaveState())
        {
            float rand = Random.Range(0.1f, 0.3f);
            transform.Translate(0, rand, 0, Space.World);
            yield return new WaitForSeconds(0.1f);
            transform.Translate(0, rand * -1, 0, Space.World);
            yield return new WaitForSeconds(0.1f);
            yield return null;
        }
        yield return null;
    }
    public void EnemyAttack()
    {
        GetComponentInChildren<Animator>().SetTrigger("enemyAttack");
        if (enemyID == 4 || enemyID == 5 || enemyID == 6 )
        {
            GameMgr.Inst.gameScene.m_gameUI.effectsMgr.BossAttackEffect();
        }
    }
    public void KnockBackEnemy(float attackForce)
    {
        Invoke("KnockBack", 0.4f);
    }
    public void KnockBack()
    {
        StartCoroutine(KnockBackEnemyCor());
        //GetComponent<Rigidbody2D>().AddForce(Vector2.right * attackForce, ForceMode2D.Impulse);
    }
    IEnumerator KnockBackEnemyCor()
    {
        isKnockback = true;
        float currentTime = 0;
        float lerpTime = 2f;
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
            float moveTo = Mathf.Lerp(transX, 5.5f, t);
            float kmove = moveTo - transform.position.x;
            transform.Translate(new Vector3(kmove, 0, 0), Space.World);
            //transform.position = new Vector3(moveTo, -0.8f, 0);
            yield return null;
        }
        isKnockback = false;
        if (IsEnemyDead())
        {

        }
        yield return null;
    }
    public void EnemyDead()
    {
        m_hpBar.gameObject.SetActive(false);
        GetComponentInChildren<Animator>().SetTrigger("isDead");
        enemyDead_Anime = true;
        isMove = true;
        if (!GameMgr.Inst.gameScene.m_battleFSM.IsResultState())
        {
            StartCoroutine(DeadCor());
        }
    }
    void DeleteMyself()
    {
        GameMgr.Inst.gameScene.m_gameUI.m_enemyMgr.DeleteEnemy();
    }
    IEnumerator DeadCor()
    {
        float a = 255f;
        while (transform.position.x >= -11f && isMove)
        {
            transform.Translate(2 * Time.deltaTime * Vector2.left, Space.World);
            yield return null;
        }
        Destroy(gameObject);
        yield return null;
    }
    float EaseOutExpo(float x)
    {
        return (x == 1) ? 1 : 1 - Mathf.Pow(2,-10 * x);
    }
    public void HpBarUpdate()
    {
        float f = ((float)enemyHp / (float)enemyMaxHp) * 100f;
        if(f == 100)
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
        else if( f > 0)
        {
            m_hpBar.sprite = m_hpBarSprites[10];
        }
        else
        {
            m_hpBar.sprite = m_hpBarSprites[11];
        }
    }
    public void EnemyHurt(int pDamage)
    {
        enemyHp -= pDamage;
        HpBarUpdate();
        GameMgr.Inst.gameScene.m_hudUI.m_enemyInfoDlg.EnemyInfoUpdate(enemyID, enemyLevel, minEnemyAttack, maxEnemyAttack, enemyHp);
    }
    public int EnemyDamage()
    {
        return Random.Range(minEnemyAttack, maxEnemyAttack + 1);
    }
    public int DropExp()
    {
        return Mathf.Clamp(Random.Range(20,31) + GameMgr.Inst.ginfo.CurStage * 3,20,40);
    }
    public bool IsEnemyTalkable() { return (((float)enemyHp / (float)enemyMaxHp) * 100f) <= 25f && !IsEnemyDead(); }
    public bool IsEnemyDead() { return enemyHp <= 0; }
    public bool IsLastBoss()
    {
        return GameMgr.Inst.ginfo.CurStage == 3 && enemyID == 6;
    }
}
