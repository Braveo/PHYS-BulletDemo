using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using System.Text;
using System.IO;
using System;

public class ProjectileMotionBehaviour : MonoBehaviour {

	public ProjectileMotion pm = new ProjectileMotion();
	[Space(10)]
	public float trajectoryFrequency = 60f;
	public float trajectoryTime = 5f;
	[Space(10)]
	public FBDArrow drag;
	public FBDArrow gravity;
	public FBDArrow velocity;
	[Space(10)]
	public Transform rendererPivot;
	public CinemachineVirtualCamera cam;
	[Space(10)]
	public TMP_Text textTopLeft;
	public TMP_Text textbottomLeft;
	[Space(10)]
	public GameObject tabUI;
	[Space(10)]
	public LineRenderer lineRender;
	public float maxDuration = 10;

	void Awake() {

		InitializeGraphData();
		if (velocity) velocity.point.localPosition = Vector2.zero;
		if (drag) drag.point.localPosition = Vector2.zero;
		if (gravity) gravity.point.localPosition = Vector2.zero;

	}

	float startingVelocity = 1;
	float startingAngle = 0;

	void Update() {

		if (!launching && !tabbedOut) {

			if (lineRender) {

				lineRender.positionCount = Mathf.CeilToInt(trajectoryTime * trajectoryFrequency);

				pm.Start();
				for (float i = 0; i < trajectoryTime; i += 1 / trajectoryFrequency) {

					lineRender.SetPosition(Mathf.FloorToInt(i * trajectoryFrequency), pm.position);
					pm.Update(1 / trajectoryFrequency);

				}

			}

			

			if (Input.GetMouseButtonDown(0)) {

				LaunchBall();

			}

			startingVelocity += Input.GetAxisRaw("Vertical") * Time.deltaTime * Time.timeScale;

			if (Input.GetAxisRaw("Vertical") == 0) startingVelocity = Mathf.Round(startingVelocity / 0.05f) * 0.05f;
			
			if (velocity) velocity.point.localPosition = 
					(Camera.main.ScreenToWorldPoint(Input.mousePosition) * Vector2.one
					- (Vector2)velocity.transform.position).normalized * startingVelocity * Time.timeScale;
			startingAngle = Mathf.Atan2(
				velocity.point.localPosition.y, velocity.point.localPosition.x)
				* Mathf.Rad2Deg;
			startingAngle = Mathf.Round(startingAngle / 5f) * 5f;

			if (velocity) velocity.point.localPosition =
					new Vector2(
						Mathf.Cos(startingAngle * Mathf.Deg2Rad),
						Mathf.Sin(startingAngle * Mathf.Deg2Rad)
					) * startingVelocity * Time.timeScale;

			pm.initialVelocity = velocity.point.localPosition / Time.timeScale;


		}

		UpdateInputs();
		UpdateText();

	}

	string stringTL = "";
	string stringTB = "";

	void UpdateText() {

		stringTL = ((startingVelocity > 343) ? "WARNING: ABOVE SPEED OF SOUND\n\n" : "") +
			$"StartingVelocity: {startingVelocity}\n" +
			$"StartingAngle: {startingAngle}\n" +
			$"TimeScale: {Time.timeScale}\n\n" +
			$"Time: {pm.time}\n" +
			$"p: {pm.position}\n" + 
			$"v: {pm.velocity} , {pm.velocity.magnitude}\n" +
			$"a(Drag): {pm.acceleration - pm.gravity} , {(pm.acceleration - pm.gravity).magnitude}\n\n" +
			$"Re: {pm.reynoldNum.ToString("0.00000E0")}\n" +
			$"C: {pm.dragCoeff}\n";
			
		stringTB =
			$"FluidViscocity: {pm.fluid.viscocity.ToString("0.00000E0")}\n" +
			$"FluidDensity: {pm.fluid.density}\n\n" +
			$"ProjectileRadius: {pm.projectile.radius}\n" +
			$"ProjectileArea: {pm.projectile.Area}\n" +
			$"ProjectileMass: {pm.projectile.mass}\n" +
			$"ProjectileGravity: {pm.gravity}";


		textTopLeft.text = stringTL;
		textbottomLeft.text = stringTB;

	}

	//void OnGUI() {

	//	cText = ((startingVelocity > 343) ? "WARNING: ABOVE SPEED OF SOUND\n\n" : "") +
	//		$"StartingVelocity: {startingVelocity}\n" +
	//		$"StartingAngle: {startingAngle}\n" + 
	//		$"TimeScale: {Time.timeScale}\n\n" +
	//		$"Time: {pm.time}\n" + 
	//		$"v: {pm.velocity.magnitude}\n" + 
	//		$"a(Drag): {(pm.acceleration - pm.gravity).magnitude}\n\n" +
	//		$"Re: {pm.reynoldNum.ToString("0.00000E0")}\n" +
	//		$"C: {pm.dragCoeff}\n\n";

	//	GUI.Label(
	//		new Rect(0, 0, 300f, 300f),
	//		cText,
	//		GUI.skin.textArea);
	//}

	void UpdateInputs() {

		if (Input.GetKeyDown(KeyCode.Tab)) {

			tabbedOut = !tabbedOut;
			tabUI.SetActive(!tabUI.activeInHierarchy);

		}

		if (tabbedOut) return;

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
		if (Input.GetKeyDown(KeyCode.Equals)) {

			if (cam) cam.m_Lens.OrthographicSize /= 1.5f;

		}

	}

	bool launching = false;
	bool frozen = false;
	bool tabbedOut = false;
	public void LaunchBall() {

		pm.Start();
		StartCoroutine(EnLaunchBall());

	}

	List<float> timePlot = new List<float>();
	List<Vector2> posPlot = new List<Vector2>();
	List<Vector2> velPlot = new List<Vector2>();
	List<Vector2> accPlot = new List<Vector2>();

	List<float> reyPlot = new List<float>();
	List<float> coeffPlot = new List<float>();

	void ExportCSV() {

		StringBuilder csv = new StringBuilder();
		csv.Append("Time," +
			"PosX,PosY," +
			"VelX,VelY,VelM," +
			"AccX,AccY,AccM," +
			"DragX,DragY,DragM," +
			"Reynold,DragCoeff\n");

		for (int i = 0; i < timePlot.Count; i++) {

			csv.Append($"{timePlot[i]}," +
				$"{posPlot[i].x},{posPlot[i].y}," +
				$"{velPlot[i].x},{velPlot[i].y},{velPlot[i].magnitude}," +
				$"{accPlot[i].x},{accPlot[i].y},{accPlot[i].magnitude}," +
				$"{accPlot[i].x-pm.gravity.x},{accPlot[i].y-pm.gravity.y},{(accPlot[i]-(Vector2)pm.gravity).magnitude}," +
				$"{reyPlot[i]},{coeffPlot[i]}\n"
				);

		}

		string path = Application.streamingAssetsPath + "/Recordings/";
		if (!File.Exists(path)) {

			Directory.CreateDirectory(path);

		}

		
		string dateTime = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}_" +
			$"{DateTime.Now.Hour}-{DateTime.Now.Minute}-{DateTime.Now.Second}";
		File.WriteAllText(path + $"{dateTime}.csv", csv.ToString());
		csv.Clear();

		timePlot.Clear();
		posPlot.Clear();
		velPlot.Clear();
		accPlot.Clear();
		reyPlot.Clear();
		coeffPlot.Clear();

	}

	IEnumerator EnLaunchBall() {

		launching = true;
		yield return new WaitForSecondsRealtime(0.02f);

		int tick = 0;

		while (launching && pm.time < maxDuration) {

			if (!tabbedOut) {

				tick += 1;
				if (tick > 3) {

					tick = 0;

					timePlot.Add(pm.time);
					posPlot.Add(pm.position);
					velPlot.Add(pm.velocity);
					accPlot.Add(pm.acceleration);
					reyPlot.Add(pm.reynoldNum);
					coeffPlot.Add(pm.dragCoeff);

				}
				pm.Update(Time.deltaTime);

			}

			if (!tabbedOut && Input.GetMouseButtonDown(1)) {

				frozen = true;
				yield return new WaitForSecondsRealtime(0.02f);

				while (true) {

					if (velocity) velocity.point.localPosition = pm.velocity * Time.timeScale;
					if (drag) drag.point.localPosition = (pm.acceleration - pm.gravity) * Time.timeScale;
					if (gravity) gravity.point.localPosition = pm.gravity * Time.timeScale;

					if (Input.GetKeyDown(KeyCode.Semicolon)) ExportCSV();

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

		ExportCSV();
		launching = false;
		frozen = false;
		if (velocity) velocity.point.localPosition = Vector2.zero;
		if (drag) drag.point.localPosition = Vector2.zero;
		if (gravity) gravity.point.localPosition = Vector2.zero;
		transform.position = Vector3.zero;

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