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
            // if (childObjects[i].name == "name") �̷������� ���Ұ�� �ڽ��� �Ͼ�� ������ �ϸ�ȵ�!
            if (childObjects[i].name.Equals(childName))  //equals�� ���ٰ�.
            {
                return childObjects[i];
            }
        }
        Debug.Log(childName + "�� �̸��� �߸��ưų� ����.");
        return null;
    }
}
