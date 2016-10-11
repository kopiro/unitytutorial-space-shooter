using UnityEngine;
using System.Collections;

public class EngineController : MonoBehaviour {

	public Rigidbody rb;
	public float power = 2.0f;
	public ParticleSystem particle;

	// Use this for initialization
	void Start () {
		particle.Stop ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartThrust(float ratio) {
		particle.Play ();
		rb.AddForceAtPosition (ratio * power * transform.forward, transform.position);
	}

	public void StartThrust(float ratio, Vector3 vec) {
		particle.Play ();
		rb.AddForceAtPosition (ratio * power * vec, transform.position);
	}

	public void StopThrust() {
		particle.Stop ();

	}
}
