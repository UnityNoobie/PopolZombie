using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utill
{
   public static Transform GetChildObject(GameObject parent, string childName)
    {
        var childObjects = parent.GetComponentsInChildren<Transform>();
        for(int i = 0; i < childObjects.Length; i++) 
        {
            // if (childObjects[i].name == "name") 이런식으로 비교할경우 박싱이 일어나기 때문에 하면안됨!
            if (childObjects[i].name.Equals(childName))  //equals로 해줄것.
            {
                return childObjects[i];
            }
        }
        Debug.Log(childName + "의 이름이 잘못됐거나 없음.");
        return null;
    }
}
