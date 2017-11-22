using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    float m_fallSpeed = 1.5f , m_destination = 0.02f;

    [SerializeField] PlayerButton m_playerButton;

    [SerializeField] StarsCurrency m_starsCurrency;

    void Start()
    {
        m_playerButton = FindObjectOfType<PlayerButton>();
        m_starsCurrency = FindObjectOfType<StarsCurrency>();
        StartCoroutine("SelfDestructRoutine");    
    }

    void Update()
    {
        if(Time.timeScale == 0)
        {
            return;
        }

        if(transform.localScale.x <= 1 && transform.localScale.y <= 1)
        {
            transform.localScale = new Vector2(transform.localScale.x + 0.01f , transform.localScale.y + 0.01f);
        }

        if(transform.localScale.y >= 1 && transform.position.y > m_destination)
        {
            transform.Translate(-Vector2.up * m_fallSpeed * Time.deltaTime);
        }
    }

    IEnumerator SelfDestructRoutine()
    {
        yield return new WaitForSeconds(2f);

        if(transform.position.y < m_destination)
        {
            Debug.Log(transform.position);
            Destroy(gameObject);
        }
     
        StartCoroutine("SelfDestructRoutine");
    }

    void OnMouseDown()
    {
		if(transform.localScale.x >= 1)
		{
			LevelManager.m_notEnoughStarsText.enabled = false;
			m_playerButton.ResetSelection();
			m_starsCurrency.m_starsCount += 10;
			Destroy(gameObject);	
		}
    }
}
