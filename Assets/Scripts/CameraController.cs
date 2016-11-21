using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public GameObject target; // l’objet à suivre
	private Vector3 offset; // L’offset initial

	void Start ()
	{
		offset = transform.position - target.transform.position;
		offset.x = 0;
		offset.z = 0;
	}

	void LateUpdate()
	{
		transform.position = target.transform.position + offset + 10 * (new Vector3(-target.transform.forward.x, 0, -target.transform.forward.z));
		transform.LookAt (target.transform);
	}
}