using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KartHealth : MonoBehaviour {

	public float startingHealth = 100f;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public int nbOfDeaths = 0;
    [HideInInspector]
    public bool dead;
    [HideInInspector]
    public Slider slider; // health slider 
    [HideInInspector]
    public Image fillImage; // fill image of health slider
	public Color zeroHealthColor; 
	public Color fullHealthColor;
    [HideInInspector]
    public GameObject cam;
    public float spawnDelay = 2f; //delay to respawn (in sec)

    [HideInInspector]
    public Transform[] spawnPoints;
    [HideInInspector]
    public HoverCarControl hcv;
    [HideInInspector]
    public Rigidbody rig;
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
        if(currentHealth < 0)
        {
            nbOfDeaths++;
        }
        updateHealthUI();
    }

	public virtual void updateHealthUI(){
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
        rig.angularVelocity = new Vector3(0f, 0f, 0f);
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

    public int opponentLayer(GameObject go)
    {
        return (go.layer == 8) ? 9 : 8;
    }

    // cette methode renvoie le point ou il faut respawn, prioritairment le point de respawn le plus proche, 
    // mais si des ennemis y sont presents on choisi le point le plus eloigné d'un ennemi 
    public Transform getSpawnPoint()
    {
        int res = 0;
        int secondChoice = -1;
        float maxDist = -1f;
        bool trouve = false;
        int i = 0;
        sortClosestSpawn(); // on tri le tableau des points de spawn
        while (!trouve && i < spawnPoints.Length)
        {
            Collider[] colliders = Physics.OverlapSphere(spawnPoints[i].position, 50, 1<<opponentLayer(gameObject));
            if (colliders == null || colliders.Length == 0)
            {
                trouve = true;
                res = i;
            }
            else
            {
                float minDist = 100000;
                foreach(Collider c in colliders)
                {
                    Debug.Log(c.name);
                    if ((spawnPoints[i].position - c.transform.position).magnitude < minDist)
                    {
                        minDist = (spawnPoints[i].position - c.transform.position).magnitude;
                    }
                }
                if (minDist > maxDist)
                {
                    maxDist = minDist;
                    secondChoice = i;
                }
            }
            i++;
        }
        return trouve?spawnPoints[res]:spawnPoints[secondChoice];
    }

    // cette méthode tri le tableau des spawn points
    public void sortClosestSpawn()
    {
        for(int i = 0; i < spawnPoints.Length - 1; i++)
        {
            int argmin = i;
            float min = (spawnPoints[i].position - transform.position).magnitude;
            for (int j = i+1; j < spawnPoints.Length; j++)
            {
                if((spawnPoints[j].position - transform.position).magnitude < min)
                {
                    min = (spawnPoints[j].position - transform.position).magnitude;
                    argmin = j;
                }
            }
            if(argmin != i)
            {
                Transform temp = spawnPoints[i];
                spawnPoints[i] = spawnPoints[argmin];
                spawnPoints[argmin] = temp;
            }
        }
    }

    public void goToSpawnPoint()
    {
        // on determine a quel point on respawn, et on evite de respawn sur un coequipier
        Transform spawn = getSpawnPoint();
        Collider[] colliders = Physics.OverlapSphere(spawn.position, 5, 1<<gameObject.layer);
        if (colliders.Length > 0)
        {
            transform.position = new Vector3(spawn.position.x + 8, spawn.position.y, spawn.position.z);
        }
        else
        {
            transform.position = spawn.position;
        }
        transform.rotation = spawn.rotation;
    }
}
