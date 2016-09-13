using UnityEngine;
using System.Collections;

public class DestroyOnContact : MonoBehaviour {

	public GameObject explosion; 
	public GameObject playerExplosion; 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	void OnTriggerEnter(Collider other) {
		Debug.Log ("Contact with " + other.name);
		if (other.CompareTag ("Boundary"))
			return;

		if (other.CompareTag ("Player")) {
			Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
		} else {
			Instantiate (explosion, transform.position, transform.rotation);
		}

		Destroy (other.gameObject);
		Destroy (this.gameObject);
	}
}
