using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBloodObj : MonoBehaviour
{
    public int bloodID = -1;
    public Animator m_selfAnime = null;

    private void Awake()
    {
        m_selfAnime = gameObject.GetComponent<Animator>();
    }
    public void Initialize()
    {
        int k = bloodID;
        m_selfAnime.SetInteger("bloodNum", k);
        //Debug.Log(k);
    }

    public void PlayBlood()
    {
        //Debug.Log(m_selfAnime.GetInteger("bloodNum"));
        m_selfAnime.SetInteger("bloodNum", bloodID);
        m_selfAnime.SetTrigger("bloodTrig");
    }
}
