using UnityEngine;
using System.Collections;
using System;

public class Deathmatch : MonoBehaviour {
    public float startDelay;
    public GameObject kartPlayerPrefab;
    public GameObject kartIAPrefab;
    public Transform[] spawnPointsTeam1;
    public Transform[] spawnPointsTeam2;
    public Canvas playerCanvas;
    public float spawnDelay;
    public GameObject mainCamera;

    public KartManager[] team1;
    public KartManager[] team2;

	// Use this for initialization
	void Start () {
        spawnKarts();
        mainCamera.GetComponent<CameraController>().target = team1[0].instance.transform;
        Debug.Log("WE STARTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
	}

    // Update is called once per frame
    void Update () {
	
	}

    private void spawnKarts()
    {
        Debug.Log(kartPlayerPrefab.GetType());
        Vector3 pos1 = spawnPointsTeam1[0].position;
        Vector3 pos2 = spawnPointsTeam2[0].position;
        Vector3 position = pos1;
        team1[0].instance = (GameObject)Instantiate(kartPlayerPrefab, position, spawnPointsTeam1[0].rotation);
        team1[0].playerCanvas = playerCanvas;
        for (int i = 0; i < team1.Length; i++)
        {
            Debug.Log(i);
            position = new Vector3(pos1.x + (i % 2) * 8, pos1.y, pos1.z + (i / 2) * 8);
            if (i != 0)
            {
                team1[i].instance = Instantiate(kartIAPrefab, position, spawnPointsTeam1[0].rotation) as GameObject;
            }
            team1[i].spawnPoints = spawnPointsTeam1;
            team1[i].Setup();
            team1[i].setSpawnDelay(spawnDelay);

            position = new Vector3(pos2.x + (i % 2) * 8, pos2.y, pos2.z + (i / 2) * 8);
            team2[i].instance = Instantiate(kartIAPrefab, position, spawnPointsTeam2[0].rotation) as GameObject;
            team2[i].spawnPoints = spawnPointsTeam2;
            team2[i].Setup();
            team2[i].setSpawnDelay(spawnDelay);
            if (i != 0)
            {
                team1[i].initIAComponents(mainCamera.GetComponent<Camera>());
            }
            team2[i].initIAComponents(mainCamera.GetComponent<Camera>());
        }
        team1[0].initPlayerComponents(mainCamera.GetComponent<Camera>(), playerCanvas);
    }

    private void EnableAllControls(bool val)
    {
        for (int i = 0; i < team1.Length; i++)
        {
            team1[i].EnableControl(val);
            team2[i].EnableControl(val);
        }
    }
}
