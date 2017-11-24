using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour 
{
	void Start () 
	{
		Invoke("Suicide" , 2f);
	}
	
	void Suicide()
	{
		Destroy(gameObject);
	}
}
