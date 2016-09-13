using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject hazard;
	public Transform boundary;


	// Use this for initialization
	void Start () {
		this.SpawnWaves ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Mathf.Floor(Random.value * 100) % 10 == 0) {
			SpawnWaves ();
		}
	}

	void SpawnWaves() {
		Vector3 s = new Vector3(Random.Range(-boundary.localScale.x, boundary.localScale.x), 0, 30);
		Instantiate (hazard, s, Quaternion.identity);
	}
}
