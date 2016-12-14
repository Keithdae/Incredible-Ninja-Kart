using UnityEngine;
using System.Collections;

public class Shuriken : MonoBehaviour {
	public AudioSource audio;
	public float damage;
	public float maxLifeTime;

    private float currentLifeTime;

	// Use this for initialization
	void Start () {
        currentLifeTime = 0f;
	}

	void OnTriggerEnter(Collider other){
		GameObject target = other.gameObject;

        // on vérifie qu'on a touché un ennemi
        if (target.layer == opponentLayer (this.gameObject)) {
			KartHealth otherHealth = target.GetComponent<KartHealth>();
            while(otherHealth == null)
            {
                target = target.transform.parent.gameObject;
                otherHealth = target.GetComponent<KartHealth>();
            }
			// on a touché un ennemi
			otherHealth.TakeDamage(damage);
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
            currentLifeTime = 0f;
        }
	}

	public int opponentLayer(GameObject go)
    {
        return (go.layer == 8) ? 9 : 8;
	}
}
