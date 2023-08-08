using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MonsterAnimController : AnimationController //애니메이션컨트롤러를 상속받는 MonsterAnimController
{
    public enum Motion //몬스터의 모션(상태) 정의
    {
        Idle,
        Chase,
        Attack,
        Rage,
        Skill1,
        Skill2,
        Die,
        KnockBack,
        Max
    }
    StringBuilder m_sb = new StringBuilder();
    [SerializeField]
    Motion m_motion;
    public virtual Motion GetMotion { get { return m_motion; } }

    public void SetMotionState(Motion motion) //모션 설정
    {
        m_motion = motion;
    }
    public void Play(Motion motion, bool isBlend = true) //애니메이션 플레이, 현재 모션을 저장
    {
        m_motion = motion;
        m_sb.Append(motion);
        Play(m_sb.ToString(), isBlend);
        m_sb.Clear();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
}
