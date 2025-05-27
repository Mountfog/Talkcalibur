using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodUIDlg : MonoBehaviour
{
    public Text m_txtBlood = null;

    public void BloodUpdate()
    {
        int blood = GameMgr.Inst.ginfo.blood;
        m_txtBlood.text = blood.ToString();
    }
}
