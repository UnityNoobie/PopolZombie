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
        if(itemcount <= 0) //������ 0�� ���ϸ� �̹���,���� ����
        {
            m_itemImage.enabled = false;
            m_itemCount.enabled = false;
        }
        else //�ƴϸ� �̹��� Ű�� �̹���, ������ ���� �߰�
        {
            m_itemImage.enabled = true;
            m_itemCount.enabled = true;
            m_itemCount.text = itemcount.ToString();
            m_itemImage.sprite = ImageLoader.Instance.GetImage(image);
        }
    }

}
