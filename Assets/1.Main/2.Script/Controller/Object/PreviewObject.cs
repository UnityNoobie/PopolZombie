using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PreviewObject : MonoBehaviour //������ ������Ʈ ���� ��ũ��Ʈ.
{
    #region Constants and Fields
    bool isCanBuild;
    MeshRenderer[] m_materials;
    BoxCollider col;
    List<GameObject> m_object = new List<GameObject>();
    #endregion

    #region Coroutine

    IEnumerator Coroutine_IsCanBuild() //�浹 ���� �˻縦 ���� ���尡 �������� üũ
    {
        while (true)
        {
            isCanBuild = true;
            col.enabled = true;
            yield return new WaitForSeconds(0.06f);
            col.enabled = false;
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
    private void OnTriggerEnter(Collider other) //�浹 ���� �˻��Ͽ� isCanBuild ����
    {
        if(other.CompareTag("Background") || other.CompareTag("Tower") || other.CompareTag("Barricade") || other.CompareTag("Generator"))
        {
            isCanBuild = false;
        }
    }
    #endregion
}
