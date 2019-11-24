using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAppear : MonoBehaviour {

	private CanvasGroup c;
	private float opacity = 0f;
	private Coroutine routine;
	private float easeSpeed = 15f;

	void Start() {
		c = GetComponent<CanvasGroup>();
	}

	public void enable() {
		routine = StartCoroutine(Appear());
	}

	public void disable() {
		if (routine != null) StopCoroutine(routine);
		routine = StartCoroutine(Disappear());
	}

	IEnumerator Appear() {
		while (opacity < 1f) {
			yield return new WaitForSeconds(0.01f);
			opacity = FourD.ease(opacity, 1f, easeSpeed);
			c.alpha = opacity;
		}
	}

	IEnumerator Disappear() {
		while (opacity > 0f) {
			yield return new WaitForSeconds(0.01f);
			opacity = FourD.ease(opacity, 0f, easeSpeed);
			c.alpha = opacity;
		}
	}
}