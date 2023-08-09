using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : AnimationController
{

    public enum Motion  //플레이어의 모션종류.
    {
        Idle,
        Movement,
        Fire,
        Reload,
        Die,
        PutRifle,
        GrabRifle,
        ShotGunReload,
        MeleeIdle,
        Combo1,
        Combo2,
        MeleeArm,
        Max
    }
    StringBuilder m_sb = new StringBuilder(); //ToString이용시 GC작동하여 렉발생 가능하므로 using System.Text; 추가하여 스트링빌더로 저장하여 사용! 
    [SerializeField]
    Motion m_motion; //현재 모션을 인스펙터창에 노출.
    public Motion GetMotion { get { return m_motion; } }
    public void SetMotionState(Motion motion) //현재 모션의 상태를 정의.
    {
        m_motion = motion;
    }
    public void Play(Motion motion, bool isBlend = true) // Play를통해 모션을 즉시 실행하기위해 필요. isBlend를 False로 할시 모션 블랜드되지 않고 즉시실행.
    {
        m_motion = motion; //현재 모션을 인스펙터창에 전달.
        m_sb.Append(motion); //스트링빌더에 현재 모션을 추가해줌
        Play(m_sb.ToString(), isBlend); //스트링빌더에 저장된 모션을 실행(블랜드 유무 확인후 실행)
        m_sb.Clear(); //GC 혹은 메모리낭비를 막기 위해 저장했떤 m_sb 정보 삭제처리.
    }
    protected override void Start() //AnimController의 Start를 실행해주기위함.
    {
        base.Start();
    }
}
