using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KartHealthPlayer : KartHealth {
    
    public Text healthText; // health text
    public Image damageImage; // flash image when hurt                     
    public float flashSpeed = 0.1f;
    public Color flashColour = new Color(255f, 0f, 0f, 0.1f);

    // Use this for initialization
    void Start () {
	
	}

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        damageImage.color = flashColour; // flash
        updateHealthUI();
    }

    protected virtual void Update()
    {
        if (currentHealth <= 0 && !dead)
        {
            onDeath();
        }
        if (transform.position.y < -25f && !dead)
        {
            onDeath();
        }
        damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
    }

    protected override void updateHealthUI()
    {
        base.updateHealthUI();
        healthText.text = "Health: " + slider.value + "%";
    }

    protected override void onDeath()
    {
        base.onDeath();
        cam.gameObject.GetComponent<CameraController>().StartCoroutine("OnPlayersDeath");
        StartCoroutine("Respawn");
    }

    IEnumerator Respawn()
    {
        setKartVisible(false);
        hcv.enabled = false;
        rig.useGravity = false;
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        yield return new WaitForSeconds(spawnDelay);
        setKartVisible(true);
        dead = false;
        hcv.enabled = true;
        rig.useGravity = true;
        currentHealth = startingHealth;
        updateHealthUI();
    }
}
