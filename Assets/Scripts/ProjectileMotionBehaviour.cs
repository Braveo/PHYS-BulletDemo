using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ProjectileMotionBehaviour : MonoBehaviour {

	public ProjectileMotion pm = new ProjectileMotion();
	public float trajectoryFrequency = 60f;
	public float trajectoryTime = 5f;

	public FBDArrow drag;
	public FBDArrow gravity;
	public FBDArrow velocity;

	public Transform rendererPivot;
	public CinemachineVirtualCamera cam;

	void Awake() {

		InitializeGraphData();
		if (velocity) velocity.point.localPosition = Vector2.zero;
		if (drag) drag.point.localPosition = Vector2.zero;
		if (gravity) gravity.point.localPosition = Vector2.zero;

	}

	float startingVelocity = 1;
	float startingAngle = 0;

	void Update() {

		if (Input.GetMouseButtonDown(0) && !launching) {

			Debug.Log("GOOD QUESTION");
			LaunchBall();

		}

		if (!launching) {

			startingVelocity += Input.GetAxisRaw("Vertical") * Time.deltaTime * Time.timeScale;
			
			if (velocity) velocity.point.localPosition = 
					(Camera.main.ScreenToWorldPoint(Input.mousePosition) * Vector2.one
					- (Vector2)velocity.transform.position).normalized * startingVelocity * Time.timeScale;
			startingAngle = Mathf.Atan2(
				velocity.point.localPosition.y, velocity.point.localPosition.x)
				* Mathf.Rad2Deg;
			pm.initialVelocity = velocity.point.localPosition / Time.timeScale;


		}

		//Time.timeScale += Input.GetAxisRaw("Mouse ScrollWheel");
		if (Input.GetAxisRaw("Mouse ScrollWheel") > 0) {

			Time.timeScale *= 2f;

		}
		if (Input.GetAxisRaw("Mouse ScrollWheel") < 0) {

			Time.timeScale /= 2f;

		}

		if (Input.GetKeyDown(KeyCode.RightArrow)) {

			pm.projectile.radius *= 2f;
			rendererPivot.localScale = Vector3.one * pm.projectile.radius * 4;

		}
		if (Input.GetKeyDown(KeyCode.LeftArrow)) {

			pm.projectile.radius /= 2f;
			rendererPivot.localScale = Vector3.one * pm.projectile.radius * 4;

		}

		if (Input.GetKeyDown(KeyCode.Minus)) {

			if (cam) cam.m_Lens.OrthographicSize *= 1.5f;

		}
		if (Input.GetKeyDown(KeyCode.Plus)) {

			if (cam) cam.m_Lens.OrthographicSize /= 1.5f;

		}

	}
	string cText = "";
	void OnGUI() {

		cText = ((startingVelocity > 343) ? "WARNING: ABOVE SPEED OF SOUND\n\n" : "") +
			$"StartingVelocity: {startingVelocity}\n" +
			$"StartingAngle: {startingAngle}\n" + 
			$"TimeScale: {Time.timeScale}\n\n" +
			$"Time: {pm.time}\n" + 
			$"v: {pm.velocity.magnitude}\n" + 
			$"a(Drag): {(pm.acceleration - pm.gravity).magnitude}\n\n" +
			$"Re: {pm.reynoldNum.ToString("0.00000E0")}\n" +
			$"C: {pm.dragCoeff}\n\n";

		GUI.Label(
			new Rect(0, 0, 300f, 300f),
			cText,
			GUI.skin.textArea);
	}

	bool launching = false;
	bool frozen = false;
	public void LaunchBall() {

		pm.Start();
		StartCoroutine(EnLaunchBall());

	}
	IEnumerator EnLaunchBall() {

		launching = true;
		yield return new WaitForSecondsRealtime(0.02f);

		while (launching) {

			pm.Update(Time.deltaTime);

			if (Input.GetMouseButtonDown(1)) {

				frozen = true;
				yield return new WaitForSecondsRealtime(0.02f);

				while (true) {

					if (velocity) velocity.point.localPosition = pm.velocity * Time.timeScale;
					if (drag) drag.point.localPosition = (pm.acceleration - pm.gravity) * Time.timeScale;
					if (gravity) gravity.point.localPosition = pm.gravity * Time.timeScale;

					if (Input.GetMouseButtonDown(0)) {

						launching = false;
						frozen = false;
						break;

					}

					if (Input.GetMouseButtonDown(1)) {

						frozen = false;
						break;
						
					}

					yield return null;

				}

			}

			//if (Input.GetMouseButtonDown(0)) launching = false;
			//if (pm.position.y < 0) launching = false;
			yield return null;
			transform.position = pm.position;

			if (velocity) velocity.point.localPosition = pm.velocity * Time.timeScale;
			if (drag) drag.point.localPosition = (pm.acceleration - pm.gravity) * Time.timeScale;
			if (gravity) gravity.point.localPosition = pm.gravity * Time.timeScale;

		}

		if (velocity) velocity.point.localPosition = Vector2.zero;
		if (drag) drag.point.localPosition = Vector2.zero;
		if (gravity) gravity.point.localPosition = Vector2.zero;

	}

	[ContextMenu("INITIALIZE GRAPH DATA")]
	public void InitializeGraphData() {

		pm.graphData.Start();

	}

	public void OnDrawGizmos() {

		return;

		if (Application.isPlaying) return;
		if (!pm.graphData.Initialized) return;

		pm.Start();

		if (trajectoryFrequency > 144*4) return;
		for (float t = 0; t < trajectoryTime; t+=1f/trajectoryFrequency) {
			Gizmos.color = Color.red;
			
			Gizmos.DrawRay(pm.position, pm.velocity/trajectoryFrequency);
			Gizmos.color = Color.green;
			Gizmos.DrawRay(pm.position, pm.acceleration/trajectoryFrequency);
			pm.Update(1f/trajectoryFrequency);

		}

	}

}