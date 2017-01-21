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
	public Color zeroHealthColor; 
	public Color fullHealthColor;
    public GameObject cam;
    public float spawnDelay = 2f; //delay to respawn (in sec)

    public Transform[] spawnPoints;
    protected HoverCarControl hcv;
    protected Rigidbody rig;
    protected Renderer[] children;

    protected virtual void Start(){
		currentHealth = startingHealth;
		dead = false;
        hcv = GetComponent<HoverCarControl>();
        rig = GetComponent<Rigidbody>();
        children = GetComponentsInChildren<Renderer>();
	}

	// Use this for initialization
	protected void OnEnable () {
		currentHealth = startingHealth;
		dead = false;
	}

	public virtual void TakeDamage(float damage){
		currentHealth -= damage;
        updateHealthUI();
    }

	protected virtual void updateHealthUI(){
        slider.value = (currentHealth / startingHealth) * 100;
        fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, currentHealth / startingHealth);
	}
	
	// On players death
	protected  virtual void onDeath () {
		dead = true;
        arreterKart();
	}

    public void arreterKart()
    {
        rig.velocity = new Vector3(0f, 0f, 0f);
    } 

    public float getHealth()
    {
        return slider.value;
    }

    public void setKartVisible(bool val)
    {
        foreach (Renderer child in children)
        {
            child.enabled = val;
        }
    }
}
