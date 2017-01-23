using UnityEngine;
using System.Collections;

public class FlagArea : MonoBehaviour {

    CaptureTheFlag manager;

    public int layerTeam = 2;

    // Use this for initialization
    void Start () {
        manager = GameObject.FindObjectOfType<CaptureTheFlag>();
    }

    // Update is called once per frame
    void Update () {

    }

    void OnTriggerEnter(Collider other){
        GameObject target = other.gameObject;
        if (target.layer == layerTeam)
        {
            // Score a point, bring flag back home
            FlagHold hold = target.GetComponentInChildren<FlagHold>();
            while (hold == null)
            {
                target = target.transform.parent.gameObject;
                hold = target.GetComponentInChildren<FlagHold>();
            }
            if (hold.hasFlag)
            {
                FlagTrigger flag = hold.flagHeld;
                flag.backToBase();
                hold.hasFlag = false;
                hold.flagHeld = null;
                manager.CaptureFlag(target.layer);
            }
        }
    }
}
