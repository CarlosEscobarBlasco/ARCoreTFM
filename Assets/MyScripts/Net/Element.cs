using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Element : MonoBehaviour
{
	
	public int Index;
	public string Type;
	public float PositionX;
	public float PositionY;
	public float PositionZ;
	public float RotationX;
	public float RotationY;
	public float RotationZ;
	public float RotationW;
	
	
	// Update is called once per frame
	void Update ()
	{
		PositionX = transform.position.x;
		PositionY = transform.position.y;
		PositionZ = transform.position.z;

		RotationX = transform.rotation.x;
		RotationY = transform.rotation.y;
		RotationZ = transform.rotation.z;
		RotationW = transform.rotation.w;
	}

	public string ToJson()
	{
		return JsonUtility.ToJson(this);
	}

}
