using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BhanuEnemy : MonoBehaviour
{
    LevelManager m_levelManager;

	[SerializeField] GameObject m_explosionPSPrefab;

    public float m_seenEverySecs;
    [Range(0 , 1000)] public int m_hitpoints;

	void Start()
    {
		m_levelManager = FindObjectOfType<LevelManager>();
        StartCoroutine("DieRoutine");
	}

    IEnumerator DieRoutine()
    {
        yield return new WaitForSeconds(0.15f);

        if(m_hitpoints <= 0)
        {
            m_levelManager.m_totalEnemiesKilled++;
			GameObject explosion = Instantiate(m_explosionPSPrefab , transform.position , transform.rotation) as GameObject;
			ParticleSystem redExplosion = explosion.GetComponent<ParticleSystem>();
			redExplosion.Play();
            Destroy(gameObject);
        }

        if(transform.position.x < 0.4f)
        {
			if(m_levelManager.m_currentSceneIndex == 2) 
			{
				m_levelManager.Lose ();
			} 
			else 
			{
				m_levelManager.UnityAds();	
			}
        }

        StartCoroutine("DieRoutine");
    }

    void OnMouseDown()
    {
		LevelManager.Disable(LevelManager.m_notEnoughStarsText);
    }
}
