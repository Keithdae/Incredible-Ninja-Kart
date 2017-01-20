using UnityEngine;
using System.Collections;

public class HoverCarSystem : MonoBehaviour
{
  	Rigidbody body;
  	float deadZone = 0.1f;
	public float groundedDrag = 3f;
  	public float hoverForce = 1000;
	public float gravityForce = 1000f;
  	public float hoverHeight = 1.5f;
  	public GameObject[] hoverPoints;

	public ParticleSystem[] dustTrails = new ParticleSystem[2];

	// The first two wheels are assumed to be the front wheels
    public GameObject[] wheels;
    float wheelAngle;
	float wheelTurnSpeed = 1.5f;

 	int layerMask;

	void Start()
  	{
    	body = GetComponent<Rigidbody>();
		body.centerOfMass = Vector3.down;

	    layerMask = 1 << LayerMask.NameToLayer("Vehicle");
	    layerMask = ~layerMask;
	}

	// Uncomment this to see a visual indication of the raycast hit points in the editor window
//	void OnDrawGizmos()
//	{
//		RaycastHit hit;
//	    for (int i = 0; i < hoverPoints.Length; i++)
//	    {
//			var hoverPoint = hoverPoints [i];
//	      	if (Physics.Raycast(hoverPoint.transform.position, 
//	                            -Vector3.up, out hit,
//	                           	hoverHeight, 
//	                          	layerMask))
//	      	{
//	        	Gizmos.color = Color.blue;
//	        	Gizmos.DrawLine(hoverPoint.transform.position, hit.point);
//	        	Gizmos.DrawSphere(hit.point, 0.5f);
//	      	} 
//			else
//	     	{
//	        	Gizmos.color = Color.red;
//	        	Gizmos.DrawLine(hoverPoint.transform.position, 
//	                       		hoverPoint.transform.position - Vector3.up * hoverHeight);
//	      	}
//		}  
//	}
	
  	void Update()
  	{
		
  	}

  	void FixedUpdate()
  	{
		//  Do hover/bounce force
		RaycastHit hit;
		bool  grounded = false;
	    for (int i = 0; i < hoverPoints.Length; i++)
	    {
	   		var hoverPoint = hoverPoints [i];
            if (Physics.Raycast(hoverPoint.transform.position, -transform.up, out hit,hoverHeight, layerMask))
			{
				body.AddForceAtPosition(Vector3.up * hoverForce* (1.0f - (hit.distance / hoverHeight)), hoverPoint.transform.position);
				grounded = true;
			}
	   		else
	   		{
				// Self levelling - returns the vehicle to horizontal when not grounded and simulates gravity
		        if (transform.position.y > hoverPoint.transform.position.y)
				{
					// body.AddForceAtPosition(hoverPoint.transform.up * gravityForce, hoverPoint.transform.position);
				}
		        else
				{
					body.AddForceAtPosition(hoverPoint.transform.up * -gravityForce, hoverPoint.transform.position);
				}
	   		}
   		}

		// Particle handling
		var emissionRate = 0;
		if(grounded)
		{
			body.drag = groundedDrag;
			emissionRate = 10;
		}
		else
		{
			body.drag = 0.1f;
		}

		for(int i = 0; i<dustTrails.Length; i++)
		{
			var emission = dustTrails[i].emission;
			emission.rate = new ParticleSystem.MinMaxCurve(emissionRate);
		}

		// Spin the wheels
		spinWheels();
	}

    // TODO
	private void spinWheels()
	{
        foreach (GameObject wheel in wheels)
		{
            if (wheel != null)
            {
                Vector3 localVel = transform.InverseTransformDirection(body.velocity);
                if (Mathf.Abs(localVel.z) > 0.1)
                {
                    wheel.transform.Rotate(new Vector3(localVel.z, 0f, 0f));
                }
            }
		}
	}
}