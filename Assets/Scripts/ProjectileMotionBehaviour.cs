using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMotionBehaviour : MonoBehaviour {

	public ProjectileMotion pm = new ProjectileMotion();
	public float trajectoryFrequency = 60f;
	public float trajectoryTime = 5f;

	public void OnDrawGizmos() {

		

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