using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : AnimationController
{

    public enum Motion  //�÷��̾��� �������.
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
    StringBuilder m_sb = new StringBuilder(); //ToString�̿�� GC�۵��Ͽ� ���߻� �����ϹǷ� using System.Text; �߰��Ͽ� ��Ʈ�������� �����Ͽ� ���! 
    [SerializeField]
    Motion m_motion; //���� ����� �ν�����â�� ����.
    public Motion GetMotion { get { return m_motion; } }
    public void SetMotionState(Motion motion) //���� ����� ���¸� ����.
    {
        m_motion = motion;
    }
    public void Play(Motion motion, bool isBlend = true) // Play������ ����� ��� �����ϱ����� �ʿ�. isBlend�� False�� �ҽ� ��� ������� �ʰ� ��ý���.
    {
        m_motion = motion; //���� ����� �ν�����â�� ����.
        m_sb.Append(motion); //��Ʈ�������� ���� ����� �߰�����
        Play(m_sb.ToString(), isBlend); //��Ʈ�������� ����� ����� ����(���� ���� Ȯ���� ����)
        m_sb.Clear(); //GC Ȥ�� �޸𸮳��� ���� ���� �����߶� m_sb ���� ����ó��.
    }
    protected override void Start() //AnimController�� Start�� �������ֱ�����.
    {
        base.Start();
    }
}
