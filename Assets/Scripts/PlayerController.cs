using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private Rigidbody rb;

	public GameObject explosion; 
	public ParticleSystem engineCenter;

	public Transform cam;
	private Vector3 cameraOffset;

	private float accelerationCenter = 1.0f;

	private float nextFire = 0.0f;
	private float fireRate = 0.1f;
	private float currentFireRate = 0;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		cameraOffset = cam.transform.position - transform.position;
	}

	void Update () {
//		if (Input.GetButton("Fire") && Time.time > nextFire && currentFireRate < 10.0f) {
//			nextFire = Time.time + fireRate;
//			currentFireRate += 1.0f;
//			Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
//		} else {
//			currentFireRate = Mathf.Max (0, currentFireRate - 0.1f);
//		}
	}

	void FixedUpdate() {
		Quaternion rot = transform.rotation;

		if (Input.GetButton ("EngineCenter")) {
			// rot = Quaternion.Euler (rot.eulerAngles.x * 0.99f, rot.eulerAngles.y, rot.eulerAngles.z);
			rb.velocity = rb.velocity + (accelerationCenter * transform.forward);
			engineCenter.Play ();
		} else {
			engineCenter.Stop ();
		}

		float speedForward = rb.velocity.magnitude;
		Debug.Log (rot.eulerAngles.z);

		rot *= Quaternion.AngleAxis ((1.0f * Input.GetAxisRaw ("Vertical")), Vector3.right);
		rot *= Quaternion.AngleAxis ((1.0f * Input.GetAxisRaw ("Horizontal")), Vector3.up);
		rot *= Quaternion.AngleAxis (-(1.0f * Input.GetAxisRaw ("Horizontal")), Vector3.forward);

//					
//		rot = Quaternion.Euler (
//			Mathf.Clamp ( 
//			(rot.eulerAngles.x >= 180 ? rot.eulerAngles.x - 360 : rot.eulerAngles.x) + 
//			(1.0f * Input.GetAxisRaw ("Vertical")), 
//			-80.0f, 80.0f), 
//			rot.eulerAngles.y + ((1.0f * Input.GetAxisRaw ("Horizontal"))),
//			Mathf.Clamp ( 
//			(rot.eulerAngles.z >= 180 ? rot.eulerAngles.z - 360 : rot.eulerAngles.z) - 
//			(1.0f * Input.GetAxisRaw ("Horizontal")), 
//			-80.0f, 80.0f)
//		);
//
		transform.rotation = rot;

		cam.transform.position = transform.position + Quaternion.AngleAxis (rot.eulerAngles.y, Vector3.up) * cameraOffset;
		cam.transform.LookAt (transform.position + transform.up);

		Camera camComponent = cam.GetComponent<Camera> ();
		camComponent.fieldOfView = Mathf.Lerp (
			camComponent.fieldOfView,
			40.0f + (speedForward * 1.0f),
			Time.deltaTime
		);
	}

	void OnCollisionEnter(Collision col) {
		if (col.gameObject.CompareTag ("Terrain")) {
			Destroy (gameObject);
			Instantiate (explosion, transform.position, Quaternion.identity);
		}
	}


}
