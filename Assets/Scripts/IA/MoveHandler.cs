using UnityEngine;
using System.Collections;
using Panda;
using System.Collections.Generic;

public class MoveHandler : MonoBehaviour {

    public NavMeshAgent navAgent;
    public float exploRange = 20.0f;
    public float sightRange = 50.0f;

    List<GameObject> enemies;
    List<GameObject> enemiesInSight;
    private int enemyLayer;

    // Use this for initialization
    void Start () {
        enemies = new List<GameObject>();
        enemiesInSight = new List<GameObject>();
        navAgent = GetComponent<NavMeshAgent>();
        enemyLayer = opponentLayer();
        GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach(GameObject go in gos)
        {
            if(go.layer == enemyLayer)
            {
                enemies.Add(go);
            }
        } 
    }

    // Tasks for the behaviour tree
    [Task]
    void Explore()
    {
        if (DestReached())
        {
            Vector3 dest;
            if(RandomPoint(transform.position, exploRange, out dest))
                navAgent.SetDestination(dest);
        }
        Task.current.Succeed();
    }


    [Task]
    void FollowEnemy()
    {
        Vector3 target = enemiesInSight[0].transform.position;
        navAgent.SetDestination(target);
        Task.current.Succeed();
    }

    [Task]
    void checkForEnemies()
    {
        enemiesInSight.Clear();
        foreach (GameObject enemy in enemies)
        {
            Vector3 dir = enemy.transform.position - transform.position;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir, out hit, sightRange))
            {
                Debug.Log("Hit this : " + hit.transform.gameObject.name);
                if(hit.transform.gameObject.layer == enemyLayer)
                    enemiesInSight.Add(hit.transform.gameObject);
            }
        }
        Task.current.Succeed();
    }

    [Task]
    void hasEnemyInSight()
    {
        Task.current.Complete(enemiesInSight.Count>0);
    }
    // Utility functions
    private bool RandomPoint(Vector3 center, float range, out Vector3 result) {
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


    private int opponentLayer()
    {
        return (gameObject.layer == 8) ? 9 : 8;
    }


}
