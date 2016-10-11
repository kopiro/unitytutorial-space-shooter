using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private Rigidbody rb;

	public Transform boundary;

	public GameObject explosion; 
	public GameObject explosionPlayer; 

	public Transform engineCenter;
	public Transform engineBottom;

	public Transform cam;
	private Vector3 cameraOffset;

	private float rot = 0;
	private float speed = 10;
	private float tilt = 0.1f;

	private float lerpCamTime = 0.0f;
	private float maxLerpCamTime = 4.0f;

	private float lerpRotation = 0.0f;
	private float maxLerpRotationTime = 20.0f;

	private float lerpTilt = 0.0f;
	private float maxLerpTiltTime = 20.0f;

	private float nextFire = 0.0f;
	private float fireRate = 0.1f;
	private float currentFireRate = 0;

	public GameObject shot;
	public Transform shotSpawn;

	public Text UILabelLife;

	private float life = 100;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		cameraOffset = cam.transform.position - transform.position;
	}

	void Update () {
		if (Input.GetKey(KeyCode.JoystickButton16)  && Time.time > nextFire && currentFireRate < 10.0f) {
			nextFire = Time.time + fireRate;
			currentFireRate += 1.0f;
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
		} else {
			currentFireRate = Mathf.Max (0, currentFireRate - 0.1f);
		}
				}

	void FixedUpdate() {
		if (Input.GetButton ("EngineCenter")) {
			engineCenter.GetComponent<EngineController> ().StartThrust (1);
		} else {
			engineCenter.GetComponent<EngineController> ().StopThrust ();
		}

		if (Input.GetButton ("EngineBottom")) {
			engineBottom.GetComponent<EngineController> ().StartThrust (1, transform.up);
		} else {
			engineBottom.GetComponent<EngineController> ().StopThrust ();
		}

		if (Input.GetAxis ("Horizontal") != 0.0f) {
			rot += Input.GetAxis ("Horizontal");

			tilt += -1 * Input.GetAxis ("Horizontal");
			tilt = Mathf.Clamp (tilt, -90.0f, 90.0f);
		}
			
		transform.rotation = Quaternion.Euler (0.0f, rot, tilt);

		cam.transform.position = Vector3.Lerp (
			cam.transform.position,
			transform.position + (Quaternion.AngleAxis (rot, Vector3.up) * cameraOffset),
			0.8f + Time.smoothDeltaTime
		);
		cam.transform.LookAt (transform.position + 1.0f * Vector3.up);

	}
		

	void OnTriggerEnter(Collider other) {
		if (this.CompareTag ("Boundary")) {
			return;
		}

		if (other.CompareTag ("BoltEnemy")) {
			this.life--;
			Instantiate (explosion, other.transform.position, other.transform.rotation);
			Destroy (other.gameObject);
		} else if (other.CompareTag ("Hazard")) {
			this.life -= 10;
			Instantiate (explosion, other.transform.position, other.transform.rotation);
			Destroy (other.gameObject);
		}

		UILabelLife.text = this.life.ToString();

		if (this.life <= 0) {
			Instantiate (explosionPlayer, this.transform.position, this.transform.rotation);
			Destroy (this.gameObject);
		}
	}


}
