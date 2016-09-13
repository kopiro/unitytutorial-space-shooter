using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {

	private Rigidbody rb;

	public float speed;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		rb.velocity = speed * transform.forward;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
