using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Panda;

public class KartHealthIA : KartHealth {
    private Canvas childCanvas;
    private NavMeshAgent navAg;

    private Vector3 spawnPos;

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
        foreach (Collider col in collids)
        {
            col.enabled = false;
        }
        goToSpawnPoint();
        spawnPos = transform.position;
        navAg.SetDestination(transform.position);
        navAg.enabled = false;
        yield return new WaitForSeconds(spawnDelay);
        setKartVisible(true);
        childCanvas.enabled = true;
        dead = false;
        rig.useGravity = true;
        foreach (Collider col in collids)
        {
            col.enabled = true;
        }
        navAg.enabled = true;
        currentHealth = startingHealth;
        updateHealthUI();
    }
    

    // For PandaBT
    [Task]
    void isAlive(){
        Task.current.Complete(!dead);
    }
}
