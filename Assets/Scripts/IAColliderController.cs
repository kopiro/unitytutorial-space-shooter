﻿using UnityEngine;
using System.Collections;

public class IAColliderController : MonoBehaviour {

	public GameObject parent;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = new Vector3(parent.transform.position.x, parent.transform.position.y, parent.transform.position.z - 2.0f);
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag ("Bolt")) {
			EnemyController cnt = parent.GetComponent<EnemyController> () as EnemyController;
			cnt.boltIsColliding (other.transform.position);
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.CompareTag("Bolt")) {
			EnemyController cnt = parent.GetComponent<EnemyController> () as EnemyController;
			cnt.boltIsGoingOut ();
		}
	}
}
