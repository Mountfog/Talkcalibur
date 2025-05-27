using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectHostDlg : MonoBehaviour
{
    public List<HostSelect> m_hostSelects = new List<HostSelect>();
    public void Initialize()
    {
        List<Sprite> hostSprites = GameMgr.Inst.gameScene.m_gameUI.m_hostSprites;
        for(int i = 0; i < m_hostSelects.Count; i++)
        {
            int randLevel = Random.Range(3, 7);
            int rand = Random.Range(0,4);
            m_hostSelects[i].m_hostImage.sprite = hostSprites[rand];
            int kMin = Random.Range(9, 14) + i; //최소데미지
            int kMax = Random.Range(17, 22) - (i * 2); //최대데미지
            int krand = 15 + i * 10;//Random.Range(3 + (i * 3),10 + (i * 2)); 신뢰도
            int k = i;
            m_hostSelects[i].Initialize(rand,kMin,kMax,krand,k,randLevel);
            m_hostSelects[i].AddLinstner(OnClick_HostSelect);
        }
    }
    public void OnClick_HostSelect(int kselect)
    {
        List<int> hostData = m_hostSelects[kselect].m_hostData;
        GameMgr.Inst.ginfo.SetHostInfo(hostData[4], hostData[1], hostData[2], hostData[3], hostData[0],0);
        GameMgr.Inst.gameScene.m_battleFSM.SetWaveState();
    }

}
