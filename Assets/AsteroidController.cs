using UnityEngine;
using System.Collections;

public class AsteroidController : MonoBehaviour {

	private Rigidbody rb;
	public GameObject explosion;

	// Use this for initialization
	void Start () {
		float size = Random.Range (50, 300);
		transform.localScale = size * new Vector3 (1, 1, 1);
		rb = GetComponent<Rigidbody> ();
		rb.angularVelocity = new Vector3 (Random.value, Random.value, Random.value) * 20.0f;
		rb.velocity = new Vector3 (Random.value * 5.0f, -1.0f * Random.value * 300.0f, Random.value * 5.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision col) {
		if (col.gameObject.CompareTag ("Terrain")) {
			Instantiate (explosion, transform.position, Quaternion.identity);
			Destroy (gameObject);
		}
	}
}
