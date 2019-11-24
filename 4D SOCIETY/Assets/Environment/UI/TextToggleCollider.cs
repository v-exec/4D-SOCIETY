using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextToggleCollider : MonoBehaviour {

	private bool zero = true;
	public TextAppear text;
	public bool enabler = false;

	private void OnTriggerEnter(Collider other) {
		if (zero) {
			if (other.tag == "Player") {
				zero = false;
				if (enabler) text.enable();
				else text.disable();
			}
		}
	}
}
