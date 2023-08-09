using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TweenMove : MonoBehaviour //애니메이션 커브 조정하여 Tween효과 구현
{
    [SerializeField]
    NavMeshAgent m_navMeshAgent;  
    public Vector3 m_from;    
    public Vector3 m_to;
    [SerializeField]
    AnimationCurve m_curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    public float m_duration = 1f;    
    IEnumerator Coroutine_TweenProcess() 
    {
        float time = 0f;       
        while (true)
        {
            var value = m_curve.Evaluate(time);
            var result = m_from * (1f - value) + m_to * value;
            time += Time.deltaTime / m_duration;
            var dir = (result - transform.position);
            dir.y = 0f;
            m_navMeshAgent.Move(dir);
            if (time > 1f)
            {
                yield break;
            }
            yield return null;
        }
    }
    public void Play()
    {
        StopAllCoroutines();
        StartCoroutine("Coroutine_TweenProcess");
    }
    // Start is called before the first frame update
    void Start()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();    
    }

}
