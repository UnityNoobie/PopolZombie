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
    [SerializeField]
    Slider m_hpSlider;
    [SerializeField]
    TextMeshProUGUI m_hpValue;
    [SerializeField]
    GameObject m_damagedText;
    GameObjectPool<DamagedText> m_damageTextPool = new GameObjectPool<DamagedText>();
    Camera m_mainCam;
    Camera m_uiCam;
    Transform m_targtObj;
    #endregion
   
    #region Methods
    public void SetHUD()
    {
        m_hpSlider = GetComponent<Slider>();
        m_hpValue = Utill.GetChildObject(gameObject, "ValueText").GetComponent<TextMeshProUGUI>();
        m_damagedText = Resources.Load<GameObject>("Prefabs/DamageText");
        m_damageTextPool = new GameObjectPool<DamagedText>(5, () =>
        {
            var obj = Instantiate(m_damagedText);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.gameObject.SetActive(false);
            var text = obj.GetComponent<DamagedText>();
            return text;
        });
    }

    public void SetTransform(Camera uiCam,Transform target)
    {
        m_targtObj = target;
        m_mainCam = Camera.main;
        m_uiCam = uiCam;
        Vector3 Pos = m_mainCam.WorldToScreenPoint(transform.position);
        Pos = m_uiCam.ViewportToWorldPoint(Pos);
        Pos.z = 0f;
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
        Invoke("Hide", 5f);
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
