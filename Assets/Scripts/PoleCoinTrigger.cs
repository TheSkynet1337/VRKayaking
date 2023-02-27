using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleCoinTrigger : MonoBehaviour
{
	[SerializeField] ParkourCounter parkourCounter;
	[SerializeField] GameObject Rig;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("coin"))
		{
			print("coin");
			parkourCounter.coinCount += 1;
			Rig.GetComponent<AudioSource>().Play();
			other.gameObject.SetActive(false);
		}
	}
}
