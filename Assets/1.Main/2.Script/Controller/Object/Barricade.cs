using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour, IDamageAbleObject
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
    Transform m_hudPos;
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
        SoundManager.Instance.PlaySFX("SFX_GunHit", m_audio);
        m_hp -= Mathf.CeilToInt(attack);
        m_hud.DisplayDamage(-damage, m_hp, m_maxHP);
        if (m_hp <= 0)
        {
            Destroyed();
        }
    }
    void Destroyed()
    {
        StopAllCoroutines();
        m_destroyd.SetActive(true);
        GameManager.Instance.DestroyTarget(gameObject); //���� ������ ��Ͽ��� ���ӿ�����Ʈ ����ó��
        Invoke("DestroyGameObject", 1f); // ������Ʈ Ǯ�����ֱ�
    }
    void DestroyGameObject() //Ȱ��ȭ ���� �� Ǯ�� �ٽ� �־��ֱ�
    {
        m_destroyd.SetActive(false);
        gameObject.SetActive(false);
        ObjectManager.Instance.SetBarricade(this);
    }
    public void RestoreHP(int heal) //����ȸ��
    {
        m_hp += (m_maxHP * heal) / 100;
        if (m_hp >= m_maxHP)
        {
            m_hp = m_maxHP;
        }
    }
    public void IncreaseMaxHp(int hp) //�ִ�ü�� ����
    {
        float value = m_hp / m_maxHP;
        m_maxHP += hp;
        m_hp = Mathf.CeilToInt(m_maxHP * value);
    }
    public void IncreaseDefence(int defence)
    {
        m_defence += defence;
    }
    public void SetTransform()
    {
        m_destroyd = Utill.GetChildObject(gameObject, "DestroyEffect").gameObject; //�ı��� Ȱ��ȭ ������ ������Ʈ
        m_audio = GetComponent<AudioSource>();
        m_hudPos = Utill.GetChildObject(gameObject, "HudPos");
        m_hud = ObjectManager.Instance.GetHud();
        m_hud.SetTransform(m_hudPos);
    }
    public void BuildBarricade(int hp, int def,Vector3 buildPos, float barirota, float hudrota)
    {
        m_maxHP = m_hp = hp;
        m_defence = def;
        transform.position = buildPos;
        transform.localEulerAngles = new Vector3(0f, barirota, 0f);
        m_hudPos.transform.localEulerAngles = new Vector3(30f,hudrota, 0f);
        gameObject.SetActive(true); //���ӿ�����Ʈ Ȱ��ȭ
        GameManager.Instance.SetGameObject(gameObject);//���� ������ ������Ʈ ��Ͽ� �߰����ֱ�
    }
}
