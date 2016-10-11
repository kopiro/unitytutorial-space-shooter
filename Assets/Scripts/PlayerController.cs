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
	private Vector3 cameraOffsetRotation;

	private Vector3 rotation = Vector3.zero;
	private float speed = 10;
	private float tilt = 3;

	private float xRotation = 0;

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
		if (Input.GetAxis ("R2") > 0) {
			engineCenter.GetComponent<EngineController> ().StartThrust (Input.GetAxis ("R2"));
		} else {
			engineCenter.GetComponent<EngineController> ().StopThrust ();
		}

		if (Input.GetAxis ("L2") > 0) {
			engineBottom.GetComponent<EngineController> ().StartThrust (Input.GetAxis ("L2"), Vector3.up);
		} else {
			engineBottom.GetComponent<EngineController> ().StopThrust ();
		}
			
		float newYR = transform.rotation.eulerAngles.y + (Input.GetAxis ("Horizontal"));
		transform.rotation = Quaternion.Euler (0.0f, newYR, 0.0f);
	}

	void LateUpdate() {
		cameraOffset = Quaternion.AngleAxis (Input.GetAxis("CameraX") * 3.0f, Vector3.up) * cameraOffset;
		cameraOffset = Quaternion.AngleAxis (Input.GetAxis("CameraY") * 3.0f, new Vector3(1,0,0)) * cameraOffset;
		cam.transform.position = transform.position + cameraOffset;
		cam.transform.LookAt (transform.position);
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
