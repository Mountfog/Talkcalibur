using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public List<Sprite> m_hostSprites = new List<Sprite>();
    public BackGroundMgr m_backGroundMgr = null;
    public EnemyMgr m_enemyMgr = null;
    public PlayerController m_playerController = null;
    public SwordController m_sword = null;
    public List<AudioClip> effectAudioClips = new List<AudioClip>();
    public List<AudioClip> BGAudioClips = new List<AudioClip>();
    public AudioSource m_audioSourceBGM = null;
    public EffectsMgr effectsMgr = null;

    public enum EffectName
    {
        _1up = 0,
        hit = 1,
        talk = 2,
        UIOn = 3,
        UIoff = 4,
    }
    public enum BGName
    {
        stage1_normal = 0,
        stage1_boss = 1,
        stage2_normal = 2,
        stage2_boss = 3,
        stage3_normal = 4,
        stage3_boss = 5,
    }

    public void SetReadyState()
    {
        PlayBGM(0);
        m_playerController.SetReadyState();
        m_backGroundMgr.SetReadyState();
        m_enemyMgr.Initialize();
        m_sword.gameObject.SetActive(false);
        m_sword.SetReadyState();
    }
    public void SetWaveState()
    {
        m_sword.gameObject.SetActive(true);
        m_backGroundMgr.SetWaveState();
        m_backGroundMgr.SetStage();
        m_enemyMgr.SetWaveState();

    }
    public void SetActionState()
    {
        m_playerController.SetActionState();
    }
    public void SetBattleState()
    {
        m_playerController.SetBattleState();
    }
    public void PlaySFX(int num)
    {
        GetComponent<AudioSource>().PlayOneShot(effectAudioClips[num]);
    }
    public void PlaySFX(int num, float volume)
    {
        GetComponent<AudioSource>().PlayOneShot(effectAudioClips[num],volume);
    }
    public void PlayBGM(int num)
    {
        m_audioSourceBGM.clip = BGAudioClips[num];
        m_audioSourceBGM.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
