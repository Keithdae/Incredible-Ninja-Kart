using UnityEngine;
using UnityEngine.UI;
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
    public KartHealth healthComponent;

    public void Setup()
    {
        // Get references to the components.
        if (instance.tag == "IA")
        {
            pandaBehaviourIA = instance.GetComponent<Panda.PandaBehaviour>();
            healthComponent = instance.GetComponent<KartHealthIA>();
        }
        else if (instance.tag == "Player")
        {
            playerDrivingBehaviour = instance.GetComponent<HoverCarControl>();
            playerShootingBehaviour = instance.GetComponent<ShootingShuriken>();
            healthComponent = instance.GetComponent<KartHealthPlayer>();
        }
    }

    public void EnableControl(bool val)
    {
        if (instance.tag == "IA")
        {
            pandaBehaviourIA.enabled = val;
            instance.GetComponent<NavMeshAgent>().enabled = val;
        }
        else if (instance.tag == "Player")
        {
            playerDrivingBehaviour.enabled = val;
            playerShootingBehaviour.enabled = val;
            //playerCanvas.enabled = val;
        }
    }

    public void setSpawnDelay(float delay)
    {
        healthComponent.spawnDelay = delay;
    }

    public void initPlayerComponents(Camera cam, Canvas hud)
    {
        Image[] images = hud.GetComponentsInChildren<Image>(); // on recupere toutes les images de l'HUD (dans l'ordre)
        playerShootingBehaviour.cam = cam;
        playerShootingBehaviour.aimImage = images[3];
        playerShootingBehaviour.HUDCanvas = hud;

        healthComponent.slider = hud.GetComponentInChildren<Slider>();
        healthComponent.fillImage = images[2];
        healthComponent.cam = cam.gameObject;
        healthComponent.spawnPoints = spawnPoints;
        ((KartHealthPlayer)healthComponent).damageImage = images[0];
        ((KartHealthPlayer)healthComponent).healthText = hud.GetComponentInChildren<Text>(); ;
    }

    public void initIAComponents(Camera cam)
    {
        healthComponent.cam = cam.gameObject;
        healthComponent.spawnPoints = spawnPoints;
    }
}