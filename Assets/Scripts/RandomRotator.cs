using UnityEngine;
using System.Collections;

public class RandomRotator : MonoBehaviour {

	public float tumble;

	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		rb.angularVelocity = new Vector3 (Random.value, Random.value, Random.value) * tumble;
	}

	void Update() {
	}

}
