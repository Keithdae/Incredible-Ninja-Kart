﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class Deathmatch : MonoBehaviour
{
    public float startDelay;
    public GameObject kartPlayerPrefab;
    public GameObject kartAllyPrefab;
    public GameObject kartEnemyPrefab;
    public Transform[] spawnPointsTeam1;
    public Transform[] spawnPointsTeam2;
    public List<Transform> wpBaseTeam1;
    public List<Transform> wpBaseTeam2;
    public Canvas playerCanvas;
    public float spawnDelay;
    public int dureePartie;
    public int nbKillsToWin;
    public GameObject mainCamera;
    public GameObject startDisplay;
    public GameObject scoreDisplay;
    public GameObject endDisplay;
    public GameObject bombSlider;

    public KartManager[] team1;
    public KartManager[] team2;

    private Text scoreText;
    private Text timeText;
    private float timeleft;
    private Text endText;


    // Use this for initialization
    void Start()
    {
        startDisplay.GetComponent<StartCountdown>().delai = startDelay;
        startDisplay.SetActive(false);
        Text[] aux = scoreDisplay.GetComponentsInChildren<Text>();
        timeText = aux[0];
        scoreText = aux[1];
        scoreDisplay.SetActive(false);
        endText = endDisplay.GetComponentInChildren<Text>();
        endDisplay.SetActive(false);
        timeleft = 60f * dureePartie;
        spawnKarts();
        mainCamera.GetComponent<CameraController>().target = team1[0].instance.transform;
        StartCoroutine("gameLoop");
    }
    

    private void spawnKarts()
    {
        Vector3 pos1 = spawnPointsTeam1[0].position;
        Vector3 pos2 = spawnPointsTeam2[0].position;
        Vector3 position = pos1;
        team1[0].instance = (GameObject)Instantiate(kartPlayerPrefab, position, spawnPointsTeam1[0].rotation);
        team1[0].instance.GetComponent<ShootingParchemin>().cam = mainCamera.GetComponent<Camera>();
        team1[0].instance.GetComponent<ShootingParchemin>().bomb = bombSlider;
        team1[0].playerCanvas = playerCanvas;
        for (int i = 0; i < team1.Length; i++)
        {
            position = new Vector3(pos1.x + (i % 2) * 8, pos1.y, pos1.z + (i / 2) * 8);
            if (i != 0)
            {
                team1[i].instance = Instantiate(kartAllyPrefab, position, spawnPointsTeam1[0].rotation) as GameObject;
                //SetLayer(team1[i].instance, 8);
            }
            team1[i].spawnPoints = spawnPointsTeam1;
            team1[i].wpAlly = wpBaseTeam1;
            team1[i].wpEnemy = wpBaseTeam2;
            team1[i].Setup();
            team1[i].setSpawnDelay(spawnDelay);

            position = new Vector3(pos2.x + (i % 2) * 8, pos2.y, pos2.z + (i / 2) * 8);
            team2[i].instance = Instantiate(kartEnemyPrefab, position, spawnPointsTeam2[0].rotation) as GameObject;
            team2[i].spawnPoints = spawnPointsTeam2;
            team2[i].wpAlly = wpBaseTeam2;
            team2[i].wpEnemy = wpBaseTeam1;
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

    public void SetLayer(GameObject go, int layerNumber)
    {
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }

    private IEnumerator start()
    {
        startDisplay.SetActive(true);
        mainCamera.GetComponent<CameraController>().enableBlur(true);
        bombSlider.SetActive(false);
        EnableAllControls(false);
        yield return new WaitForSeconds(startDelay);
        mainCamera.GetComponent<CameraController>().enableBlur(false);
        bombSlider.SetActive(true);
        startDisplay.SetActive(false);
    }

    private IEnumerator deathmatch()
    {
        scoreDisplay.SetActive(true);
        EnableAllControls(true);
        bool finished = false;
        while (!finished)
        {
            timeleft -= Time.deltaTime;
            int score1 = 0;
            int score2 = 0;
            for (int i = 0; i < team1.Length; i++){
                score1 += team2[i].healthComponent.nbOfDeaths;
                score2 += team1[i].healthComponent.nbOfDeaths;
            }
            scoreText.text = (score1 * 10).ToString() + " : " + (score2 * 10).ToString();
            int minutesleft = ((int)timeleft) / 60;
            int secondsleft = ((int)timeleft) % 60;
            timeText.text = minutesleft.ToString() + " : " + ((secondsleft<10)?"0"+secondsleft.ToString():secondsleft.ToString());
            if (score1 >= nbKillsToWin || score2 >= nbKillsToWin || timeleft <= 0)
            {
                finished = true;
            }
            yield return null;
        }
        scoreDisplay.SetActive(false);
    }

    private IEnumerator end()
    {
        EnableAllControls(false);
        endDisplay.SetActive(true);
        mainCamera.GetComponent<CameraController>().enableBlur(true);
        int score1 = 0;
        int score2 = 0;
        for (int i = 0; i < team1.Length; i++)
        {
            score1 += team2[i].healthComponent.nbOfDeaths;
            score2 += team1[i].healthComponent.nbOfDeaths;
        }
        String aux = score1 > score2 ? "You win!\n" : (score2 > score1 ? "You lose!\n" : "It's a draw!\n");
        aux += (score1*10).ToString() + " : " + (score2*10).ToString();
        endText.text = aux;
        endText.color = score1 > score2 ? Color.green : (score2 > score1 ? Color.red : Color.yellow);
        yield return null;
    }


    private IEnumerator gameLoop()
    {
        yield return StartCoroutine(start());
        yield return StartCoroutine(deathmatch());
        yield return StartCoroutine(end());
    }

    public void backToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
