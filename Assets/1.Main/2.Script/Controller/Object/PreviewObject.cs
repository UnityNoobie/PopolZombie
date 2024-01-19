using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PreviewObject : MonoBehaviour //프리뷰 오브젝트 관리 스크립트.
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

    IEnumerator Coroutine_IsCanBuild() //충돌 여부 검사를 통해 빌드가 가능한지 체크
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
    public void DeActive() //코루틴 종료 후 꺼주기
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
    public void ActiveOBJ() // 오브젝트 액티브 켜주고 코루틴 시작.
    {
        SetTransform();
        gameObject.SetActive(true);
        StartCoroutine(Coroutine_IsCanBuild());
    }
    public bool IsCanBuild() //건설 가능여부 리턴
    {
        return isCanBuild;
    }
   void  SetTransform() //좌표지정
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
