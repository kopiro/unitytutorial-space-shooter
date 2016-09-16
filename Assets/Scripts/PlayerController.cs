using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private Rigidbody rb;

	public Transform boundary;

	public GameObject explosion; 
	public GameObject explosionPlayer; 

	private float speed = 10;
	private float tilt = 3;

	private float nextFire = 0.0f;
	private float fireRate = 0.1f;

	public GameObject shot;
	public Transform shotSpawn;

	public Text UILabelLife;

	private float life = 100;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	void Update () {
		if ((Input.GetButton("Fire1") || Input.GetKey(KeyCode.Space)) && Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
		}
	}

	void FixedUpdate() {
		float mh = Input.GetAxis ("Horizontal");

		rb.velocity = speed * new Vector3 (mh, 0.0f, 0.0f);
		rb.rotation = Quaternion.Euler (tilt / 4 * rb.velocity.z, 0.0f, -1 * tilt * rb.velocity.x);

		float sx = boundary.localScale.x / 2;
		float sy = boundary.localScale.y  / 2;
		float sz = boundary.localScale.z / 2;

		float x = Mathf.Clamp (rb.position.x, boundary.position.x - sx, boundary.position.x + sx);
		float y = Mathf.Clamp (rb.position.y, boundary.position.y - sy, boundary.position.z + sy);
		float z = Mathf.Clamp (rb.position.z, boundary.position.z - sz, boundary.position.z + sz);

		rb.position = new Vector3 (x, y, z);
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
