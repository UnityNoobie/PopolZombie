using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    private void Awake()
    {
        Application.targetFrameRate = 120; //일단은 타겟프레임만.
       
    }

}
