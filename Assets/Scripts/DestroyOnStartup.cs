using UnityEngine;
using System.Collections;

public class DestroyOnStartup : MonoBehaviour {

	public int lifetime = 5;

	// Use this for initialization
	void Start () {
		Destroy (this.gameObject, lifetime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
