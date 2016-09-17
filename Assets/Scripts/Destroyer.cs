using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour {

	void OnTriggerExit(Collider other) {
		if (other.CompareTag ("Player"))
			return;
		if (other.CompareTag ("Enemy"))
			return;
		
		Destroy (other.gameObject);
	}
}
