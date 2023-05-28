using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonDontDestroy<GameManager>
{
    [SerializeField]
    SoundManager soundManager;
    [SerializeField]
    UpdateManager updateManager;
    [SerializeField]
    UIManager uiManager;
    [SerializeField]
    GameManager gameManager;
    public void Hello()
    {
        UIManager.Instance.SystemMessageCantOpen("ㅇ안농!!");
    }
    private void Awake()
    {
        Application.targetFrameRate = 120; //일단은 타겟프레임만.
    }

}
