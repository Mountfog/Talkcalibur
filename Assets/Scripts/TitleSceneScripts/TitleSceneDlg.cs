using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneDlg : MonoBehaviour
{
    public Button m_btnStart = null;
    public Button m_btnInst = null;
    public Button m_btnExit = null;
    public Button m_btnOptions = null;
    public Button m_btnOptionsSave = null;
    public Button m_btnOptionsExit = null;
    public Slider m_sliderBGM = null;
    public Slider m_sliderSFX = null;
    public GameObject m_OptionsPanel = null;
    public GameObject m_msgExit = null;
    public Button m_msgYes = null;
    public Button m_msgNo = null;
    public GameObject m_titleSword = null;
    public Animator m_talkBalloonAnimator = null;
    public Image m_fadeOutPanel = null;
    public TitleTips m_titleTips = null;
    public AudioSource m_audioSourceSFX = null;
    public AudioSource m_audioSourceBGM = null;
    public List<AudioClip> m_audioClips = null;
    public bool isCor = false;
    public Texture2D cursorTex;

    private void Awake()
    {
        m_OptionsPanel.SetActive(false);
        m_titleTips.Initialize();
        m_btnStart.onClick.AddListener(OnBtnClick_Start);
        m_btnInst.onClick.AddListener(OnBtnClick_Inst);
        m_btnExit.onClick.AddListener(OnBtnClick_Exit);
        m_btnOptions.onClick.AddListener(OnBtnClick_Options);
        m_msgYes.onClick.AddListener(OnClick_Yes);
        m_msgNo.onClick.AddListener(OnClick_No);
        m_btnOptionsSave.onClick.AddListener(OnBtnClick_OptionsSave);
        m_btnOptionsExit.onClick.AddListener(OnBtnClick_OptionsExit);
        m_sliderBGM.onValueChanged.AddListener((float f)=>OnValueChanged_BGM(f));
        m_fadeOutPanel.gameObject.SetActive(false);
        m_talkBalloonAnimator.SetBool("isAwake", false);
        isCor = false;
        float bgm = PlayerPrefs.GetFloat("BgmVolume", 0.5f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        m_sliderBGM.value = bgm;
        m_sliderSFX.value = sfx;
        m_audioSourceBGM.volume = bgm;
        m_audioSourceSFX.volume = sfx;
        float xspot = cursorTex.width / 2;
        float yspot = cursorTex.height / 2;
        Vector2 hotSpot = new Vector2(xspot - 30f, yspot - 40f);
        Cursor.SetCursor(cursorTex, hotSpot, CursorMode.ForceSoftware);
    }
    private void Start()
    {
        StartCoroutine(SwordIdle());
    }
    public void OnBtnClick_Start()
    {
        m_btnInst.interactable = false;
        m_btnExit.interactable = false;
        m_btnStart.interactable = false;
        StartCoroutine(LoadGame());
    }
    IEnumerator SwordIdle()
    {
        while (true)
        {
            while(m_titleSword.transform.position.y < -1f)
            {
                float moveScale = 0.05f;
                m_titleSword.transform.Translate(new Vector3(0, moveScale, 0), Space.World);
                yield return new WaitForSeconds(0.1f);
            }
            while (m_titleSword.transform.position.y > -3f)
            {
                float moveScale = -0.05f;
                m_titleSword.transform.Translate(new Vector3(0, moveScale, 0), Space.World);
                yield return new WaitForSeconds(0.1f);
            }
            if (isCor)
                break;
            yield return null;
        }
        yield return null;
    }
    IEnumerator LoadGame()
    {
        PlaySFX(1);
        m_talkBalloonAnimator.SetBool("isAwake", true);
        yield return new WaitForSeconds(1f);
        m_fadeOutPanel.gameObject.SetActive(true);
        m_fadeOutPanel.color = new Color(0, 0, 0, 0);
        float curTime = Time.time;
        while(Time.time - curTime <= 2f)
        {
            float t = (Time.time - curTime) / 2f;
            m_fadeOutPanel.color = new Color(0, 0, 0, t);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("GameScene");
        yield return null;
    }
    
    public void OnBtnClick_Inst()
    {
        PlaySFX(2);
        m_titleTips.OnClick_Tips();
    }
    public void OnBtnClick_Exit()
    {
        PlaySFX(2);
        m_msgExit.SetActive(true);
    }
    public void OnBtnClick_Options()
    {
        PlaySFX(2);
        m_OptionsPanel.SetActive(true);
    }
    public void OnValueChanged_BGM(float f)
    {
        m_audioSourceBGM.volume = f;
    }
    public void OnBtnClick_OptionsSave()
    {
        PlaySFX(2);
        PlayerPrefs.SetFloat("BgmVolume", m_sliderBGM.value);
        PlayerPrefs.SetFloat("SFXVolume", m_sliderSFX.value);
    }
    public void OnBtnClick_OptionsExit()
    {
        m_OptionsPanel.SetActive(false);
        float bgm = PlayerPrefs.GetFloat("BgmVolume", 1);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1);
        m_sliderBGM.value = bgm;
        m_sliderSFX.value = sfx;
        m_audioSourceBGM.volume = bgm;
        m_audioSourceSFX.volume = sfx;
        PlaySFX(3);
    }
    public void PlaySFX(int k)
    {
        m_audioSourceSFX.PlayOneShot(m_audioClips[k]);
    }

    public void OnClick_Yes()
    {
        PlaySFX(2);
        m_msgExit.SetActive(false);
        Application.Quit();
    }
    public void OnClick_No()
    {
        PlaySFX(2);
        m_msgExit.SetActive(false);
    }


}
