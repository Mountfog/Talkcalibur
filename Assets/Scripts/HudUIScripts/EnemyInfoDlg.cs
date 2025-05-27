using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfoDlg : MonoBehaviour
{

    public Image m_hostImage = null;
    public Text m_txtHp = null;
    public Text m_txtAtk = null;
    public Text m_TxtLevel = null;
    bool isOpen = false;

    public void Initialize()
    {
        isOpen = false;
    }
    public void EnemyInfoUpdate(int kenemyID,int klevel,int kmin,int kmax,int kCurHp)
    {
        int hostID = kenemyID;
        int hostMaxHp = 50 + (klevel * 10);
        int hostHp = (kCurHp<0)?0:kCurHp;
        int hostMin = kmin;
        int hostMax = kmax;
        int hostLevel = klevel;
        m_hostImage.sprite = GameMgr.Inst.gameScene.m_gameUI.m_hostSprites[hostID];
        m_txtHp.text = string.Format("{0} / {1}", hostHp, hostMaxHp);
        m_txtAtk.text = string.Format("{0}-{1}", hostMin, hostMax);
        m_TxtLevel.text = string.Format("Level {0}", hostLevel);
    }
    public void EaseIn()
    {
        StartCoroutine(EaseInCor());
    }
    public void EaseOut()
    {
        StartCoroutine(EaseOutCor());
    }
    IEnumerator EaseInCor()
    {
        float currentTime = 0;
        float lerpTime = 0.3f;
        float transY = transform.localPosition.y;
        while (currentTime != lerpTime)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= lerpTime)
            {
                currentTime = lerpTime;
            }
            float t = currentTime / lerpTime;
            t = EaseOutExpo(t);
            float moveTo = Mathf.Lerp(transY, 0f, t);
            transform.localPosition = new Vector3(0, moveTo, 0);
            yield return null;
        }
        yield return null;
    }
    IEnumerator EaseOutCor()
    {
        float currentTime = 0;
        float lerpTime = 0.3f;
        float transY = transform.localPosition.y;
        while (currentTime != lerpTime)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= lerpTime)
            {
                currentTime = lerpTime;
            }
            float t = currentTime / lerpTime;
            t = EaseOutExpo(t);
            float moveTo = Mathf.Lerp(transY, -270f, t);
            transform.localPosition = new Vector3(0, moveTo, 0);
            yield return null;
        }
        yield return null;
    }
    float EaseOutExpo(float x)
    {
        return 1 - (1 - x) * (1 - x);
    }
    public void ActivateUI()
    {
        gameObject.SetActive(true);
    }
    public void HideUI()
    {
        gameObject.SetActive(false);
    }
}
