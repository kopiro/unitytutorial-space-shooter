using UnityEngine;
using System.Collections;

public class HazardMover : MonoBehaviour {

	public float speed = 10;
	public float tumble = 4;

	public GameObject explosion; 

	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();

		this.transform.localScale *= Random.Range (0.2f, 1.5f);
		rb.velocity = -1 * Random.Range(0.3f, 1.0f) * speed * transform.forward;
		rb.angularVelocity = tumble * new Vector3 (Random.value, Random.value, Random.value);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag ("Hazard")) {
			Instantiate (explosion, transform.position, transform.rotation);
			Destroy (this.gameObject);
		}
	}
}
