using UnityEngine;
using System.Collections;

public class Shuriken : MonoBehaviour {
	public AudioSource audio;
	public float damage;
	public float maxLifeTime;

    private float currentLifeTime;
    private bool collided = false;

	// Use this for initialization
	void Start () {
        currentLifeTime = 0f;
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
            this.gameObject.SetActive(false);
		}
        else if(!collided && target.layer != this.gameObject.layer)
        {
            Rigidbody rb = this.GetComponent<Rigidbody>();
            Vector3 temp = rb.velocity;
            rb.velocity = new Vector3(0f,0f,0f);
            this.transform.position -= temp.normalized * 3;
            collided = true;
        }
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
            collided = false;
            currentLifeTime = 0f;
        }
	}

	public int opponentLayer(GameObject go)
    {
        return (go.layer == 8) ? 9 : 8;
	}
}
