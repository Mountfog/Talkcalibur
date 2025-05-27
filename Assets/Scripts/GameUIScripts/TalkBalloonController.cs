using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkBalloonController : MonoBehaviour
{
    public Animator m_talkBalloonAnimator = null;
    public void Initialize()
    {
        m_talkBalloonAnimator.SetBool("isAwake", false);
        gameObject.SetActive(false);
    }
    public void AwakeBalloon()
    {
        StartCoroutine(BalloonAwakeCor());
    }
    IEnumerator BalloonAwakeCor()
    {
        m_talkBalloonAnimator.SetBool("isAwake", true);
        yield return new WaitForSeconds(1f);
        m_talkBalloonAnimator.SetBool("isAwake", false);
        gameObject.SetActive(false);
        yield return null;
    }
}
