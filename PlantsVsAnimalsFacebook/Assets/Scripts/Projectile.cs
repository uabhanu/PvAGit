using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	PlayerButton m_playerButton;
	Vector2 m_positionOnScreen;

	[SerializeField] AudioClip m_projectileSound;

	[Range(0.0f , 100.0f)] [SerializeField] float m_flySpeed;

	[Range(0.0f , 1.0f)] [SerializeField] float m_volume;

    [SerializeField] GameObject m_currentTarget;

    [Range(0 , 1000)] [SerializeField] int m_attack;

	void Update()
    {
		if(Time.timeScale == 0)
        {
            return;
        }

        transform.Translate(Vector2.right * m_flySpeed * Time.deltaTime);

        if(transform.position.x > 11.05f)
        {
            Destroy(gameObject);
        }
	}

    void CauseDamage()
    {
        if(m_currentTarget != null)
        {
			AudioSource.PlayClipAtPoint(m_projectileSound , transform.position , m_volume);
			m_currentTarget.gameObject.GetComponent<BhanuEnemy>().m_hitpoints -= m_attack;
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("Sir Bhanu, there is no Defender target anymore to cause damage to");
        }
    }

	void OnMouseDown()
	{
		LevelManager.m_notEnoughStarsText.enabled = false;

		m_playerButton = FindObjectOfType<PlayerButton>();
		m_playerButton.ResetSelection();
	}

    void OnTriggerEnter2D(Collider2D tri2D)
    {
        if(tri2D.gameObject.tag.Equals("Enemy"))
        {
            Debug.Log("Corgette Collision with Enemy Successful"); //Working Fine
            m_currentTarget = tri2D.gameObject;
            CauseDamage();
        }
    }
}
