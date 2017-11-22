using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gnome : MonoBehaviour
{
    public enum GnomeState
	{
        ATTACK,
		IDLE,
	};
	
	public GnomeState m_currentState;
	public GnomeState m_previousState;

	Animator m_animator;
    BhanuPlayer m_bhanuPlayer;
    GameObject m_projectilesParent;

	[SerializeField] GameObject m_axeObj , m_axePrefab;

    [SerializeField] Transform m_axeSpawnPosition;

	void Start()
    {
	    m_animator = GetComponent<Animator>();
        m_bhanuPlayer = GetComponent<BhanuPlayer>();
        m_projectilesParent = GameObject.Find("Projectiles");

        if(m_projectilesParent == null)
        {
            m_projectilesParent = new GameObject("Projectiles");
        }
	}

    void Update()
    {
		if(Time.timeScale == 0)
        {
            return;
        }

        if(m_bhanuPlayer.m_enemyInSight)
        {
            SetState(GnomeState.ATTACK);
        }
        else
        {
            SetState(GnomeState.IDLE);
        }

        UpdateAnimations();
        UpdateStateMachine();
	}

    void Attack()
    {
		if(m_axeObj == null)
		{
			m_axeObj = Instantiate(m_axePrefab , m_axeSpawnPosition) as GameObject;	

			if(m_projectilesParent != null)
			{
				m_axeObj.transform.parent = m_projectilesParent.transform;
			}
		}
    }

    GnomeState GetState()
	{
		return m_currentState;
	}

    public void SetState(GnomeState newState)
	{
		if (m_currentState == newState)
		{
			return;
		}
		
		m_previousState = m_currentState;
		m_currentState = newState;
	}

    void UpdateAnimations()
	{
		switch(m_currentState)
		{
            case GnomeState.ATTACK:
                m_animator.SetBool("Hop" , true);
            break;

			case GnomeState.IDLE:
                m_animator.SetBool("Hop" , false);
            break;
		}
	}

    void UpdateStateMachine()
	{
		switch(m_currentState)
		{
			case GnomeState.ATTACK:
				
			break;
			
			case GnomeState.IDLE: 
				
			break;
		}
	}
}
