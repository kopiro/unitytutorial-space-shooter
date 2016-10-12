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

	public void StartThrust() {
		particle.Play ();
	}
		
	public void StopThrust() {
		particle.Stop ();

	}
}
