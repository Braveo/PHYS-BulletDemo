using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class UIParameterSection : MonoBehaviour {

	ProjectileMotionBehaviour proj;

	public TMP_InputField fViscosity;
	public TMP_InputField fDensity;
	public TMP_InputField pMass;
	public TMP_InputField pGravityX;
	public TMP_InputField pGravityY;
	public TMP_InputField pGravityZ;
	public TMP_InputField duration;

	void Awake() {

		proj = FindObjectOfType<ProjectileMotionBehaviour>();
		fViscosity.onSubmit.AddListener(ctx => Send(proj));
		fDensity.onSubmit.AddListener(ctx => Send(proj));
		pMass.onSubmit.AddListener(ctx => Send(proj));
		pGravityX.onSubmit.AddListener(ctx => Send(proj));
		pGravityY.onSubmit.AddListener(ctx => Send(proj));
		pGravityZ.onSubmit.AddListener(ctx => Send(proj));
		duration.onSubmit.AddListener(ctx => Send(proj));

		Receive(proj);

	}

	public void Receive(ProjectileMotionBehaviour pmb) {

		fViscosity.text = pmb.pm.fluid.viscocity.ToString();
		fDensity.text = pmb.pm.fluid.density.ToString();
		pMass.text = pmb.pm.projectile.mass.ToString();
		pGravityX.text = pmb.pm.gravity.x.ToString();
		pGravityY.text = pmb.pm.gravity.y.ToString();
		pGravityZ.text = pmb.pm.gravity.z.ToString();
		duration.text = pmb.maxDuration.ToString();

	}

	public void Send(ProjectileMotionBehaviour pmb) {

		pmb.pm.fluid.viscocity = double.Parse(fViscosity.text);
		pmb.pm.fluid.density = double.Parse(fDensity.text);
		pmb.pm.projectile.mass = float.Parse(pMass.text);
		pmb.pm.gravity.x = float.Parse(pGravityX.text);
		pmb.pm.gravity.y = float.Parse(pGravityY.text);
		pmb.pm.gravity.z = float.Parse(pGravityZ.text);
		pmb.maxDuration = float.Parse(duration.text);

	}

}
