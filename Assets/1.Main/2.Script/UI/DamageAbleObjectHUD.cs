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
    public void SetHUD() //HUD 세팅
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
    public void SetTransform(Transform target) //좌표 설정
    {
        m_targtObj = target;
        transform.parent = m_targtObj;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        m_hudPos = Utill.GetChildObject(gameObject, "ObjectHUD");
    }
    void CreateText(float damage) //피해, 회복 등의 효과를 받을 때 화면상 표기해줄 Text Object 생성
    {
        var text = m_damageTextPool.Get();
        text.SetTransform(this);
        text.transform.position = transform.position;
        text.TextValue(damage);
        text.gameObject.SetActive(true);
    }
    public void SetKillCount(int count) //머신러닝 특성 등을 위한 처치한 적 카운트.
    {
        m_killCount.text = "처치한 적 : " + count;
    }
    public void EnqueDamageText(DamagedText text) //데미지텍스트를 풀에 다시 넣어줌
    {
        text.gameObject.SetActive(false);
        m_damageTextPool.Set(text);
    }
    public void DisplayDamage(float damage, float hp, float maxhp) //화면에 HUD표기해주는 메소드
    {
        Show(); //호출 되었을 때 게임오브젝트를 켜주고
        if (IsInvoking("Hide"))
        {
            CancelInvoke("Hide");
        }
        if (hp >= maxhp*0.95f)
        {
            Invoke("Hide", 5f); //체력이 95퍼센트 이상이라면 5초뒤 화면에서 사라지도록 인보크.
        }
        m_slider.SetSliderValue((int)maxhp, (int)hp);
        hp = Mathf.CeilToInt(hp);
        maxhp = Mathf.CeilToInt(maxhp);
        damage = Mathf.CeilToInt(damage);
        CreateText(damage);
    }
    void Show() //게임오브젝트 온
    {
        gameObject.SetActive(true);
    }
    void Hide() // 오프
    {
        gameObject.SetActive(false);
    }
    #endregion
}
