using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Shuriken : MonoBehaviour {
	public float damage;
	public float maxLifeTime;
    [HideInInspector]
    public bool player = false;

    private float currentLifeTime;
    private bool collided = false;
    private bool exploded = false;
    private Image munition_Image;
    private Color shuriken_dispo = new Color(255f, 255f, 255f, 1f);
    private Rigidbody rb;
    private Light lumiere;
    private Renderer mesh;

    // Use this for initialization
    void Start () {
        currentLifeTime = 0f;
        rb = this.GetComponent<Rigidbody>();
        lumiere = this.GetComponent<Light>();
        mesh = this.GetComponentInChildren<Renderer>();
    }

	void OnTriggerEnter(Collider other){
		GameObject target = other.gameObject;

        // on vérifie qu'on a touché un ennemi
        if (!collided && target.layer == opponentLayer (this.gameObject)) {
			KartHealth otherHealth = target.GetComponent<KartHealth>();
            while(otherHealth == null)
            {
                target = target.transform.parent.gameObject;
                otherHealth = target.GetComponent<KartHealth>();
            }
			// on a touché un ennemi
			otherHealth.TakeDamage(damage);
            mesh.enabled = false;
            lumiere.enabled = false;
            arreterShuriken();
            collided = true;
		}
        else if(!collided && target.layer != this.gameObject.layer)
        {
            lumiere.enabled = false;
            arreterShuriken();
            collided = true;
        }
	}

    void arreterShuriken()
    {
        Vector3 temp = rb.velocity;
        rb.velocity = new Vector3(0f, 0f, 0f);
        this.transform.position -= temp.normalized * 3;
    }
	
	// Update is called once per frame
	void Update () {
	    if(this.gameObject.activeSelf && currentLifeTime < maxLifeTime)
        {
            currentLifeTime += Time.deltaTime;
        }
        else if (this.gameObject.activeSelf && currentLifeTime >= maxLifeTime)
        {
            this.gameObject.SetActive(false);
            if (player)
            {
                this.munition_Image.color = shuriken_dispo;
            }
            collided = false;
            currentLifeTime = 0f;
        }
	}

	public int opponentLayer(GameObject go)
    {
        return (go.layer == 8) ? 9 : 8;
	}

    public void setMunitionImage(Image mI)
    {
        this.munition_Image = mI;
    }
}
