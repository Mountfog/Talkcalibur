using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUIDlg : MonoBehaviour
{
    public bool isSelect = false;
    public List<CUpgradeObj> upgradeObjects = new List<CUpgradeObj>();
    public List<Sprite> iconSprites = new List<Sprite>();
    public ParticleSystem m_effectLevelUp = null;

    public void Initialize()
    {
        List<int> numList = new List<int>();
        for (int i = 0; i < upgradeObjects.Count; i++)
        {
            int rand = Random.Range(0, iconSprites.Count);
            bool isAlready = false;
            for(int j = 0;j < numList.Count; j++)
            {
                if(rand == numList[j])
                    isAlready = true;
            }
            if (isAlready)
            {
                i--;
                continue;
            }
            upgradeObjects[i].m_iconImage.sprite = iconSprites[rand];
            numList.Add(rand);
            int k = i;
            upgradeObjects[i].Initialize(rand, k);
            upgradeObjects[i].AddLinstner(OnClick_HostSelect);
        }
    }
    public void OnClick_HostSelect(int kselect)
    {
        GameInfo gInfo = GameMgr.Inst.ginfo;
        int iconNum = upgradeObjects[kselect].icon;
        if(iconNum == 0)
        {
            gInfo.ExpGained(30);
            if (gInfo.IsLevelUpPossible())
            {
                gInfo.HostLevelUp();
                GameMgr.Inst.gameScene.m_gameUI.m_playerController.HpBarUpdate();
                HudUI khud = GameMgr.Inst.gameScene.m_hudUI;
                m_effectLevelUp.gameObject.SetActive(true);
                GameMgr.Inst.gameScene.m_gameUI.m_playerController.LevelUp();
            }
        }
        if(iconNum == 1)
        {
            gInfo.minHostAttack++;
        }
        if (iconNum == 2)
        {
            gInfo.maxHostAttack++;
        }
        if(iconNum == 3)
        {
            gInfo.minHostAttack -= 2;
            if(gInfo.minHostAttack < 0)
                gInfo.minHostAttack = 0;
            gInfo.maxHostAttack += 2;
        }
        gInfo.AttackClean();
        isSelect = false;
        gameObject.SetActive(false);
    }


}
