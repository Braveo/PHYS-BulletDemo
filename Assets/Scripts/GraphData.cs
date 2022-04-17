using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GraphData {

	string path = Application.streamingAssetsPath + "/Export/Drag.json";

	public bool Initialized => dat.x.Count > 0;

	SimpleGraphData dat;

	public void Start() {

		string json = File.ReadAllText(path);
		Debug.Log(json);

		dat = (SimpleGraphData)JsonUtility.FromJson(json, typeof(SimpleGraphData));

	}

	public float EndValX() {

		return dat.x.Count != 0 ? dat.x[dat.x.Count - 1] : 0;

	}

	public float EndValY() {

		return dat.y.Count != 0 ? dat.y[dat.y.Count - 1] : 0;

	}

	public float Evaluate(float val) {

		if (val > dat.x[dat.x.Count - 1]) {
			return dat.y[dat.y.Count - 1];
		}
		

		for (int i = 0; i < dat.x.Count - 1; i++) {

			if (val > dat.x[i] && val < dat.x[i + 1]) {

				return dat.y[i] + (val - dat.x[i]) * (dat.y[i + 1] - dat.y[i]) / (dat.x[i + 1] - dat.x[i]);

			}

		}

		return 0;

	}

}
