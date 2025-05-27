using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMgr : MonoBehaviour
{
    public Enemy m_curEnemy = null;
    public GameObject m_enemyPrefab = null;
    public Coroutine m_enemyCoroutine = null;
    public Transform m_enemyParent = null;
    public Transform m_enemyTrans = null;
    public void Initialize()
    {
        if(m_curEnemy is not null)
        {
            Destroy(m_curEnemy.gameObject);
            m_curEnemy = null;
        }
    }
    public void SetWaveState()
    {
        StartCoroutine(SpawnCor());
    }
    public void DeleteEnemy()
    {
        Destroy(m_curEnemy.gameObject);
        m_curEnemy = null;
    }
    IEnumerator SpawnCor()
    {
        yield return new WaitUntil(() => !GameMgr.Inst.gameScene.m_hudUI.m_upgradeUIDlg.isSelect);
        if (GameMgr.Inst.ginfo.BossSpawnNeeded())
        {
            StartCoroutine(BossCor());
        }
        Invoke(nameof(SpawnEnemy), 5.0f);
        yield return null;
    }
    IEnumerator BossCor()
    {
        yield return new WaitForSeconds(0.5f);
        GameMgr.Inst.gameScene.m_gameUI.PlaySFX(5);
        yield return new WaitForSeconds(1f);
        GameMgr.Inst.gameScene.m_gameUI.PlayBGM(1);
        yield return null;
    }
    void SpawnEnemy()
    {
        if (GameMgr.Inst.ginfo.BossSpawnNeeded())
        {
            int bloodDiff = GameMgr.Inst.ginfo.CurStage * 5;
            int kenemyID = GameMgr.Inst.ginfo.CurStage + 3;
            int kMin = 10 + bloodDiff;
            int kMax = 25 + bloodDiff;
            int kLevel = 10 + bloodDiff;
            int kcurHp = 50 + (kLevel * 10);
            GameMgr.Inst.gameScene.m_hudUI.m_enemyInfoDlg.EnemyInfoUpdate(kenemyID, kLevel, kMin, kMax, kcurHp);
            m_curEnemy = CreateEnemy(kenemyID, kLevel, kMin, kMax);
        }
        else
        {
            int bloodDiff = (GameMgr.Inst.ginfo.CurStage - 1) * 3;
            int kenemyID = Random.Range(0, 4);
            int kMin = Random.Range(4 + bloodDiff / 2, 9 + bloodDiff);
            int kMax = Random.Range(9, 12) + bloodDiff;
            int kLevel = Random.Range(1, 4) + bloodDiff * 2;
            int kcurHp = 50 + (kLevel * 10);
            GameMgr.Inst.gameScene.m_hudUI.m_enemyInfoDlg.EnemyInfoUpdate(kenemyID, kLevel, kMin, kMax, kcurHp);
            m_curEnemy = CreateEnemy(kenemyID, kLevel, kMin, kMax);
            GameMgr.Inst.ginfo.EnemySpawned();
        }
    }
    Enemy CreateEnemy(int kenemyID,int kLevel, int kMin,int kMax)
    {
        GameObject go = Instantiate(m_enemyPrefab,m_enemyTrans.position,Quaternion.identity,m_enemyParent);
        Enemy kenemy = go.GetComponent<Enemy>();
        kenemy.Initialize(kenemyID,kLevel,kMin,kMax);
        return kenemy;
    }
}
