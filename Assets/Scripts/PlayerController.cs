using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private Rigidbody rb;

	public Transform boundary;

	public float speed;
	public float tilt;

	public GameObject shot;
	public Transform shotSpawn;

	public float nextFire = 0.0f;
	public float fireRate = 0.5f;


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
		float mv = Input.GetAxis ("Vertical");

		rb.velocity = speed * new Vector3 (mh, 0.0f, mv);
		rb.rotation = Quaternion.Euler (tilt / 4 * rb.velocity.z, 0.0f, -1 * tilt * rb.velocity.x);

		float sx = boundary.localScale.x / 2;
		float sy = boundary.localScale.y  / 2;
		float sz = boundary.localScale.z / 2;

		float x = Mathf.Clamp (rb.position.x, boundary.position.x - sx, boundary.position.x + sx);
		float y = Mathf.Clamp (rb.position.y, boundary.position.y - sy, boundary.position.z + sy);
		float z = Mathf.Clamp (rb.position.z, boundary.position.z - sz, boundary.position.z + sz);

		rb.position = new Vector3 (x, y, z);
	}

	void LateUpdate() {
		
	}

}
