using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Panda;

public class CoordinationCtf : MonoBehaviour {

    public float communicationRadius = 100.0f;

    private MoveHandlerCtf mvHandler;

    private List<GameObject> alliesInRange;

    GameObject allyInNeed;

    // Use this for initialization
    void Start () {
        allyInNeed = null;
        alliesInRange = new List<GameObject>();
        mvHandler = GetComponent<MoveHandlerCtf>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    // Tasks for PandaBT ---------------------------
    [Task]
    void CallForHelp()
    {
        List<GameObject> mates = CheckAlliesInRange();
        foreach (GameObject mate in mates)
        {
            if (mate.CompareTag("Player"))
            {
                // TODO
            }
            else
            {
                CoordinationCtf coord = mate.GetComponent<CoordinationCtf>();
                coord.askHelp(gameObject);
            }
        }
        Task.current.Succeed();
    }


    [Task]
    void hasAllyInNeed()
    {
        Task.current.Complete(allyInNeed != null);
    }

    [Task]
    void GoToHelp()
    {
        mvHandler.moveTo(allyInNeed.transform.position);
        allyInNeed = null;
        Task.current.Succeed();
    }


    // Utility functions ---------------------------
    public List<GameObject> CheckAlliesInRange()
    {
        List<GameObject> allies = mvHandler.GetAllies();
        alliesInRange.Clear();
        foreach(GameObject ally in allies)
        {
            Vector3 dir = ally.transform.position - transform.position;
            RaycastHit hit;
            // Check only for objects on the same layer, aka allies
            int layerMask = (1 << gameObject.layer);
            if (Physics.Raycast(transform.position, dir, out hit, communicationRadius, layerMask))
            {
                GameObject mate = hit.transform.gameObject;
                alliesInRange.Add(mate);
            }
        }

        return alliesInRange;
    }

    void askHelp(GameObject asker)
    {
        if(allyInNeed == null)
            allyInNeed = asker;
    }



}
