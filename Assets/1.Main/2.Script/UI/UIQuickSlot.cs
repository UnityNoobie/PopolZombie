using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIQuickSlot : MonoBehaviour
{
    Image m_itemImage;
    TextMeshProUGUI m_itemCount;

    public void SetTransform()
    {
        m_itemImage = GetComponentInChildren<Image>(true);
        m_itemCount = Utill.GetChildObject(gameObject,"ItemCount").GetComponent<TextMeshProUGUI>();
    }
    public void UpdateItemSlot(string image, int itemcount)
    {
        if(itemcount <= 0) //갯수가 0개 이하면 이미지,갯수 삭제
        {
            m_itemImage.enabled = false;
            m_itemCount.enabled = false;
        }
        else //아니면 이미지 키고 이미지, 아이템 갯수 추가
        {
            m_itemImage.enabled = true;
            m_itemCount.enabled = true;
            m_itemCount.text = itemcount.ToString();
            m_itemImage.sprite = ImageLoader.Instance.GetImage(image);
        }
    }

}
