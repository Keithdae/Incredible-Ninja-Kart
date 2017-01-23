using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Panda;

public class ShootHandlerCtf : MonoBehaviour {

    [Range(0.0f, 100.0f)]
    public float accuracy = 20.0f;
    public float shootDelay = 0.56f;
    private float shootTimer;

    private MoveHandlerCtf mvHandler;
    private ShootingShuriken shoot;

	// Use this for initialization
	void Start () {
        shootTimer = shootDelay;
        mvHandler = GetComponent<MoveHandlerCtf>();
        shoot = GetComponent<ShootingShuriken>();
	}
	
    // Tasks for PandaBT
    [Task]
    void ShootEnemy()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer > shootDelay)
        {
            List<GameObject> inSight = mvHandler.GetEnemiesInSight();
            GameObject enemy = inSight[0];
            // Debug.Log("My target is "+ enemy.name, "my pos : " + transform.position + ", his pos : " + enemy.transform);
            Vector3 dir = enemy.transform.position - transform.position;
            shoot.Fire(dir.normalized);
            shootTimer = 0f;
        }
        Task.current.Succeed();
    }
}
