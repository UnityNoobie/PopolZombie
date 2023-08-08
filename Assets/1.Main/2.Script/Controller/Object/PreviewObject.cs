using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PreviewObject : MonoBehaviour //프리뷰 오브젝트 관리 스크립트.
{
    #region Constants and Fields
    bool isCanBuild;
    MeshRenderer[] m_materials;
    BoxCollider col;
    List<GameObject> m_object = new List<GameObject>();
    #endregion

    #region Coroutine

    IEnumerator Coroutine_IsCanBuild() //충돌 여부 검사를 통해 빌드가 가능한지 체크
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
    private void OnTriggerEnter(Collider other) //충돌 여부 검사하여 isCanBuild 변경
    {
        if(other.CompareTag("Background") || other.CompareTag("Tower") || other.CompareTag("Barricade") || other.CompareTag("Generator"))
        {
            isCanBuild = false;
        }
    }
    #endregion
}
