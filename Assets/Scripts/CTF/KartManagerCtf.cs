using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class KartManagerCtf
{
    public Transform[] spawnPoints;
    public List<Transform> wpAlly;
    public List<Transform> wpEnemy;
    public Transform flagAreaAlly;
    public Transform flagAraEnemy;
    public GameObject flagAlly;
    public GameObject flagEnemy;
    [HideInInspector]
    public GameObject instance;
    [HideInInspector]
    public Canvas playerCanvas;

    private Panda.PandaBehaviour pandaBehaviourIA;
    private MoveHandler movementIA;
    private HoverCarControl playerDrivingBehaviour;
    private ShootingShuriken playerShootingBehaviour;
    public KartHealth healthComponent;

    public void Setup()
    {
        // Get references to the components.
        if (instance.tag == "IA")
        {
            pandaBehaviourIA = instance.GetComponent<Panda.PandaBehaviour>();
            healthComponent = instance.GetComponent<KartHealthIaCtf>();
            movementIA = instance.GetComponent<MoveHandler>();
        }
        else if (instance.tag == "Player")
        {
            playerDrivingBehaviour = instance.GetComponent<HoverCarControl>();
            playerShootingBehaviour = instance.GetComponent<ShootingShuriken>();
            healthComponent = instance.GetComponent<KartHealthPlayerCtf>();
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
        ((KartHealthPlayerCtf)healthComponent).damageImage = images[0];
        ((KartHealthPlayerCtf)healthComponent).healthText = hud.GetComponentInChildren<Text>(); ;
    }

    public void initIAComponents(Camera cam)
    {
        healthComponent.cam = cam.gameObject;
        healthComponent.spawnPoints = spawnPoints;
        movementIA.wpAllies = wpAlly;
        movementIA.wpEnemies = wpEnemy;
        movementIA.flagAreaAlly = flagAreaAlly;
        movementIA.flagAreaEnemy = flagAraEnemy;
        movementIA.flagAlly = flagAlly;
        movementIA.flagEnemy = flagEnemy;
    }
}