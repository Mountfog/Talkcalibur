using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackGroundMgr : MonoBehaviour
{
    public List<Transform> SkyBgs = new List<Transform>();
    public List<Transform> GroundBgs = new List<Transform>();
    [SerializeField] private float skySpeed = 2f;
    [SerializeField] private float groundSpeed = 2f;
    [SerializeField] private float scale = 1f;
    float m_LeftPos;
    float m_Size = 0;
    float m_GroundPos;
    float m_GroundSize;
    public bool isBGMove = true;

    // Start is called before the first frame update
    public void SetReadyState()
    {
        for(int i = 0; i < 3; i++)
        {
            SkyBgs[i].position = new Vector3(26 * i, 0, 0);
            GroundBgs[i].position = new Vector3(26 * i, -1, 0);
        }
        float fLen = SkyBgs[1].position.x - SkyBgs[0].position.x; //28
        float x = SkyBgs[0].position.x - fLen; //-14
        m_LeftPos = x;
        m_Size = fLen * SkyBgs.Count;
        isBGMove = false;
        float groundLen = GroundBgs[1].position.x - GroundBgs[0].position.x;
        float groundX = GroundBgs[0].position.x - groundLen;
        m_GroundPos = groundX;
        m_GroundSize = groundLen * GroundBgs.Count;
    }
    public void SetWaveState()
    {
        isBGMove = true;
    }
    public void SetActionState()
    {
        isBGMove = false;
    }

    public void SetStage()
    {
        int stage = GameMgr.Inst.ginfo.CurStage;
        Color32 color = Color.cyan;
        if (stage == 1) 
        {
            color = Color.white;
        }
        if (stage == 2)
        {
            color = new Color32((byte)255, (byte)150, (byte)150, (byte)255);
        }
        if (stage == 3)
        {
            color = new Color32((byte)255, (byte)100, (byte)100, (byte)255);
        }
        for(int i=0;i<SkyBgs.Count;i++)
        {
            SkyBgs[i].GetComponent<Tilemap>().color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameMgr.Inst.gameScene.m_battleFSM.IsWaveState() && isBGMove)
        {
            for (int i = 0; i < SkyBgs.Count; i++)
            {
                SkyBgs[i].Translate(skySpeed * Time.deltaTime * Vector3.left, Space.World);
                if (SkyBgs[i].position.x <= m_LeftPos)
                {
                    SkyBgs[i].position = new Vector3(52, 0, 0);
                }
            }
            for (int i = 0; i < GroundBgs.Count; i++)
            {
                GroundBgs[i].Translate(groundSpeed * Time.deltaTime * Vector3.left, Space.World);
                if (GroundBgs[i].position.x <= m_GroundPos)
                {
                    GroundBgs[i].position = new Vector3(52, -1, 0);
                }
            }
        }
    }
}
