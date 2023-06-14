using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ImageLoader : Singleton<ImageLoader>
{
    public Dictionary<string, Sprite> m_weaponImages = new Dictionary<string, Sprite>(); //아이템의 이미지들을 저장

    public Sprite[] Tex;
    public Sprite GetImage(string path)
    {
       return m_weaponImages[path];
    }
    public void Load()
    {
        Tex = Resources.LoadAll<Sprite>("Image/Items/");
        for (int i = 0; i < Tex.Length; i++)
        {
            m_weaponImages.Add(Tex[i].name, Tex[i]);
        }

    }
}


