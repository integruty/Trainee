using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{
    public GameObject target;
    public float dist;
    NavMeshAgent nav;
    public float distTrigger=5;
    // Start is called before the first frame update
    void Start()
    {
        nav=GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(target.transform.position, transform.position);
        if (1f>dist&dist > distTrigger) 
        { nav.enabled = false; }
        gameObject.GetComponent<Animator>().SetTrigger("idle");
        if (1f < dist&dist < distTrigger) 
        { 
            nav.enabled = true;
            nav.SetDestination(target.transform.position);
            gameObject.GetComponent<Animator>().SetTrigger("walk");

        }
        if (.5f < dist & dist < 1) 
        {
            nav.enabled= false;
            gameObject.GetComponent<Animator>().SetTrigger("attack");

        }
    }
}
