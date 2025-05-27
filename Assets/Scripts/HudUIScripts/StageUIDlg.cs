using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageUIDlg : MonoBehaviour
{
    public Text m_textStage = null;
    public Text m_textTillBoss = null;
    public GameObject m_woodenPanel = null;
    public Vector3 m_openPos = new Vector3(0,308,0);
    public Vector3 m_closedPos = new Vector3(0, 555, 0);

    public void SetReadyState()
    {
        m_woodenPanel.SetActive(false);
    }
    public void SetWaveState()
    {
        StartCoroutine(SpawnCor());
    }
    IEnumerator SpawnCor()
    {
        yield return new WaitUntil(() => !GameMgr.Inst.gameScene.m_hudUI.m_upgradeUIDlg.isSelect);
        m_woodenPanel.transform.localPosition = m_closedPos;
        m_woodenPanel.SetActive(true);
        StageInfoUpdate();
        StartCoroutine(WaveCor());
        yield return null;
    }
    public void StageInfoUpdate()
    {
        GameInfo gInfo = GameMgr.Inst.ginfo;
        m_textStage.text = $"스테이지 {gInfo.CurStage}";
        if (!gInfo.BossSpawnNeeded())
        {
            m_textTillBoss.fontSize = 100;
            m_textTillBoss.text = $"{gInfo.EnemiesTillBoss}명의 적을 물리쳐야\n보스 등장";
        }
        else
        {
            m_textTillBoss.fontSize = 150;
            m_textTillBoss.text = "<color=#FF5A69>보스 등장!!!</color>";
        }
    }
    IEnumerator WaveCor()
    {
        StartCoroutine(EaseInCor());
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(EaseOutCor());
        yield return null;
    }
    IEnumerator EaseInCor()
    {
        float currentTime = 0;
        float lerpTime = 1f;
        float transY = m_woodenPanel.transform.localPosition.y;
        while (currentTime != lerpTime)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= lerpTime)
            {
                currentTime = lerpTime;
            }
            float t = currentTime / lerpTime;
            t = EaseOutExpo(t);
            float moveTo = Mathf.Lerp(transY, m_openPos.y, t);
            m_woodenPanel.transform.localPosition = new Vector3(0, moveTo, 0);
            yield return null;
        }
        yield return null;
    }
    IEnumerator EaseOutCor()
    {
        float currentTime = 0;
        float lerpTime = 1f;
        float transY = m_woodenPanel.transform.localPosition.y;
        while (currentTime != lerpTime)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= lerpTime)
            {
                currentTime = lerpTime;
            }
            float t = currentTime / lerpTime;
            t = EaseOutExpo(t);
            float moveTo = Mathf.Lerp(transY, m_closedPos.y, t);
            m_woodenPanel.transform.localPosition = new Vector3(0, moveTo, 0);
            yield return null;
        }
        yield return null;
    }
    float EaseOutExpo(float x)
    {
        return 1 - (1 - x) * (1 - x);
    }

}
