using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FBDArrow : MonoBehaviour {

	public SpriteRenderer sprite;
	public Transform point;

	void Update() {
		UpdatePoint();
	}

	void OnDrawGizmos() {
		UpdatePoint();
	}

	void UpdatePoint() {

		if (sprite && point) {

			sprite.size = new Vector2(point.localPosition.magnitude, sprite.size.y);
			sprite.transform.localEulerAngles = 
				new Vector3(0, 0, Mathf.Atan2(point.localPosition.y, point.localPosition.x) * Mathf.Rad2Deg);

		}

	}

}
