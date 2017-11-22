using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BhanuPlayer : MonoBehaviour
{
    EnemySpawner m_myLaneSpawner;

	[SerializeField] float m_volume;

	[SerializeField] GameObject m_explosionPSPrefab;

    public bool m_enemyInSight;
    [Range(0 , 1000)] public int m_hitpoints;
    [Range(0 , 1000)] public int m_playerCost;

	void Start()
    {
        StartCoroutine("DieRoutine");

        LaneSpawInfo();
        print(m_myLaneSpawner);
	}

    void FixedUpdate()
    {
        if(Time.timeScale == 0)
        {
            return;
        }

        if(EnemyInSight())
        {
            m_enemyInSight = true;
        }
        else
        {
            m_enemyInSight = false;
        }
    }

    IEnumerator DieRoutine()
    {
        yield return new WaitForSeconds(1f);

        if(m_hitpoints <= 0)
        {
			GameObject explosion = Instantiate(m_explosionPSPrefab , transform.position , transform.rotation) as GameObject;
			ParticleSystem blueExplosion = explosion.GetComponent<ParticleSystem>();
			blueExplosion.Play();
            Destroy(gameObject);
        }

        StartCoroutine("DieRoutine");
    }

    bool EnemyInSight()
    {
		if(m_myLaneSpawner != null) 
		{
			if(m_myLaneSpawner.transform.childCount <= 0)
			{
				m_enemyInSight = false;
				return m_enemyInSight;	
			}

			foreach(Transform bhanuEnemy in m_myLaneSpawner.transform)  // Unity Docs indicates that Transforms also support enumeratiors so we can actually loop through children
			{
				if(bhanuEnemy.transform.position.x > transform.position.x && bhanuEnemy.transform.position.x < 11.05f)
				{ 
					m_enemyInSight = true;
					return m_enemyInSight;
				}
			}
		}  
		else 
		{
			Debug.LogError(name + " : Enemy not in sight yet");
		}
			
        m_enemyInSight = false;
        return m_enemyInSight;
    }

    void LaneSpawInfo()
    {
        EnemySpawner[] m_enemySpawners = FindObjectsOfType<EnemySpawner>();

        foreach(EnemySpawner m_enemySpawner in m_enemySpawners)
        {
            if(m_enemySpawner.transform.position.y == transform.position.y)
            {
                m_myLaneSpawner = m_enemySpawner;
                return;
            }
            else
            {
				Debug.LogError(name + " : Sir Bhanu, Can't See any Enemy Spawner Yet");
            }
        }
    }

    void OnMouseDown()
    {
		LevelManager.m_notEnoughStarsText.enabled = false;    
    }
}
