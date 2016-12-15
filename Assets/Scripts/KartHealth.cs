using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KartHealth : MonoBehaviour {

	public float startingHealth = 100f;
	[HideInInspector]
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

    void Start(){
		currentHealth = startingHealth;
		dead = false;
		updateHealthUI ();
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
            Debug.Log(damageImage);
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
    }

	public void TakeDamage(float damage){
		currentHealth -= damage;

        damageImage.color = flashColour; // flash
        updateHealthUI ();

        if (currentHealth <= 0 && !dead) {
			onDeath ();
		}
	}

	private void updateHealthUI(){
		slider.value = (currentHealth / startingHealth) * 100;
		fillImage.color = Color.Lerp (zeroHealthColor, fullHealthColor, currentHealth / startingHealth);

        healthText.text = "Health: " + slider.value + "%";
	}
	
	// On players death
	private void onDeath () {
		dead = true;
	}
}
