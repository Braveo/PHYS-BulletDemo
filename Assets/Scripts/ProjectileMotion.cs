using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileMotion {

	public ProjectileData projectile = new ProjectileData();
	public FluidData fluid = new FluidData();

	public Vector3 initialVelocity = new Vector3(10, 10, 0);
	public Vector3 gravity = new Vector3(0, -9.81f, 0);

	public float heightOffset = 2f;
	public float testCoefficient = 1;

	public Vector3 position { get; private set; }
	public Vector3 velocity { get; private set; }
	public Vector3 acceleration { get; private set; }

	public void Start() {

		position = Vector3.zero + Vector3.up * heightOffset;
		velocity = initialVelocity;

	}

	public void Update(float delta) {

		if (delta < 1f / (144f * 4f)) return;
		
		position += velocity * delta;

		acceleration = gravity + GetDragAcceleration(testCoefficient);
		velocity += acceleration * delta;

	}

	Vector3 GetDragAcceleration(float dragCoefficient) {
		float velMagSqr = velocity.sqrMagnitude;
		return -velocity.normalized * (float)((dragCoefficient * fluid.density * projectile.Area * velMagSqr) / (2f * projectile.mass));
	}
}

[System.Serializable]
public class ProjectileData {

	public float radius = 0.25f;
	public float Area => Mathf.PI * radius * radius;

	public float mass = 5;

}

[System.Serializable]
public class FluidData {

	public double density = 1.225f;
	public double viscocity = 1.81e-5;

}