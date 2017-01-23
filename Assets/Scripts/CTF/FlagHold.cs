using UnityEngine;
using System.Collections;

public class FlagHold : MonoBehaviour {

    public FlagTrigger flagHeld;
    Transform prevParent;
    public bool hasFlag = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void grabFlag(FlagTrigger flag)
    {
        prevParent = flag.transform.parent;
        flag.transform.parent = gameObject.transform;
        flagHeld = flag;
        hasFlag = true;
    }

    public void dropFlag()
    {
        if (flagHeld != null)
        {
            hasFlag = false;
            flagHeld.transform.parent = prevParent;
            flagHeld.enableCollider();
            flagHeld = null;
        }
    }

    public void dropFlagToBase()
    {
        if (flagHeld != null)
        {
            hasFlag = false;
            flagHeld.backToBase();
            flagHeld = null;
        }
    }
}
