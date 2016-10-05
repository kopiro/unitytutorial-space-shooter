using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private Rigidbody rb;

	public Transform boundary;

	public GameObject explosion; 
	public GameObject explosionPlayer; 

	public Transform engineLeft;
	public Transform engineRight;
	public Transform engineCenter;

	public Transform cam;
	private Vector3 cameraOffset;

	private Vector3 rotation = Vector3.zero;
	private float speed = 10;
	private float tilt = 3;

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
		cameraOffset = transform.position - cam.position;
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
		Debug.Log (Input.GetAxis ("Vertical"));
		rb.AddRelativeForce (new Vector3 (0.0f, (rb.mass * 10.0f * 10.0f) * (0.5f * (1.0f + Input.GetAxis("Vertical"))), 0.0f));
			
		Debug.Log (Input.GetAxis ("Accelerator"));
		if (Input.GetAxis ("Accelerator") > 0) {
			engineCenter.GetComponent<EngineController> ().StartThrust ( Input.GetAxis ("Accelerator") );
		} else {
			engineCenter.GetComponent<EngineController> ().StopThrust ();
		}
							
		if (Input.GetAxis ("Horizontal") > 0) {
			engineRight.GetComponent<EngineController> ().StartThrust ( Mathf.Abs(Input.GetAxis ("Horizontal")) );
			engineLeft.GetComponent<EngineController> ().StopThrust ();
		} else if (Input.GetAxis ("Horizontal") < 0) {
			engineLeft.GetComponent<EngineController> ().StartThrust ( Mathf.Abs(Input.GetAxis ("Horizontal")) );
			engineRight.GetComponent<EngineController> ().StopThrust ();
		} else {
			engineLeft.GetComponent<EngineController> ().StopThrust ();
			engineRight.GetComponent<EngineController> ().StopThrust ();
		}

		rb.rotation = Quaternion.Lerp (rb.rotation, Quaternion.Euler (0.0f, rb.rotation.eulerAngles.y, 0.0f), 50.0f);
			
		rb.velocity *= 0.98f;

		cam.transform.position = Vector3.Lerp (cam.transform.position, transform.position - cameraOffset, 100.0f);
		cam.transform.rotation = Quaternion.Euler (0.0f, transform.rotation.eulerAngles.y, 0.0f);

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
