using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discover : MonoBehaviour {

	private Vector3[] points;
	private int pointCount = 10;
	private int pointIndex = 0;
	private bool ready = false;
	private float pointInterval = 0.2f;
	public LayerMask mask;

	void Start() {
		points = new Vector3[pointCount];
		StartCoroutine(ReadyCycle());
	}

	void Update() {
		if (Input.GetMouseButton(0) && ready) {
			ready = false;

			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~mask)) {
				if (hit.collider.tag == "Discoverable") {
					pointIndex++;
					if (pointIndex == pointCount) pointIndex = 0;
					points[pointIndex] = hit.point;
					updateDiscoverMeshes();
				}
			}
		}
	}

	IEnumerator ReadyCycle() {
		while (true) {
			yield return new WaitForSeconds(pointInterval);
			ready = true;
		}
	}

	void updateDiscoverMeshes() {
		
	}
}