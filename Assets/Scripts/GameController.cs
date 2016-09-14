using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject[] hazards;
	public Transform boundary;
	public GameObject enemy;

	public float hazardStartWait = 1;
	public float hazardWait = 0.04f;
	public float hazardCycleWait = 3;
	public int hazardCount = 10;
	public int pointsToWakeEnemy = 5;

	private int points = 0;
	private IEnumerator waveRoutine;

	private static GameController instance;
	public static GameController Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<GameController> ();
				if (instance == null) {
					GameObject obj = new GameObject ();
					obj.hideFlags = HideFlags.HideAndDontSave;
					instance = obj.AddComponent<GameController> ();
				}
			}
			return instance;
		}
	}

	// Use this for initialization
	void Start () {
		this.waveRoutine = SpawnWaves ();
		StartCoroutine (this.waveRoutine);
	}

	IEnumerator SpawnWaves() {
		float x = boundary.localScale.x / 2;
		float z = boundary.localScale.z / 2;

		yield return new WaitForSeconds(hazardStartWait);
		while (true) {
			for (int i = 0; i < Random.Range(0, hazardCount); i++) {
				Vector3 s = new Vector3(Random.Range(-x, x), 0, boundary.position.z + z * 2);
				Instantiate (hazards[Random.Range(0, hazards.Length)], s, Quaternion.identity);
				yield return new WaitForSeconds(Random.Range(0, hazardWait));
			}
			yield return new WaitForSeconds(Random.Range(0, hazardCycleWait));
		}
	}

	public void AddPoint() {
		this.points++;
		Debug.Log ("Add points = " + this.points);

		if (this.points == pointsToWakeEnemy) {
			this.SpawnEnemy ();
		}
	}

	public void SpawnEnemy() {
		StopCoroutine (this.waveRoutine);
		Instantiate (enemy);
	}

}
