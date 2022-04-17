using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SimpleGraphData {

	public List<float> x = new List<float>();
	public List<float> y = new List<float>();

}

public class GraphDrawer : MonoBehaviour {

	[ContextMenu("EXPORT TO CSV")]
	public void ExportToCSV() {

		StringBuilder exportText = new StringBuilder();
		exportText.Append("Re,C\n");

		for (int i = 0; i < transform.childCount; i++) {

			exportText.Append(Mathf.Pow(10, transform.GetChild(i).position.x - 1));
			exportText.Append(",");
			exportText.Append(Mathf.Pow(10, transform.GetChild(i).position.y - 2));
			exportText.Append("\n");

		}

		string path = Application.streamingAssetsPath + "/Export";
		if (!File.Exists(path)) {

			Directory.CreateDirectory(path);

		}

		File.WriteAllText(path + "/Drag.csv", exportText.ToString());
		exportText.Clear();

		Debug.Log("EXPORTED!!!!!!!");

	}

	[ContextMenu("EXPORT TO JSON")]
	public void ExportToJSON() {

		SimpleGraphData dat = new SimpleGraphData();

		for (int i = 0; i < transform.childCount; i++) {

			dat.x.Add(Mathf.Pow(10, transform.GetChild(i).position.x - 1));
			dat.y.Add(Mathf.Pow(10, transform.GetChild(i).position.y - 2));

		}

		string path = Application.streamingAssetsPath + "/Export";
		if (!File.Exists(path)) {

			Directory.CreateDirectory(path);

		}

		File.WriteAllText(path + "/Drag.json", JsonUtility.ToJson(dat,true));

	}

	void OnDrawGizmos() {

		if (transform.childCount == 0) return;

		Gizmos.color = Color.green;

		for (int i = 0; i < transform.childCount-1; i++) {

			Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i+1).position);

		}

		for (int i = 0; i < transform.childCount; i++) {

			Gizmos.DrawSphere(transform.GetChild(i).position, 0.05f);

		}

	}

}