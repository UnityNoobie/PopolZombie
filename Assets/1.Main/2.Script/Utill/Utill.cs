using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utill
{
   public static Transform GetChildObject(GameObject parent, string childName) //자식 오브젝트를 찾는 메소드
    {
        var childObjects = parent.GetComponentsInChildren<Transform>(true);
        for(int i = 0; i < childObjects.Length; i++) 
        {
            if (childObjects[i].name.Equals(childName))  
            {
                return childObjects[i];
            }
        }
        Debug.Log(childName + "의 이름이 잘못됐거나 없음.");
        return null;
    }
}
