using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CWordObj : MonoBehaviour
{
    public delegate void DelegateFunc(int k);
    public DelegateFunc m_onBtnClick = null;
    public Button m_btnWord = null;
    public Text m_textWord = null;
    public string m_word = null;
    public Vector3 m_startPos = new Vector3(0, 38, 0);
    public Vector3 m_endPos = new Vector3(0, 0, 0);

    public void SetAction()
    {
        transform.localPosition = m_startPos;
        m_textWord.color = new Color(1f, 0f, 0f, 0f);
        m_btnWord.interactable = false;
    }
    public void Initialize(int k)
    {
        string kstring = AssetMgr.Inst.GetAssetWord(k).basicWord;
        m_textWord.text = kstring;
        m_word = kstring;
        m_btnWord.onClick.RemoveAllListeners();
        m_btnWord.onClick.AddListener(() => OnClick_Host(k));
        gameObject.GetComponent<WordEventMgr>().wordId = k;
        StartCoroutine(WaveCor());
    }
    public void AddLinstner(DelegateFunc func)
    {
        m_onBtnClick = new DelegateFunc(func);
    }
    private void OnClick_Host(int k)
    {
        if (m_onBtnClick != null)
            m_onBtnClick(k);
    }
    public void OnHover()
    {
        Outline koutline = gameObject.GetComponentInChildren<Outline>();
        koutline.effectColor = new Color32((byte)255,(byte)255,(byte)0,(byte)83);
    }
    public void OnHoverExit()
    {
        Outline koutline = gameObject.GetComponentInChildren<Outline>();
        koutline.effectColor = new Color32((byte)255, (byte)0, (byte)0, (byte)83);
    }
    IEnumerator WaveCor()
    {
        yield return new WaitForSeconds(Random.Range(0.1f,0.5f));
        transform.localPosition = m_startPos;
        m_textWord.color = new Color(1f, 0f, 0f, 0f);
        m_btnWord.interactable = false;
        float currentTime = 0;
        float lerpTime = 0.5f;
        Vector3 transVec = m_startPos;
        while (currentTime != lerpTime)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= lerpTime)
            {
                currentTime = lerpTime;
            }
            float t = currentTime / lerpTime;
            t = EaseOutExpo(t);
            Vector3 moveTo = Vector3.Lerp(m_startPos, m_endPos, t);
            m_textWord.color = new Color32((byte)255f, (byte)84f, (byte)76f, (byte)(t * 255f));
            transform.localPosition = moveTo;
            yield return null;
        }
        m_btnWord.interactable = true;
        yield return null;
    }
    float EaseOutExpo(float x)
    {
        return 1 - (1 - x) * (1 - x);
    }
}
