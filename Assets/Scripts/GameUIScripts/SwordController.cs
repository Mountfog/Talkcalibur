using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    public List<TalkBalloonController> talkBalloons = new List<TalkBalloonController>();
    public Transform player;
    public Vector3 offSet = new Vector3(0, 0, -10);
    public Animator m_swordAnimator = null;
    public ParticleSystem m_effectSacrifice = null;
    public ParticleSystem m_effectRevive = null;

    public void SetReadyState()
    {
        m_effectSacrifice.gameObject.SetActive(false);
        m_effectRevive.gameObject.SetActive(false);
        m_swordAnimator = GetComponentInChildren<Animator>();
        m_swordAnimator.SetTrigger("isRevived");
        for (int i = 0; i < talkBalloons.Count; i++)
        {
            talkBalloons[i].Initialize();
        }
        UpdateSwordToDefaultPosition();
    }
    public void SwordTalk()
    {
        int i = Random.Range(0, talkBalloons.Count);
        if (!talkBalloons[i].gameObject.activeSelf)
        {
            talkBalloons[i].gameObject.SetActive(true);
            talkBalloons[i].AwakeBalloon();
        }
        else
        {
            SwordTalk();
        }
    }
    public void SacrificeSuccess()
    {
        m_effectSacrifice.gameObject.SetActive(true);
        m_effectSacrifice.Play();
    }
    public void Attack(int rand)
    {
        m_swordAnimator.SetInteger("rand", rand);
        GameMgr.Inst.gameScene.m_gameUI.effectsMgr.PlayerAttackEffect(rand,Random.Range(0,2));
        m_swordAnimator.SetTrigger("isAttack");
    }
    public void AttackEnd()
    {
        if (m_effectSacrifice.isEmitting)
        {
            m_effectSacrifice.Stop();
            m_effectSacrifice.gameObject.SetActive(false);
        }
    }
    public void HostDead()
    {
        m_swordAnimator.SetTrigger("isDead");
    }
    public void Revive()
    {
        m_swordAnimator.SetTrigger("isRevived");
    }
    public void ReviveEffect()
    {
        StartCoroutine(ReviveCor());
    }
    IEnumerator ReviveCor()
    {
        m_effectRevive.gameObject.SetActive(true);
        m_effectRevive.Play();
        yield return new WaitForSeconds(1.5f);
        m_effectRevive.Stop();
        yield return new WaitForSeconds(0.3f);
        m_effectRevive.gameObject.SetActive(false);
        yield return null;
        yield return null;
    }
    // Update is called once per frame
    void Update()
    {
        if (GameMgr.Inst.gameScene.m_battleFSM.IsBattleState())
        {
            for (int i = 0; i < talkBalloons.Count; i++)
            {
                talkBalloons[i].transform.rotation = Quaternion.Euler(0,0,transform.rotation.z * -1);
            }
        }
    }
    public void UpdateSwordToDefaultPosition()
    {
        Vector3 targetPos = player.position + offSet;
        transform.position = targetPos;
    }
    float EaseOutExpo(float x)
    {
        return 1 - (1 - x) * (1 - x);
    }
}
