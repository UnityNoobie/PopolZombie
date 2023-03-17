using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using UnityEngine;

public class AttackAreaUnitFind : MonoBehaviour
{
    public  List<GameObject> m_unitList = new List<GameObject>();
    // Start is called before the first frame update
    public void Resetlist()
    {
        for(int i = m_unitList.Count -1 ; i >= 0; i--)
        {
            m_unitList.Remove(m_unitList[i]);
        }
       // m_unitList.Clear(); 클리어는 가비지가 생성되었던걸로 기억
      //  Debug.Log("리셋적용되었음을 알림");
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Zombie"))
        {
            m_unitList.Add(other.gameObject);
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            m_unitList.Remove(other.gameObject);
        }
        if (other.CompareTag("Die"))
        {
            m_unitList.Remove(other.gameObject);
        }
    }
    
}
