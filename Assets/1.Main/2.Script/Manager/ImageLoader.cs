using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ImageLoader : Singleton<ImageLoader>
{
    public Dictionary<string, Sprite> m_weaponImages = new Dictionary<string, Sprite>(); //아이템의 이미지들을 저장

    public Sprite[] Tex;
    public Sprite GetImage(string path) //딕셔너리에 저장된 이미지를 반환
    {
       return m_weaponImages[path];
    }
    public void Load() //이미지 로드기능
    {
        Tex = Resources.LoadAll<Sprite>("Image/Items/"); //Resorces/Image/Items 폴더에 있는 이미지들을 전부 로드
        for (int i = 0; i < Tex.Length; i++) //딕셔너리에 추가해주기
        {
            m_weaponImages.Add(Tex[i].name, Tex[i]);
        }

    }
}


