using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShootingShuriken : MonoBehaviour {

    public GameObject shuriken;
    public int Nombre_de_Shurikens;
    public int shootingForce;
    public Transform shurikenSpawn;
    public Image aimImage;
    public Camera camera;
    public float shurikenRange;

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
            munitions[i] = (GameObject)Instantiate(shuriken, shurikenSpawn.position, shurikenSpawn.rotation);
            munitions[i].GetComponent<Transform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);
            munitions[i].layer = transform.gameObject.layer;
            munitions[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.tag == "Player")
        {
            aimImage.rectTransform.position = Input.mousePosition;
            Ray aimingRay = camera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(aimingRay.origin, aimingRay.direction * 200, Color.yellow);
            if (Input.GetButtonDown(fireButton))
            {
                RaycastHit hit;
                if(Physics.Raycast(aimingRay, out hit, shurikenRange))
                {
                    Vector3 shootingDirection = hit.point - shurikenSpawn.position;
                    Fire(shootingDirection);
                }
            }
        }
    }

    private void Fire(Vector3 shootDir)
    {
        // on active le shuriken et on le tire
        if(munitions[currentShuriken].activeSelf == false)
        {
            munitions[currentShuriken].GetComponent<Transform>().position = shurikenSpawn.position;
            munitions[currentShuriken].GetComponent<Rigidbody>().velocity = shootingForce * shootDir;
            munitions[currentShuriken].SetActive(true);

            currentShuriken = (currentShuriken + 1) % Nombre_de_Shurikens;
        }
    }
}
