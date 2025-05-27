using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffectDlg : MonoBehaviour
{
    public Text m_txtPlayerDamage = null;
    public Text m_txtEnemyDamage = null;

    public void Initialize()
    {
        m_txtEnemyDamage.transform.localPosition = new Vector3(585f, 100,0);
        m_txtPlayerDamage.transform.localPosition = new Vector3(-750, 100,0);
        HideUI(m_txtPlayerDamage.gameObject);
        HideUI(m_txtEnemyDamage.gameObject);
    }

    public void EnemyDamage(int damage)
    {
        string str = damage.ToString();
        ActivateUI(m_txtEnemyDamage.gameObject);
        m_txtEnemyDamage.text = str;
        m_txtEnemyDamage.color = new Color(1, 0, 0, 0);
        StartCoroutine(EnemyCor());
    }
    IEnumerator EnemyCor()
    {
        for(int i = 0; i < 3; i++)
        {
            m_txtEnemyDamage.color = new Color(1, 0, 0, 1);
            yield return new WaitForSeconds(0.4f);
            m_txtEnemyDamage.color = new Color(1, 0, 0, 0);
            yield return new WaitForSeconds(0.4f);
        }
        m_txtEnemyDamage.transform.localPosition = new Vector3(585f, 100, 0);
        HideUI(m_txtEnemyDamage.gameObject);
        yield return null;
    }
    public void PlayerDamage(int damage, int shield)
    {

        string str = damage.ToString();
        if (shield > 0)
        {
            str += $"(-{shield})";
        }
        ActivateUI(m_txtPlayerDamage.gameObject);
        m_txtPlayerDamage.text = str;
        m_txtPlayerDamage.color = new Color(1, 0, 0, 0);
        int realDamage = damage - shield;
        if (realDamage < 0) realDamage = 0;
        StartCoroutine(PlayerCor(realDamage));
    }
    IEnumerator PlayerCor(int realDamage)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == 1) { m_txtPlayerDamage.text = realDamage.ToString(); }
            m_txtPlayerDamage.color = new Color(1, 0, 0, 1);
            yield return new WaitForSeconds(0.4f);
            m_txtPlayerDamage.color = new Color(1, 0, 0, 0);
            yield return new WaitForSeconds(0.4f);
        }
        m_txtPlayerDamage.transform.localPosition = new Vector3(-750, 100, 0);
        HideUI(m_txtPlayerDamage.gameObject);
        yield return null;
    }
    public void HideUI(GameObject go)
    {
        go.SetActive(false);
    }
    public void ActivateUI(GameObject go)
    {
        go.SetActive(true);
    }
    float EaseOutExpo(float x)
    {
        return (x == 1) ? 1 : 1 - Mathf.Pow(2, -10 * x);
    }
}
