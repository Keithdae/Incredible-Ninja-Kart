using UnityEngine;
using System.Collections;
using Panda;

public class MoveHandler : MonoBehaviour {

    public NavMeshAgent navAgent;
    public float range = 20.0f;

    // Use this for initialization
    void Start () {
        navAgent = GetComponent<NavMeshAgent>();
    }

    // Tasks for the behaviour tree

    [Task]
    void Explore()
    {
        if (DestReached())
        {
            Vector3 dest;
            if(RandomPoint(transform.position, range, out dest))
                navAgent.SetDestination(dest);
        }
        Task.current.Succeed();
    }



    // Utility functions
    bool RandomPoint(Vector3 center, float range, out Vector3 result) {
        for (int i = 0; i < 30; i++) {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 5.0f, NavMesh.AllAreas)) {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    private bool DestReached()
    {
        return (Vector3.Distance(transform.position, navAgent.destination) <= navAgent.stoppingDistance * 2.0f) /*&& (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f)*/;
    }
}
