using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public GameObject explosion; 
	public GameObject explosionEnemy;

	public Transform boundary;	 

	private Rigidbody rb;
	private Vector3 targetPoint;
	private Vector3 speedVector = Vector3.zero;

	public GameObject shot;

	public Text UILabelLife;

	public float speed = 10;
	public float tilt = 20;
	public float nextFire = 0.0f;
	public float fireRate = 0.5f;
	public float smooth = 0.5f;

	private Vector3 pointToAvoid;

	private float life = 100;

	public float avoidX = Random.Range (2.0f, 5.0f);
	public float minDist = Random.Range(1.0f, 2.0f);

	// Use this for initialization
	void Start () {
		this.rb = this.GetComponent<Rigidbody> ();
		this.CalculateNewTargetPoint ();
	}

	void FixedUpdate () {

		Vector3 diff = this.transform.position - this.targetPoint;
		if (Mathf.Abs (diff.x) < 1 && Mathf.Abs (diff.z) < 1) {
			this.CalculateNewTargetPoint ();
			return;
		}

		// Move
		this.transform.position = Vector3.SmoothDamp (
			this.transform.position,
			this.targetPoint,
			ref speedVector,
			this.smooth
		);
		this.rb.rotation = Quaternion.Euler (
			tilt / 4 * speedVector.z, 
			180,
			-1 * tilt * speedVector.x
		);

		// Fire
		if (Time.time > nextFire) {
			nextFire = Time.time + fireRate + Random.Range (0, 0.2f);
			Instantiate (shot, this.transform.position, this.transform.rotation);
		}
		
	}

	void CalculateNewTargetPoint() {
		float w = boundary.position.x + boundary.localScale.x / 2;
		float z = boundary.position.z + boundary.localScale.z / 2;

		this.targetPoint = new Vector3 (
			Random.Range(w, -w), 
			this.transform.position.y, 
			z + Random.Range(-2,2)
		);
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("EnemyContoller.OnTriggerEnter = " + other.name);

		if (this.CompareTag ("Boundary")) {
			return;
		}

		if (other.CompareTag("Bolt")) {
			this.life--;
			Instantiate (explosion, other.transform.position, other.transform.rotation);
			Destroy (other.gameObject);
		} else if (other.CompareTag ("Hazard")) {
			this.life--;
			Instantiate (explosion, other.transform.position, other.transform.rotation);
			Destroy (other.gameObject);
		}

		UILabelLife.text = this.life.ToString();

		if (this.life <= 0) {
			Instantiate (explosionEnemy, other.transform.position, other.transform.rotation);
			Destroy(this.gameObject);
		}
	}


	public void boltIsColliding(Vector3 pointToAvoid) {
		float w = boundary.position.x + boundary.localScale.x / 2;

	

		float newX;
		float xLeft = pointToAvoid.x - avoidX;
		float xRight = pointToAvoid.x + avoidX;

		if (this.transform.position.x - pointToAvoid.x < minDist) {
			newX = xLeft;
		} else if (this.transform.position.x - pointToAvoid.x > -minDist) {
			newX = xRight;
		} else {
			return;
		}

		if (newX >= w) {
			newX = xLeft;
		} else if (newX <= -w) {
			newX = xRight;
		} 

		this.targetPoint.Set (newX, this.targetPoint.y, this.targetPoint.z);
	}

}
