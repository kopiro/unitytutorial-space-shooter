using UnityEngine;
using System.Collections;

public class HazardMover : MonoBehaviour {

	public float speed = 10;

	// Use this for initialization
	void Start () {
		this.transform.localScale *= Random.Range (0.2f, 1.5f);
		GetComponent<Rigidbody> ().velocity = -1 * Random.Range(0.3f, 1.0f) * speed * transform.forward;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
