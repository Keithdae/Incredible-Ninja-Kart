using UnityEngine;
using System.Collections;

public class Shuriken : MonoBehaviour {
	public AudioSource audio;
	public float damage;
	public float maxLifeTime;

	// Use this for initialization
	void Start () {
	
	}

	private void onTriggerEnter(Collider other){
		GameObject target = other.gameObject;

		// on vérifie qu'on a touché un ennemi
		if (target.layer == opponentLayer (this.gameObject)) {
			KartHealth otherHealth = target.GetComponent<KartHealth>();
			// on a touché un ennemi
			otherHealth.TakeDamage(damage);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int opponentLayer(GameObject go){
		return (go.layer == 8) ? 9 : 8;
	}
}
