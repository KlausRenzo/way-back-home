using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beacon : MonoBehaviour
{
	[SerializeField] Transform directionToPoint;
	[SerializeField] private Material[] materials;
	[SerializeField] private bool active;
	[SerializeField] private AnimationCurve lightAnimationCurve;
	[SerializeField] private float lightIntensity = 1;
	private new Light light;

	// Start is called before the first frame update
	void Start()
	{
		light = GetComponentInChildren<Light>();
	}

	// Update is called once per frame
	void Update()
	{
	}


	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("ENTER");
		var roverInteraction = other.GetComponent<RoverInteraction>();
		if (!roverInteraction)
			return;

		StopCoroutine(nameof(FadeLigthTo));
		StartCoroutine(FadeLigthTo(true));
		roverInteraction.EnableBeaconActivation(this);
	}

	private void OnTriggerExit(Collider other)
	{
		Debug.Log("EXIT");
		var roverInteraction = other.GetComponent<RoverInteraction>();
		if (!roverInteraction)
			return;

		StopCoroutine(nameof(FadeLigthTo));
		StartCoroutine(FadeLigthTo(false));

		roverInteraction.DisableBeaconActivation(this);
	}

	private IEnumerator FadeLigthTo(bool b)
	{
		float time = Time.time;

		float fadeDuration = 2f;

		while (Time.time - time < fadeDuration)
		{
			if (b)
			{
				var t = (Time.time - time) / fadeDuration;
				light.intensity = lightAnimationCurve.Evaluate(t) * lightIntensity;
			}
			else
			{
				var t = (Time.time - time) / fadeDuration;
				light.intensity = lightAnimationCurve.Evaluate(1 - t) * lightIntensity;
			}

			yield return new WaitForEndOfFrame();
		}
	}

	public bool Activate()
	{
		if (active)
			return false;

		active = true;
		return true;
	}
}