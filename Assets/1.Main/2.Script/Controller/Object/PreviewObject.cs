using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    bool isCanBuild;
    MeshRenderer[] m_materials;
    BoxCollider col;
    List<GameObject> m_object = new List<GameObject>();

    IEnumerator Coroutine_IsCanBuild()
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
    public void DeActive()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
    public void ActiveOBJ()
    {
        SetTransform();
        gameObject.SetActive(true);
        StartCoroutine(Coroutine_IsCanBuild());
    }
    public bool IsCanBuild()
    {
        return isCanBuild;
    }
   void  SetTransform()
    {
        col = GetComponent<BoxCollider>();
        m_materials = GetComponentsInChildren<MeshRenderer>(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Background") || other.CompareTag("Tower") || other.CompareTag("Barricade") || other.CompareTag("Generator"))
        {
            isCanBuild = false;
        }
    }
}
