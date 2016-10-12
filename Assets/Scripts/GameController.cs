using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject boundary;
	public GameObject player;
	public GameObject asteroid;

	void Start () {
		
	}

	void FixedUpdate() {
		if (Random.Range(0,20) == 0) {
			Instantiate (asteroid, 
				new Vector3 (
					Random.Range(boundary.transform.position.x - boundary.transform.localScale.x / 2.0f, boundary.transform.position.x + boundary.transform.localScale.x / 2.0f),
					boundary.transform.position.y + boundary.transform.localScale.y / 2.0f,
					Random.Range(boundary.transform.position.z - boundary.transform.localScale.z / 2.0f, boundary.transform.position.z + boundary.transform.localScale.z / 2.0f)
				),
				Quaternion.identity);
		}
	}
		
}
