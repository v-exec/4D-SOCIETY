using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discover : MonoBehaviour {

	private Vector4[] points;
	private int pointCount = 150;
	private int pointIndex = 0;
	private bool ready = false;
	private float pointInterval = 0.03f;
	private float blenderSpeed = 5f;
	private Coroutine[] routines;
	private float[] amounts;
	public LayerMask mask;
	private float biggestDot = 0.1f;
	private float smallestDot = 5f;
	public Material[] discoverableMaterials;

	void Start() {
		points = new Vector4[pointCount];
		routines = new Coroutine[pointCount];
		amounts = new float[pointCount];
		StartCoroutine(ReadyCycle());
	}

	void Update() {
		//on mouse click at interval,
		//trigger a new point discovery
		if (Input.GetMouseButton(0) && ready) {
			ready = false;

			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~mask)) {
				if (hit.collider.tag == "Discoverable") {
					pointIndex++;
					if (pointIndex == pointCount) pointIndex = 0;

					points[pointIndex] = new Vector4(hit.point.x, hit.point.y, hit.point.z, 0);
					if (routines[pointIndex] != null) StopCoroutine(routines[pointIndex]);
					routines[pointIndex] = StartCoroutine(Blend(pointIndex));
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


	IEnumerator Blend(int index) {
		//reset blending amounts
		amounts[index] = smallestDot;
		for (int i = 0; i < discoverableMaterials.Length; i++) {
			discoverableMaterials[i].SetFloatArray("_Amounts", amounts);
		}

		//add new discovery point to all meshes
		for (int i = 0; i < discoverableMaterials.Length; i++) {
			discoverableMaterials[i].SetVectorArray("_Discoveries", points);
		}

		//ease discovery falloff for visual effect
		while (amounts[index] > biggestDot) {
			yield return new WaitForSeconds(0.01f);
			amounts[index] = FourD.ease(amounts[index], biggestDot, blenderSpeed);
			for (int i = 0; i < discoverableMaterials.Length; i++) {
				discoverableMaterials[i].SetFloatArray("_Amounts", amounts);
			}
		}
	}
}