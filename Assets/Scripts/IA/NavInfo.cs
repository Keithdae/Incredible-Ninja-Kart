using UnityEngine;
using System.Collections;

public class NavInfo : MonoBehaviour {

    public Transform dest;
    public NavMeshAgent navAgent;

	// Use this for initialization
	void Start () {
        navAgent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
