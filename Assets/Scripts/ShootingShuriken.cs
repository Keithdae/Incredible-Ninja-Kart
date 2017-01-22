using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShootingShuriken : MonoBehaviour {

    public GameObject shuriken;
    public int Nombre_de_Shurikens;
    public int shootingForce;
    public Transform shurikenSpawn;
    [HideInInspector]
    public Image aimImage;
    [HideInInspector]
    public Camera cam;
    public float shurikenRange;
    [HideInInspector]
    public Canvas HUDCanvas;
    public GameObject munition_Image;

    private string fireButton;
    private GameObject[] munitions;
    private GameObject[] munition_Images;
    private int currentShuriken;
    private Color shuriken_dispo = new Color(255f, 255f, 255f, 1f);
    private Color shuriken_indispo = new Color(255f, 255f, 255f, .25f);

	// Use this for initialization
	void Start () {
        // The fire axis is based on the player number.
        fireButton = "Fire1";

        // initialisation de shurikens
        munitions = new GameObject[Nombre_de_Shurikens];
        munition_Images = new GameObject[Nombre_de_Shurikens];
        for (int i = 0; i < Nombre_de_Shurikens; i++)
        {
            munitions[i] = (GameObject)Instantiate(shuriken, shurikenSpawn.position, shurikenSpawn.rotation);
            munitions[i].layer = transform.gameObject.layer;
            munitions[i].SetActive(false);
            //munitions[i].transform.parent = this.gameObject.transform;
            if (this.gameObject.tag == "Player")
            {
                munition_Images[i] = (GameObject)Instantiate(munition_Image, HUDCanvas.transform);
                munition_Images[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(munition_Images[i].GetComponent<RectTransform>().anchoredPosition.x + i * 23, munition_Images[i].GetComponent<RectTransform>().anchoredPosition.y);
                munition_Images[i].GetComponent<Image>().color = shuriken_dispo;
                munitions[i].GetComponent<Shuriken>().setMunitionImage(munition_Images[i].GetComponent<Image>());
                munitions[i].GetComponent<Shuriken>().player = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.tag == "Player")
        {
            aimImage.rectTransform.position = Input.mousePosition;
            Ray aimingRay = cam.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(aimingRay.origin, aimingRay.direction * 200, Color.yellow);
            if (Input.GetButtonDown(fireButton))
            {
                RaycastHit hit;
                if(Physics.Raycast(aimingRay, out hit, shurikenRange))
                {
                    Vector3 shootingDirection = (hit.point - shurikenSpawn.position).normalized;
                    Fire(shootingDirection);
                }
                else
                {
                    Vector3 point = aimingRay.origin + (aimingRay.direction.normalized * shurikenRange);
                    Vector3 shootingDirection = (point - shurikenSpawn.position).normalized;
                    Fire(shootingDirection);
                }
            }
        }
    }

    public void Fire(Vector3 shootDir)
    {
        // on active le shuriken et on le tire
        if(munitions[currentShuriken].activeSelf == false)
        {
            munitions[currentShuriken].GetComponent<Transform>().position = shurikenSpawn.position;
            //munitions[currentShuriken].GetComponent<Rigidbody>().velocity = shootingForce * shootDir;
            munitions[currentShuriken].GetComponent<Shuriken>().shootDir = shootDir;
            munitions[currentShuriken].SetActive(true);
            munitions[currentShuriken].GetComponent<Light>().enabled = true;
            munitions[currentShuriken].GetComponentInChildren<Renderer>().enabled = true;
            if (this.gameObject.tag == "Player")
            {
                munition_Images[currentShuriken].GetComponent<Image>().color = shuriken_indispo;
            }

            currentShuriken = (currentShuriken + 1) % Nombre_de_Shurikens;
        }
    }
}
