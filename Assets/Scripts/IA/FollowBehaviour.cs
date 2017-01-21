using UnityEngine;
using System.Collections;

public class FollowBehaviour : MonoBehaviour {
    
    private NavInfo navInfo;

	// Use this for initialization
	void Start () {
        navInfo = GetComponent<NavInfo>();
        navInfo.navAgent.SetDestination(navInfo.dest.position);
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(navInfo.navAgent.destination);    
        if (navInfo.navAgent.destination != null)
        {
            navInfo.navAgent.SetDestination(navInfo.dest.position);
        }
    }
}
