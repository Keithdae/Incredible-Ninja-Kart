using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Panda;

public class KartHealthIaCtf : KartHealth {
    private Canvas childCanvas;
    private NavMeshAgent navAg;

    private FlagHold hold;

    private bool fellToDeath = false;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        childCanvas = GetComponentInChildren<Canvas>();
        navAg = GetComponentInChildren<NavMeshAgent>();
        slider = GetComponentInChildren<Slider>();
        hold = GetComponent<FlagHold>();
        updateHealthUI();
    }
	
	// Update is called once per frame
	protected void Update ()
    {
        if (currentHealth <= 0 && !dead)
        {
            fellToDeath = false;
            onDeath();
        }
        if (transform.position.y < -25f && !dead)
        {
            fellToDeath = true;
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
        // Drop the flag on the ground
        if (hold.hasFlag)
        {
            // On est mort sur le sol, il suffit de laisser le flag sur place
            if (!fellToDeath)
                hold.dropFlag();
            else // On est tomber dans le vide avec le flag, il retourne a sa base
                hold.dropFlagToBase();
        }
        goToSpawnPoint();
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
    void isAliveCtf(){
        Task.current.Complete(!dead);
    }
}
