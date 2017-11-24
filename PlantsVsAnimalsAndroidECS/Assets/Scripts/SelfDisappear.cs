using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelfDisappear : MonoBehaviour 
{
	[SerializeField] Text m_currentText;

	void Start() 
	{
		StartCoroutine("TextDisappearRoutine");
	}
	
	IEnumerator TextDisappearRoutine()
	{
		m_currentText = GetComponent<Text>();
		yield return new WaitForSeconds(2.5f);
		m_currentText.enabled = false;
		StartCoroutine("TextDisappearRoutine");
	}
}
