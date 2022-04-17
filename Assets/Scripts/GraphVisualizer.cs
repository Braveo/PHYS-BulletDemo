using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GraphVisualizer : MonoBehaviour {

	public GraphData data = new GraphData();
	public float timestep = 0.1f;

	public LineRenderer line;

	void Start() {
		data.Start();

		StartCoroutine(EnDrawLine());
	}

	IEnumerator EnDrawLine() {

		if (line) {

			List<Vector3> a = new List<Vector3>();

			line.positionCount = 0;

			for (float i = 0; i < data.EndValX(); i += timestep) {

				a.Add(new Vector2(i, data.Evaluate(i) * 10000));
				line.positionCount += 1;

				line.SetPositions(a.ToArray());
				yield return new WaitForSeconds(0.01f);

			}

			

		}

	}

	//void Update() {

	//	for (float i = 0; i < data.EndValX(); i += timestep) {

	//		Debug.DrawRay(new Vector2(i, data.Evaluate(i)), Vector3.up * 0.05f, Color.red);

	//	}

	//}

}
