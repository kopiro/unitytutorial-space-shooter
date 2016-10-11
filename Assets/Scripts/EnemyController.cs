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
	public Transform shotSpawnLeft;
	public Transform shotSpawnRight;

	public Text UILabelLife;

	private float tilt = 2;

	private float nextFire = 2.0f;
	private float fireRate = 0.1f;
	private float currentFireRate = 0;

	// Difficulty here
	private float smooth = 0.3f;

	private Vector3 pointToAvoidBolt;
	private Vector3 latestBoltPoint;
	private bool boltIsGoingToCollide = false;

	private int life = 100;
	private int prevLife = 100;

	private float avoidX = 4.0f;
	private float minDist = 2.0f;

	private int painFrameChecker = 0;
	private int deltaLife = 0;


	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody> ();
	}

	void FixedUpdate () {
		return;

		transform.position = Vector3.SmoothDamp (transform.position, targetPoint, ref speedVector, smooth);
		rb.rotation = Quaternion.Euler (tilt / 4 * speedVector.z, 180, -1 * tilt * speedVector.x);

		if (painFrameChecker++ % 50 == 0) {
			deltaLife = prevLife - life;
			prevLife = life;
		}

		Process ();
		ProcessFire ();
	}

	void Process() {
		if (boltIsGoingToCollide == false && deltaLife < 1) {
			EnterAttackMode ();
		} else {
			EnterDefenseMode ();
		}
	}

	float GetX() {
		return player.transform.position.x;
	}

	float GetY() {
		return player.transform.position.y;
	}

	float GetZ() {
		return player.transform.position.z + 10.0f;
	}

	void ProcessFire() {
		float dist = Mathf.Abs (player.transform.position.x - transform.position.x);

		if (Time.time > nextFire && (dist < minDist || Random.Range(0,2) == 1) && currentFireRate < 10.0f) {
			nextFire = Time.time + fireRate + (dist / minDist);
			currentFireRate += 1.0f;
			Instantiate (shot, shotSpawnLeft.position, shotSpawnLeft.rotation);
			Instantiate (shot, shotSpawnRight.position, shotSpawnRight.rotation);
		} else {
			currentFireRate = Mathf.Max (0, currentFireRate - 0.1f);
		}
	}

	void EnterAttackMode() {
		targetPoint = new Vector3 (
			GetX(), 
			GetY(), 
			GetZ()
		);
	}

	void EnterDefenseMode() {
		targetPoint = new Vector3 (
			pointToAvoidBolt.x, 
			transform.position.y, 
			GetZ()
		);
	}

	void OnTriggerEnter(Collider other) {
		if (CompareTag ("Boundary")) {
			return;
		}

		if (other.CompareTag("Bolt")) {
			life--;
			boltIsGoingToCollide = false;
			Instantiate (explosion, other.transform.position, other.transform.rotation);
			Destroy (other.gameObject);
		} 

		UILabelLife.text = life.ToString();

		if (life <= 0) {
			Instantiate (explosionEnemy, other.transform.position, other.transform.rotation);
			Destroy(gameObject);
		}
	}


	public void boltIsColliding(Vector3 p) {
		boltIsGoingToCollide = true;

		float xMax = boundary.position.x + boundary.localScale.x / 2;

		ArrayList choicesX = new ArrayList ();

		if (transform.position.x - p.x < minDist && (p.x - avoidX) > -xMax) choicesX.Add (p.x - avoidX);
		if (transform.position.x - p.x > -minDist && (p.x + avoidX) < xMax) choicesX.Add (p.x + avoidX);
	
		if (choicesX.Count == 0) return;
		float x = (float)choicesX [Random.Range (0, choicesX.Count)];
	
		pointToAvoidBolt = new Vector3(x, transform.position.y, transform.position.z);
	}

	public void boltIsGoingOut() {
		boltIsGoingToCollide = false;
	}

}
