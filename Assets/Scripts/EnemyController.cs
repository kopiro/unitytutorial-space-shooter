using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public GameObject explosion; 
	public GameObject explosionEnemy;

	public Transform boundary;	 


	private Rigidbody rb;

	private Vector3 targetPoint;

	public GameObject shot;

	public float nextFire = 0.0f;
	public float fireRate = 0.5f;

	private float damage = 0;

	// Use this for initialization
	void Start () {
		this.rb = this.GetComponent<Rigidbody> ();
		this.CalculateNewTargetPoint ();
	}

	void FixedUpdate () {
		Vector3 speedVector = this.transform.position - this.targetPoint;
		if (Mathf.Abs(speedVector.x) < 1) {
			this.CalculateNewTargetPoint ();
			return;
		}
						
		this.rb.velocity = -speedVector;

		Debug.Log (Time.time + "-"+ nextFire);

		if (Time.time > nextFire) {
			nextFire = Time.time + fireRate + Random.Range (0, 0.5f);;
			Instantiate (shot, this.transform.position, this.transform.rotation);
		}
	}

	void CalculateNewTargetPoint() {
		Debug.Log ("Recalc!");
		float w = boundary.localScale.x / 2;
		this.targetPoint = new Vector3 (Random.Range (-w, w), this.transform.position.y, this.transform.position.z);
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag ("Boundary"))
			return;

		if (other.CompareTag ("BoltEnemy"))
			return;

		if (other.CompareTag("Bolt")) {
			this.damage++;
			if (this.damage > 10) {
				Instantiate (explosionEnemy, other.transform.position, other.transform.rotation);
				Destroy(this.gameObject);
			}
		}
		
		Instantiate (explosion, other.transform.position, other.transform.rotation);
		Destroy (other.gameObject);
	}
}
