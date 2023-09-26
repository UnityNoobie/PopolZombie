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
        else if(itemcount <= 0) //������ 0�� ���ϸ� �̹���,���� ����
        {
            m_itemImage.gameObject.SetActive(false);
            m_itemCount.gameObject.SetActive(false);
        }
        else //�ƴϸ� �̹��� Ű�� �̹���, ������ ���� �߰�
        {
            m_itemImage.gameObject.SetActive(true);
            m_itemCount.gameObject.SetActive(true);
            m_itemCount.text = itemcount.ToString();
            m_itemImage.texture = ImageLoader.Instance.GetImage(image).texture;
        }
    }

}
