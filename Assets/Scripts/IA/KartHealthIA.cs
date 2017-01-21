using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KartHealthIA : KartHealth {
    private Canvas childCanvas;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        childCanvas = GetComponentInChildren<Canvas>();
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
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        yield return new WaitForSeconds(spawnDelay);
        setKartVisible(true);
        childCanvas.enabled = true;
        dead = false;
        rig.useGravity = true;
        currentHealth = startingHealth;
        updateHealthUI();
    }
}
