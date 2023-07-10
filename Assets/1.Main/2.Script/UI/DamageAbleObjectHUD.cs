using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class DamageAbleObjectHUD : MonoBehaviour
{
    #region Constants and Fields
    Slider m_hpSlider;
    TextMeshProUGUI m_hpValue;
    GameObject m_damagedText;
    GameObjectPool<DamagedText> m_damageTextPool = new GameObjectPool<DamagedText>();
    Transform m_targtObj;
    Transform m_hudPos;
    #endregion

   
    #region Methods
    public void SetHUD()
    {
        m_hpSlider = GetComponentInChildren<Slider>(true);
        m_hpValue = Utill.GetChildObject(gameObject, "ValueText").GetComponent<TextMeshProUGUI>();
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
    public void SetTransform(Transform target)
    {
        m_targtObj = target;
        transform.parent = m_targtObj;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        m_hudPos = Utill.GetChildObject(gameObject, "ObjectHUD");
    }
    void CreateText(float damage)
    {
        var text = m_damageTextPool.Get();
        text.SetTransform(this);
        text.transform.position = transform.position;
        text.TextValue(damage);
        text.gameObject.SetActive(true);
    }
    public void EnqueDamageText(DamagedText text)
    {
        text.gameObject.SetActive(false);
        m_damageTextPool.Set(text);
    }
    public void DisplayDamage(float damage, float hp, float maxhp)
    {
        Show();
        if (IsInvoking("Hide"))
        {
            CancelInvoke("Hide");
        }
        if (hp >= maxhp*0.95f)
        {
            Invoke("Hide", 5f);
        }
        m_hpValue.text = hp + "/" + maxhp;
        m_hpSlider.value = hp/maxhp;
        CreateText(damage);
    }
    void Show()
    {
        gameObject.SetActive(true);
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }
    #endregion
}
