using UnityEngine;
using System.Collections;

public class HazardMover : MonoBehaviour {

	public float speed = 10;
	public float tumble = 4;

	private int life = 5;

	public GameObject explosion; 

	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();

		rb.velocity = Random.Range(0.3f, 1.0f) * speed * transform.forward;
		rb.angularVelocity = tumble * new Vector3 (Random.value, Random.value, Random.value);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag ("Hazard")) {
			Instantiate (explosion, transform.position, transform.rotation);
			Destroy (gameObject);
		} else if (other.CompareTag ("Bolt")) {
			GameObject o = Instantiate (explosion, transform.position, transform.rotation) as GameObject;
			o.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			if (life-- <= 0) {
				Destroy (gameObject);
			}
		} 
	}
}
