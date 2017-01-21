using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class CameraController : MonoBehaviour {
	[SerializeField] Transform target;
    public Image wastedImage;
    public AudioSource wastedSound;
    public Transform deathCamPosition;
    public float spawnDelay;

	private float maxDistance = 15f;
    private Blur blurComponent;
    private Grayscale grayscaleComponent;

    void Start()
    {
        blurComponent = gameObject.GetComponent<Blur>();
        grayscaleComponent = gameObject.GetComponent<Grayscale>();
    }

	void LateUpdate()
	{
		transform.position = target.position;
		Quaternion targetRotation = Quaternion.Euler(0,target.rotation.eulerAngles.y,0);
		transform.rotation = targetRotation;
		transform.Translate(new Vector3(0,6,-maxDistance));

		//RaycastHit hit;
		var camVector = transform.position-target.position;
		Ray ray = new Ray(target.position,camVector);
	    /*if (Physics.Raycast(ray,out hit,maxDistance+0.5f))
		{
			transform.position = hit.point + hit.normal;
		}*/

		var rot = transform.rotation.eulerAngles;
		rot.x = Vector3.Angle(target.position - transform.position, transform.forward);
		transform.rotation = Quaternion.Euler(rot);
		transform.Translate(Vector3.forward*0.5f);
	}

    IEnumerator OnPlayersDeath()
    {
        transform.position = deathCamPosition.position;
        transform.rotation = deathCamPosition.rotation;
        blurComponent.enabled = true;
        grayscaleComponent.enabled = true;
        wastedImage.enabled = true;
        wastedSound.enabled = true;
        yield return new WaitForSeconds(spawnDelay);
        blurComponent.enabled = false;
        grayscaleComponent.enabled = false;
        wastedImage.enabled = false;
        wastedSound.enabled = false;
    }
}
