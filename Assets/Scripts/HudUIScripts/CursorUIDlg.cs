using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CursorUIDlg : MonoBehaviour
{
    public enum InfoType
    {
        none = 0,
        icon = 1,
        word = 2,
        host = 3,
        enemy = 4,
    }
    public GameObject m_panel = null;
    public InfoType curInfoType = InfoType.none;
    public Vector3 panelPos = Vector3.zero;
    public Vector3 panelMiddle = Vector3.zero;
    public Vector3 transRestrict = Vector3.zero;


    [SerializeField] Canvas canvas = null;

    [Header("Player또는 Enemy표시에 쓰이는 UI")]
    public GameObject m_UnitUIGroup = null;
    public Text m_txtTitle = null;
    public Image m_hostImage = null;
    public Text m_txtHp = null;
    public Text m_txtAtk = null;
    public Text m_TxtLevel = null;

    [Header("Player에만 쓰이는 UI")]
    public GameObject m_PlayerUIGroup = null;
    public Text m_txtTrust = null;
    public Slider m_sliderTrust = null;
    public Slider m_sliderEXP = null;

    [Header("Word표시에 쓰이는 UI")]
    public GameObject m_WordUIGroup = null;
    public Text m_txtWord = null;
    public Text m_txtWordInst = null;

    [Header("Icon표시에 쓰이는 UI")]
    //체력,공격력,레벨,신뢰도
    public GameObject m_IconUIGroup = null;
    public Image m_icon = null;
    public Text m_infoTitle = null;
    public Text m_infoInst = null;
    public IconType curIconType = IconType.None;
    public List<Sprite> m_iconSprites = new List<Sprite>();

    public enum IconType
    {
        None = -1,
        HP = 0,
        Attack = 1,
        Level = 2,
        Trust = 3,
        Blood = 4,
    }


    public void Initialize()
    {
        m_txtWord.text = string.Empty;
        m_txtWordInst.text = string.Empty;
        HideUI(m_UnitUIGroup);
        HideUI(m_PlayerUIGroup);
        HideUI(m_WordUIGroup);
        HideUI(m_IconUIGroup);
    }

    public void HostInfoUpdate()
    {
        ActivateUI(m_UnitUIGroup);
        ActivateUI(m_PlayerUIGroup);
        HideUI(m_WordUIGroup);
        HideUI(m_IconUIGroup);
        GameInfo gInfo = GameMgr.Inst.ginfo;
        int hostID = gInfo.hostID;
        int hostHp = gInfo.HostHp;
        int hostMaxHp = gInfo.hostMaxHp;
        int hostMin = gInfo.minHostAttack;
        int hostMax = gInfo.maxHostAttack;
        int hosttrust = gInfo.hostTrust;
        int hostLevel = gInfo.hostLevel;
        int hostEXP = gInfo.hostEXP;
        m_hostImage.sprite = GameMgr.Inst.gameScene.m_gameUI.m_hostSprites[hostID];
        m_txtHp.text = string.Format("{0} / {1}", hostHp, hostMaxHp);
        m_txtAtk.text = string.Format("{0}-{1}", hostMin, hostMax);
        m_TxtLevel.text = string.Format("Level {0}", hostLevel);
        m_sliderEXP.maxValue = gInfo.EXPNeededForLevelUp;
        m_sliderEXP.value = hostEXP;
        m_sliderTrust.value = hosttrust;
        float r = (hosttrust <= 25) ? 255 : 255 - 255 * ((float)hosttrust / 25);
        float g = (hosttrust <= 25) ? 255 * ((float)hosttrust / 25) : 255;
        m_sliderTrust.fillRect.GetComponent<Image>().color = new Color32((byte)r, (byte)g, 0, 255);
        m_txtTitle.text = "플레이어 정보";
    }
    public void EnemyInfoUpdate()
    {
        ActivateUI(m_UnitUIGroup);
        HideUI(m_PlayerUIGroup);
        HideUI(m_WordUIGroup);
        HideUI(m_IconUIGroup);
        Enemy curEnemy = GameMgr.Inst.gameScene.m_gameUI.m_enemyMgr.m_curEnemy;
        if(curEnemy is null)
        {
            return;
        }
        int hostID = curEnemy.enemyID;
        int hostLevel = curEnemy.enemyLevel;
        int hostMaxHp = 50 + (hostLevel * 10);
        int hostHp = (curEnemy.enemyHp < 0) ? 0 : curEnemy.enemyHp;
        int hostMin = curEnemy.minEnemyAttack;
        int hostMax = curEnemy.maxEnemyAttack;

        m_hostImage.sprite = GameMgr.Inst.gameScene.m_gameUI.m_hostSprites[hostID];
        m_txtHp.text = string.Format("{0} / {1}", hostHp, hostMaxHp);
        m_txtAtk.text = string.Format("{0}-{1}", hostMin, hostMax);
        m_TxtLevel.text = string.Format("Level {0}", hostLevel);
        m_txtTitle.text = "적 정보";
    }

    public void IconInfoUpdate()
    {
        HideUI(m_UnitUIGroup);
        HideUI(m_PlayerUIGroup);
        HideUI(m_WordUIGroup);
        ActivateUI(m_IconUIGroup);   
    }
    public void WordInfoUpdate()
    {
        HideUI(m_UnitUIGroup);
        HideUI(m_PlayerUIGroup);
        ActivateUI(m_WordUIGroup);
        HideUI(m_IconUIGroup);
    }

    public void OnHoverUI(int i,string title, string info)
    {
        curIconType = (IconType)i;
        m_icon.sprite = m_iconSprites[i];
        m_infoTitle.text = title;
        m_infoInst.text = info;
    }
    public void OnHoverWord(int i)
    {
        AssetWord word = AssetMgr.Inst.GetAssetWord(i);
        m_txtWord.text = word.basicWord;
        m_txtWordInst.text = word.desc;
    }
    public void OnExitWord()
    {
        m_txtWord.text = string.Empty;
        m_txtWordInst.text = string.Empty;
    }
    public void OnHoverExitUI()
    {
        curIconType = IconType.None;
        m_infoTitle.text = string.Empty;
        m_infoInst.text = string.Empty;
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
            str = "절대적인 신뢰";
        return str;
    }
    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            RectTransform rect = canvas.transform as RectTransform;
            Vector2 mouse = Input.mousePosition;
            Vector2 targetPos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, mouse, canvas.worldCamera, out targetPos);
            RectTransform curRect = gameObject.transform as RectTransform;
            curRect.anchoredPosition = targetPos;
            PanelLocationRearrange(); 

            Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 터치한 좌표를 가져 옴
            Ray2D ray = new Ray2D(wp, Vector2.zero); // 원점에서 터치한 좌표 방향으로 Ray를 쏨
            float distance = Mathf.Infinity;
            RaycastHit2D hitPlayer = Physics2D.Raycast(ray.origin, ray.direction, distance, 1 << LayerMask.NameToLayer("Player"));
            RaycastHit2D hitEnemy = Physics2D.Raycast(ray.origin, ray.direction, distance, 1 << LayerMask.NameToLayer("Enemy"));
            // ray의 시작점에서, ray 방향으로, 최대 distance까지 감지하며, "drawer"라는 이름을 가진 레이어에 해당하는 오브젝트만 감지한다.

            if (hitPlayer)
            {
                curInfoType = InfoType.host;
                HostInfoUpdate();
            }
            else if (hitEnemy)
            {
                curInfoType = InfoType.enemy;
                EnemyInfoUpdate();
            }
            else if(curIconType != IconType.None)
            {
                curInfoType |= InfoType.icon;
                IconInfoUpdate();
            }
            else if(m_txtWord.text != string.Empty)
            {
                WordInfoUpdate();
            }
            else
            {
                curInfoType = InfoType.none;
                HideUI(m_UnitUIGroup);
                HideUI(m_PlayerUIGroup);
                HideUI(m_WordUIGroup);
                HideUI(m_IconUIGroup);
            }
        }
        else
        {
            HideUI(m_UnitUIGroup);
            HideUI(m_PlayerUIGroup);
            HideUI(m_WordUIGroup);
            HideUI(m_IconUIGroup);
        }

    }

    public void PanelLocationRearrange()
    {
        float x = panelPos.x;
        float y = panelPos.y;
        if (transform.localPosition.x >= panelMiddle.x)
        {
            x *= -1;
        }
        else if (transform.localPosition.x >= panelMiddle.x * -1)
        {
            x = 0;
        }
        if (transform.localPosition.y >= panelMiddle.y)
        {
            y *= -1;
        }
        else if (transform.localPosition.y >= panelMiddle.y * -1)
        {
            y = 0;
        }
        m_panel.transform.localPosition = new Vector3(x, y, 0);
    }
    public void HideUI(GameObject go)
    {
        go.SetActive(false);
    }
    public void ActivateUI(GameObject go)
    {
        go.SetActive(true);
    }
}
