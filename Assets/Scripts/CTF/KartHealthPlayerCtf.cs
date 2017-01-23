using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KartHealthPlayerCtf : KartHealth {

    [HideInInspector]
    public Text healthText; // health text
    [HideInInspector]
    public Image damageImage; // flash image when hurt                     
    public float flashSpeed = 0.1f;
    public Color flashColour = new Color(255f, 0f, 0f, 0.1f);

    private FlagHold hold;

    private bool fellToDeath = false;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        hold = GetComponent<FlagHold>();
        updateHealthUI();
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
            fellToDeath = false;
            onDeath();
        }
        if (transform.position.y < -25f && !dead)
        {
            fellToDeath = true;
            onDeath();
        }
        damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
    }

    public override void updateHealthUI()
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
        foreach (Collider col in collids)
        {
            col.enabled = false;
        }
        // Drop the flag on the ground
        if (hold.hasFlag)
        {
            // On est mort sur le sol, il suffit de laisser le flag sur place
            if (!fellToDeath)
            {
                Debug.Log("DropFlag");
                hold.dropFlag();
            }
            else // On est tomber dans le vide avec le flag, il retourne a sa base
            {
                Debug.Log("DropFlagTobase");
                hold.dropFlagToBase();
            }
        }
        goToSpawnPoint();
        yield return new WaitForSeconds(spawnDelay);
        setKartVisible(true);
        dead = false;
        hcv.enabled = true;
        rig.useGravity = true;
        foreach (Collider col in collids)
        {
            col.enabled = true;
        }
        currentHealth = startingHealth;
        updateHealthUI();
    }
}
