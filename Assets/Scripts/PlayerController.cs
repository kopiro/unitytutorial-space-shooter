using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private Rigidbody rb;

	public Transform boundary;

	public GameObject explosion; 
	public GameObject explosionPlayer; 

	public GameObject engine;

	public Transform rotorLeft;
	public Transform rotorRight;

	public Transform cam;
	private Vector3 cameraOffset;

	private float rotation = 0;
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
		if (Input.GetButton ("Fire1") && Time.time > nextFire && currentFireRate < 10.0f) {
			nextFire = Time.time + fireRate;
			currentFireRate += 1.0f;
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
		} else {
			currentFireRate = Mathf.Max (0, currentFireRate - 0.1f);
		}
	}

	void FixedUpdate() {
		bool engineActive = false;

		if (Input.GetKey (KeyCode.W)) {
			engineActive = true;
			rb.AddRelativeForce (new Vector3 (0.0f, 14.0f, 0.0f), ForceMode.Acceleration);
		}

		if (Input.GetKey (KeyCode.S)) {
			engineActive = true;
			rb.AddRelativeForce (new Vector3 (0.0f, -14.0f, 0.0f), ForceMode.Acceleration);
		}

		if (Input.GetKey (KeyCode.Space)) {
			engineActive = true;
			rb.AddRelativeForce (new Vector3 (0, 0, 10.0f), ForceMode.Acceleration);
		}

		if (Input.GetKey (KeyCode.A)) {
			rotation -= 1.0f;
		}

		if (Input.GetKey (KeyCode.D)) {
			rotation += 1.0f;
		}

		rb.rotation = Quaternion.Euler (0.0f, rotation, 0.0f);


		engine.SetActive (engineActive);

			
		//float x = Mathf.Clamp (rb.position.x, boundary.position.x - (boundary.localScale.x / 2), boundary.position.x + (boundary.localScale.x / 2));
		//float y = Mathf.Clamp (rb.position.y, boundary.position.y - (boundary.localScale.y / 2), boundary.position.z + (boundary.localScale.y / 2));
		//float z = Mathf.Clamp (rb.position.z, boundary.position.z - (boundary.localScale.z / 2), boundary.position.z + (boundary.localScale.z / 2));
		//rb.position = new Vector3 (x, y, z);
		//cam.position = Vector3.Lerp (cam.position, transform.position, 6.0f);
		//cam.rotation = Quaternion.Lerp(cam.rotation, transform.rotation, 6.0f);
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
