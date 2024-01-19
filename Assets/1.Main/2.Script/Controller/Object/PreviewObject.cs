using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PreviewObject : MonoBehaviour //������ ������Ʈ ���� ��ũ��Ʈ.
{
    #region Constants and Fields
    bool isCanBuild;
    bool isDetected = true;
    const float m_activeTime = 0.06f;
    MeshRenderer[] m_materials;
    BoxCollider col;
    List<Collider> m_object = new List<Collider>();
    #endregion

    #region Coroutine

    IEnumerator Coroutine_IsCanBuild() //�浹 ���� �˻縦 ���� ���尡 �������� üũ
    {
        while (true)
        {
            yield return new WaitForSeconds(m_activeTime);
            if(m_object.Count > 0) { isCanBuild = false;}
            else isCanBuild = true;
            if (isCanBuild)
            {
                for (int i = 0; i < m_materials.Length; i++)
                    m_materials[i].material.color = Color.green;
            }
            else
            {
                for (int i = 0; i < m_materials.Length; i++)
                    m_materials[i].material.color = Color.red;
            }
        }
    }
    #endregion

    #region Methods
    public void DeActive() //�ڷ�ƾ ���� �� ���ֱ�
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
    public void ActiveOBJ() // ������Ʈ ��Ƽ�� ���ְ� �ڷ�ƾ ����.
    {
        SetTransform();
        gameObject.SetActive(true);
        StartCoroutine(Coroutine_IsCanBuild());
    }
    public bool IsCanBuild() //�Ǽ� ���ɿ��� ����
    {
        return isCanBuild;
    }
   void  SetTransform() //��ǥ����
    {
        col = GetComponent<BoxCollider>();
        m_materials = GetComponentsInChildren<MeshRenderer>(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Background") || other.CompareTag("Tower") || other.CompareTag("Barricade") || other.CompareTag("Generator"))
        {
            m_object.Add(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Background") || other.CompareTag("Tower") || other.CompareTag("Barricade") || other.CompareTag("Generator"))
        {
            m_object.Remove(other);
        }
    }


    #endregion
}
