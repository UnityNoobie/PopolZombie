using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour , IDamageAbleObject
{
    #region Constants and Fields
    [SerializeField]
    int m_maxHP;
    [SerializeField]
    int m_hp;
    [SerializeField]
    int m_defence;
    [SerializeField]
    int m_hprestore;
    GameObject m_destroyd;
    AudioSource m_audio;
    DamageAbleObjectHUD m_hud;
    Camera m_uiCam;

    #endregion

    #region Coroutine
    IEnumerator Coroutine_FixGen()
    {
        while (true)
        {
            RestoreHP(m_hprestore);
            yield return new WaitForSeconds(5);
        }
    }
    #endregion
    public void SetDamage(float damage)
    {
        float attack = CalculationDamage.NormalDamage(damage, m_defence, 0f);
        UGUIManager.Instance.SystemMessageSendMessage("발전기가 공격받고 있습니다!!");
        SoundManager.Instance.PlaySFX("SFX_Generator", m_audio);
        m_hp -= Mathf.CeilToInt(attack);
        m_hud.DisplayDamage(damage, m_hp,m_maxHP);
        if(m_hp <= 0)
        {
            Destroyed();
        }
    }
    void Destroyed()
    {
        StopAllCoroutines();
        m_destroyd.SetActive(true);        
        GameManager.Instance.DestroyTarget(gameObject); //공격 가능한 목록에서 게임오브젝트 삭제처리
        GameManager.Instance.GameOver();
    }
    public void RestoreHP(int heal) //피해회복
    {
        m_hp += (m_maxHP * heal) / 100;
        if(m_hp >= m_maxHP)
        {
            m_hp = m_maxHP;
        }
    }
    public void IncreaseMaxHp(int hp) //최대체력 증가
    {
        float value = m_hp / m_maxHP;
        m_maxHP += hp;
        m_hp = Mathf.CeilToInt(m_maxHP * value);
    }
    public void IncreaseDefence(int defence)
    {
        m_defence += defence;
    }
    private void Start()
    {
        m_maxHP = m_hp = 1000;
        m_defence = 30;
        GameManager.Instance.SetGameObject(gameObject);
        m_destroyd = Utill.GetChildObject(gameObject, "DestroyEffect").gameObject;
        m_audio = GetComponent<AudioSource>();
        //  m_uiCam = UGUIManager.Instance.GetUICam();
        m_uiCam = Camera.main;
        m_hud = ObjectManager.Instance.GetHud();
        m_hud.SetTransform(m_uiCam,transform);
    }
}
