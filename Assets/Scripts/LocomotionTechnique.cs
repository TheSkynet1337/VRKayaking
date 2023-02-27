using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
public class LocomotionTechnique : MonoBehaviour
{
	// Please implement your locomotion technique in this script. 
	public OVRInput.Controller leftController;
	public OVRInput.Controller rightController;
	[Range(0, 10)] public float translationGain = 0.5f;
	public GameObject hmd;
	[SerializeField] private float leftTriggerValue;
	[SerializeField] private float rightTriggerValue;
	[SerializeField] private Vector3 startPos;
	[SerializeField] private Vector3 offset;
	[SerializeField] private bool isIndexTriggerDown;
	[SerializeField] GameObject RightTriggerBox;
	[SerializeField] GameObject LeftTriggerBox;
	[SerializeField] GameObject Pole;

	public TMP_Text leftForceText;
	public TMP_Text rightForceText;

	/////////////////////////////////////////////////////////
	// These are for the game mechanism.
	public ParkourCounter parkourCounter;
	public string stage;
	public SelectionTaskMeasure selectionTaskMeasure;
	Rigidbody rigidbody;
	[SerializeField] float rotationalDrag = 0.8f;
	[SerializeField] float forwardDrag = 0.99f;
	[SerializeField] float sidewaysDrag = 0.7f;
	[SerializeField] float rotationClamp = 20f;
	[SerializeField] float forwardClamp = 50f;
	float leftForce = 0f;
	float rightForce = 0f;
	float dampen = 250.0f;
	float rotationDampening = 0.1f;
	Vector3 lastPosLocal;
	bool lastPosValid = false;
	public bool inTask = false;
	public float max_speed = 2.5f;
	public float max_rotation = 2f;

	void Start()
	{
		rigidbody = this.GetComponent<Rigidbody>();
	}
	private void FixedUpdate()
	{
		var localVelocity = transform.InverseTransformDirection(rigidbody.velocity);
		var newLocalVelocity = new Vector3(localVelocity.x * sidewaysDrag, 0, localVelocity.z * forwardDrag);
		rigidbody.velocity = transform.TransformDirection(newLocalVelocity);
		rigidbody.AddTorque(-rigidbody.angularVelocity * rotationalDrag);
		float forwardForceToApply = Mathf.Min((leftForce + rightForce) / 60, forwardClamp);
		float rotationForceToApply = Mathf.Min(((leftForce - rightForce) / 60) * rotationDampening, rotationClamp);
		if (localVelocity.z < max_speed && forwardForceToApply > 0)
		{
			rigidbody.AddRelativeForce(new Vector3(0, 0, forwardForceToApply));
		}
		else if (localVelocity.z > -max_speed && forwardForceToApply < 0)
		{
			rigidbody.AddRelativeForce(new Vector3(0, 0, forwardForceToApply));
		}
		rigidbody.AddRelativeTorque(new Vector3(0, rotationForceToApply, 0));
		rightForce = rightForce - rightForce / 60;
		leftForce = leftForce - leftForce / 60;
		if (Mathf.Abs(leftForce) < 0.5f)
		{
			leftForce = 0f;
		}
		if (Mathf.Abs(rightForce) < 0.5f)
		{
			rightForce = 0f;
		}
		leftForceText.text = "Left " + leftForce.ToString();
		rightForceText.text = "Right " + rightForce.ToString();
	}
	public void LeftRow(Vector3 pos)
	{
		if (!lastPosValid)
		{
			lastPosLocal = gameObject.transform.InverseTransformPoint(pos);
			lastPosValid = true;
		}
		else if (!inTask)
		{
			var posLocal = gameObject.transform.InverseTransformPoint(pos);
			var speed = -(posLocal - lastPosLocal).z;
			lastPosLocal = gameObject.transform.InverseTransformPoint(pos);
			leftForce += speed * dampen;
		}
	}
	public void RightRow(Vector3 pos)
	{
		if (!lastPosValid)
		{
			lastPosLocal = gameObject.transform.InverseTransformPoint(pos);
			lastPosValid = true;
		}
		else if (!inTask)
		{
			var posLocal = gameObject.transform.InverseTransformPoint(pos);
			var speed = -(posLocal - lastPosLocal).z;
			lastPosLocal = gameObject.transform.InverseTransformPoint(pos);
			rightForce += speed * dampen;
		}
	}
	void resetPosition()
	{
		this.transform.rotation = Quaternion.Euler(0, 0, 0);
	}
	public void RemovePosition()
	{
		lastPosValid = false;
	}
	void Update()
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////
		// Please implement your LOCOMOTION TECHNIQUE in this script :D.
		leftTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, leftController);
		rightTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, rightController);

		////////////////////////////////////////////////////////////////////////////////
		// These are for the game mechanism.
		if (OVRInput.Get(OVRInput.Button.Two) || OVRInput.Get(OVRInput.Button.Four))
		{
			resetPosition();
			// if (parkourCounter.parkourStart)
			// {
			//     this.transform.position = parkourCounter.currentRespawnPos;
			// }
		}
	}

	void OnTriggerEnter(Collider other)
	{

		// These are for the game mechanism.
		if (other.CompareTag("banner"))
		{
			stage = other.gameObject.name;
			parkourCounter.isStageChange = true;
		}
		else if (other.CompareTag("objectInteractionTask"))
		{
			inTask = true;
			rightForce = 0f;
			leftForce = 0f;
			rigidbody.velocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
			selectionTaskMeasure.isTaskStart = true;
			selectionTaskMeasure.scoreText.text = "";
			selectionTaskMeasure.partSumErr = 0f;
			selectionTaskMeasure.partSumTime = 0f;
			// rotation: facing the user's entering direction
			float tempValueY = other.transform.position.y > 0 ? 12 : 0;
			Vector3 tmpTarget = new Vector3(hmd.transform.position.x, tempValueY, hmd.transform.position.z);
			selectionTaskMeasure.taskUI.transform.LookAt(tmpTarget);
			selectionTaskMeasure.taskUI.transform.Rotate(new Vector3(0, 180f, 0));
			selectionTaskMeasure.isTaskStart = true;
			selectionTaskMeasure.StartOneTask();
		}
		else if (other.CompareTag("coin"))
		{
			parkourCounter.coinCount += 1;
			this.GetComponent<AudioSource>().Play();
			other.gameObject.SetActive(false);
		}
		// These are for the game mechanism.
	}
}