using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class DamageAbleObjectHUD : MonoBehaviour
{
    #region Constants and Fields
    UISliderController m_slider;
    TextMeshProUGUI m_killCount;
    GameObject m_damagedText;
    GameObjectPool<DamagedText> m_damageTextPool = new GameObjectPool<DamagedText>();
    Transform m_targtObj;
    Transform m_hudPos;
    #endregion

   
    #region Methods
    public void SetHUD() //HUD ����
    {
        m_slider = GetComponentInChildren<UISliderController>(true);
        m_slider.SetTransform();
        m_killCount = Utill.GetChildObject(gameObject, "KillCount").GetComponent<TextMeshProUGUI>() ;
        m_damagedText = Resources.Load<GameObject>("Prefabs/DamageText");
        m_damageTextPool = new GameObjectPool<DamagedText>(5, () =>
        {
            var obj = Instantiate(m_damagedText);
            obj.transform.SetParent(m_hudPos);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.gameObject.SetActive(false);
            var text = obj.GetComponent<DamagedText>();
            return text;
        });
    }
    public void SetTransform(Transform target) //��ǥ ����
    {
        m_targtObj = target;
        transform.parent = m_targtObj;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        m_hudPos = Utill.GetChildObject(gameObject, "ObjectHUD");
    }
    void CreateText(float damage) //����, ȸ�� ���� ȿ���� ���� �� ȭ��� ǥ������ Text Object ����
    {
        var text = m_damageTextPool.Get();
        text.SetTransform(this);
        text.transform.position = transform.position;
        text.TextValue(damage);
        text.gameObject.SetActive(true);
    }
    public void SetKillCount(int count) //�ӽŷ��� Ư�� ���� ���� óġ�� �� ī��Ʈ.
    {
        m_killCount.text = "óġ�� �� : " + count;
    }
    public void EnqueDamageText(DamagedText text) //�������ؽ�Ʈ�� Ǯ�� �ٽ� �־���
    {
        text.gameObject.SetActive(false);
        m_damageTextPool.Set(text);
    }
    public void DisplayDamage(float damage, float hp, float maxhp) //ȭ�鿡 HUDǥ�����ִ� �޼ҵ�
    {
        Show(); //ȣ�� �Ǿ��� �� ���ӿ�����Ʈ�� ���ְ�
        if (IsInvoking("Hide"))
        {
            CancelInvoke("Hide");
        }
        if (hp >= maxhp*0.95f)
        {
            Invoke("Hide", 5f); //ü���� 95�ۼ�Ʈ �̻��̶�� 5�ʵ� ȭ�鿡�� ��������� �κ�ũ.
        }
        m_slider.SetSliderValue((int)maxhp, (int)hp);
        hp = Mathf.CeilToInt(hp);
        maxhp = Mathf.CeilToInt(maxhp);
        damage = Mathf.CeilToInt(damage);
        CreateText(damage);
    }
    void Show() //���ӿ�����Ʈ ��
    {
        gameObject.SetActive(true);
    }
    void Hide() // ����
    {
        gameObject.SetActive(false);
    }
    #endregion
}
