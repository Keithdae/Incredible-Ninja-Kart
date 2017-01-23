﻿using UnityEngine;
using System.Collections;

public class FlagTrigger : MonoBehaviour {

    private Vector3 startingPos;
    private int enemyLayer;
    Collider col;
    FlagHold holder;
    Transform startParent;

	// Use this for initialization
	void Start () {
        startingPos = transform.position;
        startParent = transform.parent;
        enemyLayer = opponentLayer();
        col = GetComponentInChildren<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other){
        GameObject target = other.gameObject;
        Debug.Log("Collided with : " + target.name);
        if (target.layer == enemyLayer)
        {
            // Pick up
            col.enabled = false;
            holder = target.GetComponentInChildren<FlagHold>();
            while (holder == null)
            {
                target = target.transform.parent.gameObject;
                holder = target.GetComponentInChildren<FlagHold>();
            }
            holder.grabFlag(this);
        }
        else
        {
            // Put back to base
            backToBase();
        }
    }

    public void enableCollider()
    {
        col.enabled = true;
    }

    public void backToBase()
    {
        enableCollider();
        transform.position = startingPos;
        transform.parent = startParent;
    }

    private int opponentLayer()
    {
        return (gameObject.layer == 10) ? 9 : 8;
    }
}
