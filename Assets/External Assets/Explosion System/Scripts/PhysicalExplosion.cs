using UnityEngine;
using System.Collections;
 
public class PhysicalExplosion : MonoBehaviour 
{
    public float Radius;// explosion radius
    public float Force;// explosion forse
    public float maxDamage;// explosion damage

    private bool exploded = false;
    void Update () 
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, Radius);// create explosion
        for(int i=0; i<hitColliders.Length; i++)
        {              
            if(hitColliders[i].gameObject.layer == getOpponentLayer(gameObject.transform.parent.gameObject) && !exploded)
            {
                Rigidbody rb = hitColliders[i].gameObject.GetComponentsInParent<Rigidbody>()[0];
                rb.AddExplosionForce(Force, transform.position, Radius, 0.0F); // push game object
                rb.gameObject.GetComponent<KartHealth>().TakeDamage(CalculateDamage(hitColliders[i].transform.position));
            }
			
        }
        exploded = true;
        Destroy(gameObject,0.2f);// destroy explosion
    }

    public int getOpponentLayer(GameObject o)
    {
        return o.layer == 8 ? 9 : 8;
    }

    private float CalculateDamage(Vector3 targetPosition)
    {
        // Create a vector from the shell to the target.
        Vector3 explosionToTarget = targetPosition - transform.position;

        // Calculate the distance from the shell to the target.
        float explosionDistance = explosionToTarget.magnitude;

        // Calculate the proportion of the maximum distance (the explosionRadius) the target is away.
        float relativeDistance = (Radius - explosionDistance) / Radius;

        // Calculate damage as this proportion of the maximum possible damage.
        float damage = relativeDistance * maxDamage;

        // Make sure that the minimum damage is always 0.
        damage = Mathf.Max(0f, damage);
        return damage;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,Radius);
    }
}