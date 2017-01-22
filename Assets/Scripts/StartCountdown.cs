using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartCountdown : MonoBehaviour {
    [HideInInspector]
    public float delai;
    public Text countdown; 

    private float total;

	// Use this for initialization
	void Start () {
        total = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        total += Time.deltaTime;
        countdown.text = (int)(delai - total) == 0?"Go!":((int)(delai - total)).ToString();
	}
}
