using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ImageLoader : Singleton<ImageLoader>
{
    public Dictionary<string, Sprite> m_weaponImages = new Dictionary<string, Sprite>(); //�������� �̹������� ����

    public Sprite[] Tex;
    public Sprite GetImage(string path) //��ųʸ��� ����� �̹����� ��ȯ
    {
       return m_weaponImages[path];
    }
    public void Load() //�̹��� �ε���
    {
        Tex = Resources.LoadAll<Sprite>("Image/Items/"); //Resorces/Image/Items ������ �ִ� �̹������� ���� �ε�
        for (int i = 0; i < Tex.Length; i++) //��ųʸ��� �߰����ֱ�
        {
            m_weaponImages.Add(Tex[i].name, Tex[i]);
        }

    }
}


