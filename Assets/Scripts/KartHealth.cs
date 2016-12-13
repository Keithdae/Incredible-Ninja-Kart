using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KartHealth : MonoBehaviour {

	public float startingHealth = 100f;
	[HideInInspector]
	public float currentHealth;
	[HideInInspector]
	public bool dead;

	public Slider slider;
	public Image fillImage;
	public Color zeroHealthColor;
	public Color fullHealthColor;

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

	public void TakeDamage(float damage){
		currentHealth -= damage;

		updateHealthUI ();

		if (currentHealth <= 0 && !dead) {
			onDeath ();
		}
	}

	private void updateHealthUI(){
		slider.value = (currentHealth / startingHealth) * 100;

		fillImage.color = Color.Lerp (zeroHealthColor, fullHealthColor, currentHealth / startingHealth);
	}
	
	// On players death
	private void onDeath () {
		dead = true;
	}
}
