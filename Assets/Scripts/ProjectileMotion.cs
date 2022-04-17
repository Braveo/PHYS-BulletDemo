using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileMotion {

	public ProjectileData projectile = new ProjectileData();
	public FluidData fluid = new FluidData();
	public GraphData graphData = new GraphData();

	public Vector3 initialVelocity = new Vector3(10, 10, 0);
	public Vector3 gravity = new Vector3(0, -9.81f, 0);

	public float time { get; private set; }

	public Vector3 position { get; private set; }
	public Vector3 velocity { get; private set; }
	public Vector3 acceleration { get; private set; }

	public float reynoldNum { get; private set; }
	public float dragCoeff { get; private set; }

	public void Start() {

		time = 0;
		position = Vector3.zero;
		velocity = initialVelocity;

	}

	public void Update(float delta) {

		time += delta;

		reynoldNum = (float)(fluid.density * velocity.magnitude * 2 * projectile.radius / fluid.viscocity);
		dragCoeff = graphData.Evaluate(reynoldNum);

		acceleration = gravity + GetDragAcceleration(dragCoeff);
		velocity += acceleration * delta;
		position += velocity * delta;

	}

	Vector3 GetDragAcceleration(float dragCoefficient) {
		float velMagSqr = velocity.sqrMagnitude;
		return -velocity.normalized * 
			(float)((dragCoefficient * fluid.density * projectile.Area * velMagSqr) / (2f * projectile.mass));
	}
}

[System.Serializable]
public class ProjectileData {

	public float radius = 0.25f;						//Measured in meters
	public float Area => Mathf.PI * radius * radius;	//Area as function of radius

	public float mass = 5;								//Measured in kilometers

}

[System.Serializable]
public class FluidData {

	public double density = 1.225f;     //Density of air at 15 deg C, measured in kg/m^3
	public double viscocity = 1.81e-5;  //Viscocity of air at 15 deg C, measured in kg/(m*s)

}

