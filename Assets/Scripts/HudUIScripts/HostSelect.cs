using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostSelect : MonoBehaviour
{
    public delegate void DelegateFunc(int kselect);
    public DelegateFunc m_onBtnClick = null;
    public Image m_hostImage = null;
    public Text m_txtHp = null;
    public Text m_txtAtk = null;
    public Text m_txtTrust = null;
    public Text m_txtLevel = null;
    public List<int> m_hostData = new List<int>();
    public Slider m_sliderTrust = null;

    public void Initialize(int rand,int kmin, int kmax, int krand, int k, int kLevel)
    {
        m_hostData.Clear();
        m_hostData = new List<int> { rand, kmin, kmax , krand,kLevel};
        m_txtHp.text = $"{50 + (kLevel * 10)}";
        m_txtAtk.text = $" {kmin}-{kmax}";
        m_txtLevel.text = $"Level {kLevel}";
        m_sliderTrust.value = krand;
        float r = (krand <= 25) ? 255 : 255 - 255 * ((float)krand / 25);
        float g = (krand <= 25) ? 255 * ((float)krand / 25) : 255;
        m_sliderTrust.fillRect.GetComponent<Image>().color = new Color32((byte)r, (byte)g, 0, 80);
        gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        gameObject.GetComponent<Button>().onClick.AddListener(() => OnClick_Host(k));
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
            str = "절대적인\n신뢰";
        return str;
    }
}
