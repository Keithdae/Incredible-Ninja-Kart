using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Panda;

public class KartHealthIA : KartHealth {
    private Canvas childCanvas;
    private NavMeshAgent navAg;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        childCanvas = GetComponentInChildren<Canvas>();
        navAg = GetComponentInChildren<NavMeshAgent>();
        updateHealthUI();
    }
	
	// Update is called once per frame
	protected void Update ()
    {
        if (currentHealth <= 0 && !dead)
        {
            onDeath();
        }
        if (transform.position.y < -25f && !dead)
        {
            onDeath();
        }
        childCanvas.transform.LookAt(cam.transform);
    }

    protected override void onDeath()
    {
        base.onDeath();
        StartCoroutine("Respawn");
    }

    IEnumerator Respawn()
    {
        setKartVisible(false);
        childCanvas.enabled = false;
        rig.useGravity = false;
        goToSpawnPoint();
        navAg.SetDestination(transform.position);
        yield return new WaitForSeconds(spawnDelay);
        setKartVisible(true);
        childCanvas.enabled = true;
        dead = false;
        rig.useGravity = true;
        currentHealth = startingHealth;
        updateHealthUI();
    }
    

    // For PandaBT
    [Task]
    void isAlive(){
        Task.current.Complete(!dead);
    }
}
