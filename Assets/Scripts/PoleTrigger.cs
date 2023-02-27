using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleTrigger : MonoBehaviour
{
	[SerializeField] GameObject Rig;
	Rigidbody rigidbody;
	[SerializeField] LocomotionTechnique locomotionTechnique;
	[SerializeField] ParkourCounter parkourCounter;
	float countdown = 60.0f;
	public bool inTarget = false;
	// Start is called before the first frame update
	void Start()
	{
		rigidbody = Rig.GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{

	}
	void OnTriggerEnter(Collider other)
	{

		if (other.CompareTag("RightEndTarget"))
		{
			inTarget = true;
		}
		if (other.CompareTag("LeftEndTarget"))
		{
			inTarget = true;
		}
		if (other.CompareTag("coin"))
		{
			print("coin");
			parkourCounter.coinCount += 1;
			Rig.GetComponent<AudioSource>().Play();
			other.gameObject.SetActive(false);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("RightHitbox"))
		{
			locomotionTechnique.RemovePosition();

		}
		if (other.CompareTag("LeftHitbox"))
		{
			locomotionTechnique.RemovePosition();
		}
		if (other.CompareTag("RightEndTarget"))
		{
			inTarget = false;
		}
		if (other.CompareTag("LeftEndTarget"))
		{
			inTarget = false;
		}
	}
	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("RightHitbox"))
		{
			locomotionTechnique.RightRow(gameObject.transform.position);
		}
		if (other.CompareTag("LeftHitbox"))
		{
			locomotionTechnique.LeftRow(gameObject.transform.position);
		}
	}
}
