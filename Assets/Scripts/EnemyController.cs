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
	public GameObject hazard;
	public Transform shotSpawn;

	public Text UILabelLife;

	private float tilt = 2;

	private float nextFire = 2.0f;
	private float maxFireRate = 0.1f;

	private float smooth = 0.3f;

	private Vector3 pointBecauseOfPain;
	private Vector3 latestBoltPoint;

	private int life = 100;
	private int prevLife = 100;

	private float avoidX = 2.0f;
	private float minDist = 1.0f;

	private int painFrameChecker = 0;
	private int painDetector = 0;

	private int deltaLife = 0;


	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody> ();
	}

	void FixedUpdate () {
		transform.position = Vector3.SmoothDamp (transform.position, targetPoint, ref speedVector, smooth);
		rb.rotation = Quaternion.Euler (tilt / 4 * speedVector.z, 180, -1 * tilt * speedVector.x);

		if (painFrameChecker++ % 50 == 0) {
			deltaLife = prevLife - life;
			prevLife = life;
		}

		process ();
		ProcessFire ();
	}

	void process() {
		if (deltaLife > 1) {
			EnterDefenseMode ();
		} else {
			EnterAttackMode ();
		}
	}

	void ProcessFire() {
		float dist = Mathf.Abs (player.transform.position.x - transform.position.x);
		if (Time.time > nextFire && dist < minDist) {
			nextFire = Time.time + Random.Range (0.0f, 0.2f + (maxFireRate * ( dist / minDist )));

			if (Random.Range (0, 10) == 0) {
				Instantiate (hazard, shotSpawn.position, shotSpawn.rotation);
			} else {
				Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
			}
		}
	}

	void EnterAttackMode() {
		Debug.Log ("ATTACHKK!");
		float maxZ = boundary.position.z + boundary.localScale.z / 2;

		if (latestBoltPoint != null && Mathf.Abs (player.transform.position.x - latestBoltPoint.x) < 2) {
			EnterDefenseMode ();
		} else {

			targetPoint = new Vector3 (
				player.transform.position.x, 
				transform.position.y, 
				maxZ
			);

		}
	}

	void EnterDefenseMode() {
		Debug.Log ("DEFNSE");
		float maxZ = boundary.position.z + boundary.localScale.z / 2;

		targetPoint = new Vector3 (
			pointBecauseOfPain.x, 
			transform.position.y, 
			maxZ
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


	public void boltIsColliding(Vector3 p) {
		latestBoltPoint = p;
		float xMax = boundary.position.x + boundary.localScale.x / 2;

		float newX;
		float xLeft = p.x - avoidX;
		float xRight = p.x + avoidX;

		if (transform.position.x - p.x < minDist) {
			newX = xLeft;
		} else if (transform.position.x - p.x > -minDist) {
			newX = xRight;
		} else {
			return;
		}

		if (newX >= xMax) {
			newX = xLeft;
		} else if (newX <= -xMax) {
			newX = xRight;
		} 

		pointBecauseOfPain = new Vector3(newX, transform.position.y, transform.position.z);
	}

}
