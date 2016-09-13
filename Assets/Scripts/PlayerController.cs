using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary : System.Object {
	public float xMin;
	public float xMax;
	public float zMin;
	public float zMax;
}

public class PlayerController : MonoBehaviour {

	private Rigidbody rb;

	public Boundary boundary;
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

		rb.position = new Vector3 (
			Mathf.Clamp (rb.position.x, boundary.xMin, boundary.xMax),
			0.0f,
			Mathf.Clamp (rb.position.z, boundary.zMin, boundary.zMax)
		);
	}

	void LateUpdate() {
		
	}

}
