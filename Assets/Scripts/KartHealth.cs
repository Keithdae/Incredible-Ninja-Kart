using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KartHealth : MonoBehaviour {

	public float startingHealth = 100f;
    //[HideInInspector]
	public float currentHealth;
	[HideInInspector]
	public bool dead;

	public Slider slider; // health slider 
	public Image fillImage; // fill image of health slider
    public Text healthText; // health text
	public Color zeroHealthColor; 
	public Color fullHealthColor;
    public Image damageImage; // flash image when hurt                     
    public float flashSpeed = 0.1f;                     
    public Color flashColour = new Color(255f, 0f, 0f, 0.1f);
    public ParticleSystem explosion;

    public Transform[] spawnPoints;
    private HoverCarControl hcv;
    private Rigidbody rig;
    private Transform[] children;

    void Start(){
		currentHealth = startingHealth;
		dead = false;
		updateHealthUI ();
        hcv = GetComponent<HoverCarControl>();
        rig = GetComponent<Rigidbody>();
        children = GetComponentsInChildren<Transform>();
	}

	// Use this for initialization
	private void OnEnable () {
		currentHealth = startingHealth;
		dead = false;
	}

    void Update()
    {
        if (transform.gameObject.tag == "Player")
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        if (currentHealth <= 0 && !dead) 
        {
            onDeath ();
        }
    }

	public void TakeDamage(float damage){
		currentHealth -= damage;
        if (transform.gameObject.tag == "Player")
        {
            damageImage.color = flashColour; // flash
        }
        updateHealthUI ();
	}

	private void updateHealthUI(){
        slider.value = (currentHealth / startingHealth) * 100;
        fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, currentHealth / startingHealth);

        if (transform.gameObject.tag == "Player")
        {
            healthText.text = "Health: " + slider.value + "%";
        }
	}
	
	// On players death
	private void onDeath () {
		dead = true;
        foreach (Transform child in children)
        {
            child.gameObject.SetActive(false);
        }
        gameObject.SetActive(true);
        if (transform.gameObject.tag == "Player")
        {
            hcv.enabled = false;
        }
        rig.useGravity = false;
        StartCoroutine("Respawn", 2f);
        explosion.Play();
	}

    IEnumerator Respawn(float spawnDelay)
    {
        yield return new WaitForSeconds(spawnDelay);
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        foreach (Transform child in children)
        {
            child.gameObject.SetActive(true);
        }
        dead = false;
        if (transform.gameObject.tag == "Player")
        {
            hcv.enabled = true;
        }
        rig.useGravity = true;
        currentHealth = startingHealth;
        updateHealthUI ();
    }
}
