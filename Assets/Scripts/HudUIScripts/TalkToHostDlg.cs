using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkToHostDlg : MonoBehaviour
{
    public GameObject m_topUI = null;
    public GameObject m_bottomUI = null;

    
    public Vector3 m_topUIOpenVec = Vector3.zero;
    public Vector3 m_topUICloseVec = Vector3.zero;
    public Vector3 m_bottomUIOpenVec = Vector3.zero;
    public Vector3 m_bottomUICloseVec = Vector3.zero;
    
    [Header("TopUI")]
    public Button m_BtnReroll = null;
    public List<CWordObj> wordObjects = new List<CWordObj>();
    public List<CBloodObj> bloodEffects = new List<CBloodObj>();


    [Header("BottomUI")]
    public Button m_btnFirst = null;
    public Button m_btnSecond = null;

    public string firstWord = string.Empty;
    public string secondWord = string.Empty;

    public void SetReadyState()
    {
        m_topUI.transform.localPosition = m_topUICloseVec;
        m_bottomUI.transform.localPosition = m_bottomUICloseVec;
        m_btnFirst.onClick.RemoveAllListeners();
        m_btnSecond.onClick.RemoveAllListeners();
        m_btnFirst.onClick.AddListener(OnClick_First);
        m_btnSecond.onClick.AddListener(OnClick_Second);
        for (int i = 0; i < bloodEffects.Count; i++)
        {
            bloodEffects[i].Initialize();
        }
    }
    public void SetWaveState()
    {
        firstWord = string.Empty;
        secondWord = string.Empty;
        StartCoroutine(TopEaseOutCor());
        StartCoroutine(BottomEaseOutCor());
    }
    public void SetActionState()
    {
        StartCoroutine(TopEaseInCor());
        StartCoroutine(BottomEaseInCor());
        StartCoroutine(SetActionCor());
        if (SentenceFinished())
        {
            StartCoroutine(FadeSentence());
        }
        else
        {
            BtnText(m_btnFirst.gameObject).text = string.Empty;
            BtnText(m_btnSecond.gameObject).text = string.Empty;
        }
        firstWord = string.Empty;
        secondWord = string.Empty;
    }
    IEnumerator SetActionCor()
    {
        foreach(CWordObj wordObj in wordObjects)
        {
            wordObj.SetAction();
        }
        yield return new WaitForSeconds(1.5f);
        GameMgr.Inst.gameScene.m_gameUI.PlaySFX(10);
        List<int> numList = new List<int>();
        int i = 0;
        while (i < wordObjects.Count)
        {
            CWordObj wordObj = wordObjects[i];
            int krand = Random.Range(0, 6);
            bool fbool = false;
            for (int j = 0; j < numList.Count; j++)
            {
                if (krand == numList[j])
                {
                    fbool = true;
                }
            }
            if (fbool)
            {
                continue;
            }
            numList.Add(krand);
            wordObj.AddLinstner(OnClick_Word);
            int k = krand;
            wordObj.Initialize(k);
            i++;
        }
        yield return null;
    }
    public void OnClick_Word(int k)
    {
        AssetWord kassetWord = AssetMgr.Inst.GetAssetWord(k);

        if(BtnText(m_btnFirst.gameObject).text == string.Empty)
        {
            if(secondWord != string.Empty)
            {
                AssetWord second = AssetMgr.Inst.GetAssetWord(secondWord);
                if (second.wordType == 1 && kassetWord.wordType == 1)
                {
                    return;
                }
                if (second.wordType == 2 && kassetWord.wordType == 2)
                {
                    return;
                }
            }
            BtnText(m_btnFirst.gameObject).text = kassetWord.firstWord;
            firstWord = kassetWord.basicWord;
        }
        else if(BtnText(m_btnSecond.gameObject).text == string.Empty)
        {
            if (firstWord != string.Empty)
            {
                AssetWord second = AssetMgr.Inst.GetAssetWord(firstWord);
                if (second.wordType == 1 && kassetWord.wordType == 1)
                {
                    return;
                }
                if (second.wordType == 2 && kassetWord.wordType == 2)
                {
                    return;
                }
            }
            if (kassetWord.wordType == 3)
            {
                return;
            }
            BtnText(m_btnSecond.gameObject).text = kassetWord.secondWord;
            secondWord = kassetWord.basicWord;
        }
        else
        {
            //ºó °ø°£ ¾øÀ½
        }
        GameMgr.Inst.gameScene.m_hudUI.SetTurnEndButton();
    }
    public void OnClick_First()
    {
        BtnText(m_btnFirst.gameObject).text = string.Empty;
        firstWord = string.Empty;
        GameMgr.Inst.gameScene.m_hudUI.SetTurnEndButton();
    }
    public void OnClick_Second()
    {
        BtnText(m_btnSecond.gameObject).text = string.Empty;
        secondWord = string.Empty;
        GameMgr.Inst.gameScene.m_hudUI.SetTurnEndButton();
    }
    IEnumerator TopEaseInCor()
    {
        float currentTime = 0;
        float lerpTime = 0.5f;
        float transY = m_topUI.transform.localPosition.y;
        while (currentTime != lerpTime)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= lerpTime)
            {
                currentTime = lerpTime;
            }
            float t = currentTime / lerpTime;
            t = EaseOutExpo(t);
            float moveTo = Mathf.Lerp(transY, m_topUIOpenVec.y, t);
            m_topUI.transform.localPosition = new Vector3(0, moveTo, 0);
            yield return null;
        }
        List<int> knumList = new List<int>();
        for (int m = 0; m < 6; m++)
        {
            int krand = Random.Range(0, bloodEffects.Count);
            bool kfbool = false;
            for (int l = 0; l < knumList.Count; l++)
            {
                if (krand == knumList[l])
                {
                    kfbool = true;
                }
            }
            if (kfbool)
            {
                m--;
                continue;
            }
            knumList.Add(krand);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
            GameMgr.Inst.gameScene.m_gameUI.PlaySFX(9);
            bloodEffects[krand].PlayBlood();
        }
        yield return null;
    }
    IEnumerator BottomEaseInCor()
    {
        float currentTime = 0;
        float lerpTime = 0.5f;
        float transY = m_bottomUI.transform.localPosition.y;
        while (currentTime != lerpTime)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= lerpTime)
            {
                currentTime = lerpTime;
            }
            float t = currentTime / lerpTime;
            t = EaseOutExpo(t);
            float moveTo = Mathf.Lerp(transY, m_bottomUIOpenVec.y, t);
            m_bottomUI.transform.localPosition = new Vector3(0, moveTo, 0);
            yield return null;
        }
        yield return null;
    }
    IEnumerator TopEaseOutCor()
    {
        float currentTime = 0;
        float lerpTime = 0.5f;
        float transY = m_topUI.transform.localPosition.y;
        while (currentTime != lerpTime)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= lerpTime)
            {
                currentTime = lerpTime;
            }
            float t = currentTime / lerpTime;
            t = EaseOutExpo(t);
            float moveTo = Mathf.Lerp(transY, m_topUICloseVec.y, t);
            m_topUI.transform.localPosition = new Vector3(0, moveTo, 0);
            yield return null;
        }
        m_topUI.transform.localPosition = m_topUICloseVec;
        yield return null;
    }
    IEnumerator BottomEaseOutCor()
    {
        float currentTime = 0;
        float lerpTime = 0.5f;
        float transY = m_bottomUI.transform.localPosition.y;
        while (currentTime != lerpTime)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= lerpTime)
            {
                currentTime = lerpTime;
            }
            float t = currentTime / lerpTime;
            t = EaseOutExpo(t);
            float moveTo = Mathf.Lerp(transY, m_bottomUICloseVec.y, t);
            m_bottomUI.transform.localPosition = new Vector3(0, moveTo, 0);
            yield return null;
        }
        m_bottomUI.transform.localPosition = m_bottomUICloseVec;
        yield return null;
    }
    float EaseOutExpo(float x)
    {
        return 1 - (1 - x) * (1 - x);
    }
    public void Talk()
    {
        StartCoroutine(TalkCor());
    }
    IEnumerator TalkCor()
    {
        yield return new WaitForSeconds(0.1f);
        GameMgr.Inst.gameScene.m_gameUI.PlaySFX(2);
        yield return new WaitForSeconds(0.1f);
        GameMgr.Inst.gameScene.m_gameUI.PlaySFX(2);
        yield return new WaitForSeconds(0.1f);
        GameMgr.Inst.gameScene.m_gameUI.PlaySFX(2);
        yield return null;
    }
    IEnumerator FadeSentence()
    {
        float currentTime = 0;
        float lerpTime = 0.5f;
        while (currentTime != lerpTime)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= lerpTime)
            {
                currentTime = lerpTime;
            }
            float t = currentTime / lerpTime;
            t = EaseOutExpo(t);
            BtnText(m_btnFirst.gameObject).color = new Color(1, 0, 0, 1 - t);
            BtnText(m_btnSecond.gameObject).color = new Color(1, 0, 0, 1 - t);
            yield return null;
        }
        BtnText(m_btnFirst.gameObject).text = string.Empty;
        BtnText(m_btnSecond.gameObject).text = string.Empty;
        BtnText(m_btnFirst.gameObject).color = new Color(1, 0, 0, 1);
        BtnText(m_btnSecond.gameObject).color = new Color(1, 0, 0, 1);
        yield return null;
    }
    public Text BtnText(GameObject go)
    {
        return go.GetComponentInChildren<Text>();
    }

    public bool SentenceFinished()
    {
        return (BtnText(m_btnFirst.gameObject).text != string.Empty) && (BtnText(m_btnSecond.gameObject).text != string.Empty);
    }

    
}
