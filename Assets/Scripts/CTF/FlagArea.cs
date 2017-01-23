using UnityEngine;
using System.Collections;

public class FlagArea : MonoBehaviour {

    CaptureTheFlag manager;

    // Use this for initialization
    void Start () {
        manager = GameObject.FindObjectOfType<CaptureTheFlag>();
    }

    // Update is called once per frame
    void Update () {

    }

    void OnTriggerEnter(Collider other){
        GameObject target = other.gameObject;
        if (target.layer == gameObject.layer-2)
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
                FlagTrigger flag = target.GetComponentInChildren<FlagTrigger>();
                while (flag == null)
                {
                    target = target.transform.parent.gameObject;
                    flag = target.GetComponentInChildren<FlagTrigger>();
                }
                flag.backToBase();
                manager.CaptureFlag(target.layer);
            }
        }
    }
}
