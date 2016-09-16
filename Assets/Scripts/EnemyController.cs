using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public GameObject explosion; 
	public GameObject explosionEnemy;

	public GameObject player;

	public Transform boundary;	 

	private Rigidbody rb;
	private Vector3 targetPoint;
	private Vector3 speedVector = Vector3.zero;

	public GameObject shot;

	public Text UILabelLife;

	private float tilt = 2;

	private float nextFire = 2.0f;
	private float maxFireRate = 0.1f;

	private float smooth = 0.3f;

	private Vector3 pointToAvoid;

	private float life = 100;

	private float avoidX = 2.0f;
	private float minDist = 1.0f;

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody> ();
	}

	void FixedUpdate () {
		transform.position = Vector3.SmoothDamp (transform.position, targetPoint, ref speedVector, smooth);
		rb.rotation = Quaternion.Euler (tilt / 4 * speedVector.z, 180, -1 * tilt * speedVector.x);

		ProcessFire ();

		CalculateNewTargetPoint ();
	}

	void ProcessFire() {
		float dist = Mathf.Abs (player.transform.position.x - transform.position.x);
		if (Time.time > nextFire && dist < minDist) {
			nextFire = Time.time + Random.Range (0.0f, 0.1f + (maxFireRate * ( dist / minDist )));
			Instantiate (shot, transform.position, transform.rotation);
		}
	}

	void CalculateNewTargetPoint() {
		float maxZ = boundary.position.z + boundary.localScale.z / 2;

		targetPoint = new Vector3 (
			player.transform.position.x, 
			transform.position.y, 
			maxZ + Random.Range(-2.0f,2.0f)
		);
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("EnemyContoller.OnTriggerEnter = " + other.name);

		if (CompareTag ("Boundary")) {
			return;
		}

		if (other.CompareTag("Bolt")) {
			life--;
			Instantiate (explosion, other.transform.position, other.transform.rotation);
			Destroy (other.gameObject);
		} 

		UILabelLife.text = life.ToString();

		if (life <= 0) {
			Instantiate (explosionEnemy, other.transform.position, other.transform.rotation);
			Destroy(gameObject);
		}
	}


	public void boltIsColliding(Vector3 pointToAvoid) {
		float xMax = boundary.position.x + boundary.localScale.x / 2;

		float newX;
		float xLeft = pointToAvoid.x - avoidX;
		float xRight = pointToAvoid.x + avoidX;

		if (transform.position.x - pointToAvoid.x < minDist) {
			newX = xLeft;
		} else if (transform.position.x - pointToAvoid.x > -minDist) {
			newX = xRight;
		} else {
			return;
		}

		if (newX >= xMax) {
			newX = xLeft;
		} else if (newX <= -xMax) {
			newX = xRight;
		} 

		targetPoint.Set (newX, targetPoint.y, targetPoint.z);
	}

}
