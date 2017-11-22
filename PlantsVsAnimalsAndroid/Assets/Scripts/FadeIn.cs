using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    [SerializeField] float m_fadeInTime;

    Color m_currentColour = Color.black;
    Image m_fadeInImage;

	void Start()
    {
		m_fadeInImage = GetComponent<Image>();
	}
	
	void Update()
    {
        if(Time.timeScale == 0)
        {
            return;
        }

        if(Time.timeSinceLevelLoad < m_fadeInTime)
        {
            float alphaChange = Time.deltaTime / m_fadeInTime;
            m_currentColour.a -= alphaChange;
            m_fadeInImage.color = m_currentColour;
        } 
        else
        {
            gameObject.SetActive(false);
        }
    }
}
