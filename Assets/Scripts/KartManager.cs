using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class KartManager
{
    public Transform[] spawnPoints;
    [HideInInspector]
    public GameObject instance;
    [HideInInspector]
    public Canvas playerCanvas;

    private Panda.PandaBehaviour pandaBehaviourIA;
    private HoverCarControl playerDrivingBehaviour;
    private ShootingShuriken playerShootingBehaviour;

    public void Setup()
    {
        // Get references to the components.
        if (instance.tag == "IA")
        {
            pandaBehaviourIA = instance.GetComponent<Panda.PandaBehaviour>();
        }
        else if (instance.tag == "player")
        {
            playerDrivingBehaviour = instance.GetComponent<HoverCarControl>();
            playerShootingBehaviour = instance.GetComponent<ShootingShuriken>();
        }
    }

    public void EnableControl(bool val)
    {
        if (instance.tag == "IA")
        {
            pandaBehaviourIA.enabled = val;
        }
        else if (instance.tag == "player")
        {
            playerDrivingBehaviour.enabled = val;
            playerShootingBehaviour.enabled = val;
            playerCanvas.enabled = val;
        }
    }
}