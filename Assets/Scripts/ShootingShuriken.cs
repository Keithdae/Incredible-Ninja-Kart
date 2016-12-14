using UnityEngine;
using System.Collections;

public class ShootingShuriken : MonoBehaviour {

    public GameObject shuriken;
    public int Nombre_de_Shurikens;
    public int shootingForce;
    public Transform shurikenTransform;

    private string fireButton;
    private GameObject[] munitions;
    private int currentShuriken;

	// Use this for initialization
	void Start () {
        // The fire axis is based on the player number.
        fireButton = "Fire1";

        // initialisation de shurikens
        munitions = new GameObject[Nombre_de_Shurikens];
        for(int i = 0; i < Nombre_de_Shurikens; i++)
        {
            munitions[i] = (GameObject)Instantiate(shuriken, shurikenTransform.position, shurikenTransform.rotation);
            munitions[i].GetComponent<Transform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);
            munitions[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(fireButton))
        {
            Fire();
        }
    }

    private void Fire()
    {
        // on active le shuriken et on le tire
        if(munitions[currentShuriken].activeSelf == false)
        {
            munitions[currentShuriken].SetActive(true);
            munitions[currentShuriken].GetComponent<Transform>().position = shurikenTransform.position;
            munitions[currentShuriken].GetComponent<Rigidbody>().velocity = shootingForce * transform.forward;

            currentShuriken = (currentShuriken + 1) % Nombre_de_Shurikens;
        }
    }
}
