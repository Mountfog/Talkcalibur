using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
public class HostInfoDlg : MonoBehaviour
{
    public Image m_hostImage = null;
    public Text m_txtHp = null;
    public Text m_txtAtk = null;
    public Text m_txtTrust = null;
    public Text m_TxtLevel = null;

    bool isOpen = false;

    public void Initialize()
    {
        isOpen = false;
        HostInfoUpdate();
    }
    string TrustString(int krand)
    {
        string str = "None";
        if (krand <= 3) // 0~3
            str = "불신하는";
        else if (krand <= 6) //4~6
            str = "의심스러운";
        else if (krand <= 9) //7~9
            str = "중립적인";
        else if (krand <= 12) //10~12
            str = "신용하는";
        else if (krand <= 15) //13~15
            str = "절대적인 신뢰";
        return str;
    }
    public void HostInfoUpdate()
    {
        int hostID = GameMgr.Inst.ginfo.hostID;
        int hostHp = GameMgr.Inst.ginfo.HostHp;
        int hostMaxHp = GameMgr.Inst.ginfo.hostMaxHp;
        int hostMin = GameMgr.Inst.ginfo.minHostAttack;
        int hostMax = GameMgr.Inst.ginfo.maxHostAttack;
        int krand = GameMgr.Inst.ginfo.hostTrust;
        int hostLevel = GameMgr.Inst.ginfo.hostLevel;
        int hostEXP = GameMgr.Inst.ginfo.hostEXP;
        m_hostImage.sprite = GameMgr.Inst.gameScene.m_gameUI.m_hostSprites[hostID];
        m_txtHp.text = string.Format("{0} / {1}", hostHp, hostMaxHp);
        m_txtAtk.text = string.Format("{0}-{1}", hostMin, hostMax);
        m_txtTrust.text = TrustString(krand);
        m_TxtLevel.text = string.Format("Level {0} ({1} / 100)", hostLevel, hostEXP);
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
        while(currentTime != lerpTime)
        {
            currentTime += Time.deltaTime;
            if(currentTime >= lerpTime)
            {
                currentTime = lerpTime;
            }
            float t = currentTime / lerpTime;
            t = EaseOutExpo(t);
            float moveTo = Mathf.Lerp(transY,0f, t);
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
            float moveTo = Mathf.Lerp(transY ,-270f, t);
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
