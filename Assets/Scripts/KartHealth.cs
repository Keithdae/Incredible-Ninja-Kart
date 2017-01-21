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
    public GameObject cam;
    public float spawnDelay = 2f; //delay to respawn (in sec)

    public Transform[] spawnPoints;
    private HoverCarControl hcv;
    private Rigidbody rig;
    private Renderer[] children;
    private Canvas childCanvas;

    void Start(){
		currentHealth = startingHealth;
		dead = false;
		updateHealthUI ();
        hcv = GetComponent<HoverCarControl>();
        rig = GetComponent<Rigidbody>();
        children = GetComponentsInChildren<Renderer>();
        if (gameObject.tag.Equals("IA"))
        {
            childCanvas = GetComponentInChildren<Canvas>();
        }
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
        if (gameObject.tag == "IA")
        {
            childCanvas.transform.LookAt(cam.transform);
        }
        if (currentHealth <= 0 && !dead) 
        {
            onDeath ();
        }
        if(transform.position.y < -25f && !dead)
        {
            onDeath();
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
        arreterKart();
        
        cam.gameObject.GetComponent<CameraController>().StartCoroutine("OnPlayersDeath");
        StartCoroutine("Respawn");
	}

    public void arreterKart()
    {
        rig.velocity = new Vector3(0f, 0f, 0f);
    } 

    public float getHealth()
    {
        return slider.value;
    }

    IEnumerator Respawn()
    {
        //yield return new WaitForSeconds(explosionDuration);
        foreach (Renderer child in children)
        {
            if (!child.name.Equals("explosion"))
            {
                child.enabled = false;
            }
        }
        if (gameObject.tag.Equals("IA"))
        {
            childCanvas.enabled = false;
        }        
        if (transform.gameObject.tag == "Player")
        {
            hcv.enabled = false;
        }
        rig.useGravity = false;
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        yield return new WaitForSeconds(spawnDelay);
        foreach (Renderer child in children)
        {
            child.enabled = true;
        }
        if (gameObject.tag.Equals("IA"))
        {
            childCanvas.enabled = true;
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
