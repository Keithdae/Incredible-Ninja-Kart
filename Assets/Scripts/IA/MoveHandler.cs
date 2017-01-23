using UnityEngine;
using System.Collections;
using Panda;
using System.Collections.Generic;

public class MoveHandler : MonoBehaviour {

    public NavMeshAgent navAgent;
    public float exploRange = 20.0f;
    public float sightRange = 50.0f;

    public bool randExplo = false;

    [HideInInspector]
    public Transform flagAreaAlly;
    [HideInInspector]
    public Transform flagAreaEnemy;

    //[HideInInspector]
    public GameObject flagAlly;
    private FlagTrigger flagAllyTrigger;
    //[HideInInspector]
    public GameObject flagEnemy;


    [HideInInspector]
    public List<Transform> wpAllies;
    [HideInInspector]
    public List<Transform> wpEnemies;

    private List<GameObject> allies;
    private List<GameObject> enemies;
    private List<GameObject> enemiesInSight;
    private int enemyLayer;

    private FlagHold hold;

    private KartHealthIaCtf healthComp;

    private int currentWP = 0;

    // Use this for initialization
    void Start () {
        allies = new List<GameObject>();
        enemies = new List<GameObject>();
        enemiesInSight = new List<GameObject>();
        navAgent = GetComponent<NavMeshAgent>();
        hold = GetComponent<FlagHold>();
        flagAllyTrigger = flagAlly.GetComponent<FlagTrigger>();
        healthComp = GetComponent<KartHealthIaCtf>();
        enemyLayer = opponentLayer();
        GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach(GameObject go in gos)
        {
            if(go.layer == enemyLayer && !go.CompareTag("Untagged"))
            {
                enemies.Add(go);
            }
            if (go != gameObject && go.layer == gameObject.layer && !go.CompareTag("Untagged"))
            {
                allies.Add(go);
            }
        } 
    }

    // Tasks for PandaBT ---------------------------
    [Task]
    void Explore()
    {
        if (randExplo)
            ExploreRandom();
        else
            ExploreWaypoint();
        Task.current.Succeed();
    }

    private void ExploreRandom()
    {
        if (DestReached())
        {
            Vector3 dest;
            if(RandomPoint(transform.position, exploRange, out dest))
                navAgent.SetDestination(dest);
        }
    }

    private void ExploreWaypoint()
    {
        if (DestReached())
        {
            if (currentWP < wpEnemies.Count)
            {
                Vector3 dest;
                if (RandomPoint(wpEnemies[Random.Range(0, wpEnemies.Count)].position, 3.0f, out dest))
                {
                    navAgent.SetDestination(dest);
                }
            }
            else
            {
                Vector3 dest;
                if (RandomPoint(wpEnemies[currentWP].position, 3.0f, out dest))
                {
                    navAgent.SetDestination(dest);
                    currentWP++;
                }
            }
        }
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
                //Debug.Log("Hit this : " + hit.transform.gameObject.name);
                if(hit.transform.gameObject.layer == enemyLayer)
                    enemiesInSight.Add(hit.transform.gameObject);
            }
        }
        Task.current.Succeed();
    }

    [Task]
    void GoBackToBase()
    {
        navAgent.SetDestination(flagAreaAlly.position);
        Task.current.Succeed();
    }

    [Task]
    void hasEnemyInSight()
    {
        Task.current.Complete(enemiesInSight.Count>0);
    }

    [Task]
    void hasMultipleEnemyInSight()
    {
        Task.current.Complete(enemiesInSight.Count>1);
    }

    [Task]
    void hasFlag()
    {
        Task.current.Complete(hold.hasFlag);
    }

    [Task]
    void enemyFlagInSight()
    {
        Task.current.Complete(checkForEnemyFlag());
    }

    bool checkForEnemyFlag()
    {
        bool res = false;
        Vector3 dir = flagEnemy.transform.position - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, sightRange))
        {
            if (hit.transform.gameObject.layer == flagEnemy.layer && hit.transform.gameObject.CompareTag(flagEnemy.tag))
                res = true;
        }
        return res;
    }

    [Task]
    void GoToEnemyFlag()
    {
        moveTo(flagEnemy.transform.position);
        Task.current.Succeed();
    }

    [Task]
    void teamFlagPickable()
    {
        Task.current.Complete(checkForAllyFlag());
    }

    bool checkForAllyFlag()
    {
        // Le flag ne doit pas etre porte, ni a sa position de depart, et egalement etre en vue
        bool res = !flagAllyTrigger.isHeld() && !flagAllyTrigger.isAtStart();
        if (res)
        {
            Vector3 dir = flagAlly.transform.position - transform.position;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir, out hit, sightRange))
            {
                if (!(hit.transform.gameObject.layer == flagAlly.layer && hit.transform.gameObject.CompareTag(flagAlly.tag)))
                    res = false;
            }
        }
        return res;
    }

    [Task]
    void GoToTeamFlag()
    {
        moveTo(flagAlly.transform.position);
        Task.current.Succeed();
    }

    [Task]
    void lowHealth()
    {
        Task.current.Complete(healthComp.currentHealth < (healthComp.startingHealth / 2.0f));
    }
        
    // Utility functions ---------------------------
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

    public void moveTo(Vector3 pos)
    {
        navAgent.SetDestination(pos);
    }

    private bool DestReached()
    {
        return (Vector3.Distance(transform.position, navAgent.destination) <= navAgent.stoppingDistance * 2.0f) /*&& (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f)*/;
    }


    private int opponentLayer()
    {
        return (gameObject.layer == 8) ? 9 : 8;
    }


    public List<GameObject> GetAllies(){
        return allies;
    }

    public List<GameObject> GetEnemies(){
        return enemies;
    }

    public List<GameObject> GetEnemiesInSight(){
        return enemiesInSight;
    }


}
