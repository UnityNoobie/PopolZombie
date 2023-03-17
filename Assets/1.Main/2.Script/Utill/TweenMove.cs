using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TweenMove : MonoBehaviour
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

    // Update is called once per frame
   /* void Update()
    {        
        if(Input.GetKeyDown(KeyCode.O))
        {
            m_from = transform.position;
            m_to = m_from + Vector3.forward * 3f;
            m_duration = 0.5f;
            Play();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            m_from = transform.position;
            m_to = m_from + Vector3.back * 3f;
            m_duration = 0.5f;
            Play();
        }
    }*/
}
