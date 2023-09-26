using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIQuickSlot : MonoBehaviour
{
    RawImage m_itemImage;
    TextMeshProUGUI m_itemCount;

    public void SetTransform()
    {
        m_itemImage = Utill.GetChildObject(gameObject,"ItemImage").GetComponent<RawImage>();
        m_itemCount = Utill.GetChildObject(gameObject,"ItemCount").GetComponent<TextMeshProUGUI>();
    }
    public void UpdateItemSlot(string image, int itemcount,bool isweapon)
    {
        if (isweapon)
        {
            m_itemImage.gameObject.SetActive(true);
            m_itemImage.texture = ImageLoader.Instance.GetImage(image).texture;
        }
        else if(itemcount <= 0) //갯수가 0개 이하면 이미지,갯수 삭제
        {
            m_itemImage.gameObject.SetActive(false);
            m_itemCount.gameObject.SetActive(false);
        }
        else //아니면 이미지 키고 이미지, 아이템 갯수 추가
        {
            m_itemImage.gameObject.SetActive(true);
            m_itemCount.gameObject.SetActive(true);
            m_itemCount.text = itemcount.ToString();
            m_itemImage.texture = ImageLoader.Instance.GetImage(image).texture;
        }
    }

}
