using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
	float countdown = 60.0f;
	public float challenge_time = 180f;
	public bool inTargetRight = false;
	public bool inTargetLeft = false;
	public PoleTrigger poleTriggerLeft;
	public PoleTrigger poleTriggerRight;
	public SelectionTaskMeasure selectionTaskMeasure;
	public LocomotionTechnique locomotionTechnique;
	// Start is called before the first frame update
	void Start()
	{
		countdown = challenge_time;
	}

	// Update is called once per frame
	void Update()
	{

	}
	void FixedUpdate()
	{
		if (poleTriggerLeft.inTarget && poleTriggerRight.inTarget)
		{
			countdown -= 1.0f;
		}
		if (countdown <= 0.0f)
		{
			selectionTaskMeasure.EndOneTask();
			countdown = challenge_time;
			poleTriggerLeft.inTarget = false;
			poleTriggerRight.inTarget = false;
		}
	}
}
