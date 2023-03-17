using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BossAnimController : AnimationController
{
    [SerializeField]
    Motion m_motion;
   public enum Motion
    {
        Idle,
        Chase,
        Attack,
        Rage,
        Skill1,
        Skill2,
        Die,
        Max
    }

    public Motion GetMotion { get { return m_motion; } }
    StringBuilder m_sb = new StringBuilder();
    public void SetMotionState(Motion motion)
    {
        m_motion = motion;
    }
    public void Play(Motion motion, bool isBlend = true)
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
