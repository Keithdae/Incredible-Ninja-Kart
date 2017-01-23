using UnityEngine;
using System.Collections;

public class Parchemin : MonoBehaviour
{
    public float maxLifeTime; // duree après laquelle le parchemin disparait
    public float explosionDelay; // nb de secondes avant explosion
    public GameObject explosion;

    // Use this for initialization
    void Start () {
        // If it isn't destroyed by then, destroy the shell after it's lifetime.
        explosion.layer = Shuriken.opponentLayer(gameObject.transform.parent.gameObject);
        Destroy(gameObject, maxLifeTime);
        StartCoroutine(explode());
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator explode()
    {
        yield return new WaitForSeconds(explosionDelay);
        explosion.SetActive(true);
    }
}
