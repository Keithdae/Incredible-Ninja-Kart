using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShootingParchemin : MonoBehaviour {

    public float parcheminRange;
    public float coolDown;
    public Camera cam;
    public GameObject parchemin;
    public GameObject bomb;
    private string explosionButton;
    private float timeleft;
    private Slider bombSlider;
    // Use this for initialization
    void Start () {
        explosionButton = "Fire2";
        bombSlider = bomb.GetComponent<Slider>();
        timeleft = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (this.gameObject.tag == "Player")
        {
            timeleft -= Time.deltaTime;
            timeleft = timeleft < 0 ? 0 : timeleft;
            Ray aimingRay = cam.ScreenPointToRay(Input.mousePosition);

            if (Input.GetButtonDown(explosionButton) && timeleft <= 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(aimingRay, out hit, 200))
                {
                    float distance = (hit.point - transform.position).magnitude;
                    if (Shuriken.opponentLayer(gameObject) == hit.collider.gameObject.layer && distance < parcheminRange)
                    {
                        Fire(hit.collider.gameObject);
                    }
                }
            }
            float val = 1 - (timeleft / coolDown);
            bombSlider.value = val;
        }
    }

    public void Fire(GameObject target)
    {
        GameObject p = Instantiate(parchemin, target.transform) as GameObject;
        p.transform.position = target.transform.position;
        p.transform.Translate(0.116f, 0.11f, 0.303f);
        timeleft = coolDown;
    }
}
