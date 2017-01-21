using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Panda;

public class ShootHandler : MonoBehaviour {

    [Range(0.0f, 100.0f)]
    public float accuracy = 20.0f;

    private MoveHandler mvHandler;
    private ShootingShuriken shoot;

	// Use this for initialization
	void Start () {
        mvHandler = GetComponent<MoveHandler>();
        shoot = GetComponent<ShootingShuriken>();
	}
	
    // Tasks for PandaBT
    [Task]
    void ShootEnemy()
    {
        List<GameObject> inSight = mvHandler.GetEnemiesInSight();
        foreach (GameObject enemy in inSight)
        {
            Vector3 dir = enemy.transform.position - transform.position;
            shoot.Fire(dir);
        }
        Task.current.Succeed();
    }
}
