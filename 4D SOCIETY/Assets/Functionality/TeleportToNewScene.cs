using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportToNewScene : MonoBehaviour {
	public string next;

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			SceneManager.LoadScene(next);
		}
	}
}