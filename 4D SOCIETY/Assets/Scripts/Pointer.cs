using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour {

	public Material leftMat;
	public Material rightMat;

	private IEnumerator leftRoutine;
	private IEnumerator rightRoutine;

	private float leftLerpStart = 0.1f;
	private float leftLerpLimit = 0.7f;
	private float leftLerpInc = 0.99f;

	private float rightLerpStart = 1f;
	private float rightLerpLimit = 0f;
	private float rightLerpInc = 0.5f;

	void Start() {
		leftRoutine = mark();
		rightRoutine = flash();
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit)) {
				StopCoroutine(leftRoutine);
				leftRoutine = mark();
				StartCoroutine(leftRoutine);
				leftMat.SetFloat("_Lerp", leftLerpStart);
				leftMat.SetVector("_Point", hit.point);
			}
		}

		if (Input.GetMouseButtonDown(1)) {
			StopCoroutine(rightRoutine);
			rightRoutine = flash();
			StartCoroutine(rightRoutine);
			rightMat.SetFloat("_Lerp", rightLerpStart);
			rightMat.SetVector("_Start", gameObject.transform.position);
		}
	}

	IEnumerator mark() {
		float lerp = leftLerpStart;
		while (lerp < leftLerpLimit - 0.1f) {
			yield return new WaitForSeconds(0);
			lerp = FourD.ease(lerp, leftLerpLimit, leftLerpInc);
			leftMat.SetFloat("_Lerp", lerp);
		}
	}

	IEnumerator flash() {
		float lerp = rightLerpStart;
		while (lerp > rightLerpLimit + 0.01f) {
			yield return new WaitForSeconds(0);
			lerp = FourD.ease(lerp, rightLerpLimit, rightLerpInc);
			rightMat.SetFloat("_Lerp", lerp);
		}
	}
}
