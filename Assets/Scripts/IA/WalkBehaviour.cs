using UnityEngine;
using System.Collections;

public class WalkBehaviour : MonoBehaviour {

    public Transform dest;

    private NavMeshAgent navAgent;

	// Use this for initialization
	void Start () {
        navAgent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        navAgent.SetDestination(dest.position);
	}
}
