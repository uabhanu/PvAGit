using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravestone : MonoBehaviour
{
    public enum GravestoneState
	{
        ATTACKED,
		IDLE,
	};

    [HideInInspector] [Range(0.0f , 10.0f)] [SerializeField] float m_lineDistance , m_lineStartingPoint;

    public GravestoneState m_currentState;
	public GravestoneState m_previousState;

    Animator m_animator;

	void Start()
    {
	    m_animator = GetComponent<Animator>();	
	}

    void Update()
    {
		if(Time.timeScale == 0)
        {
            return;
        }

        UpdateAnimations();
        UpdateStateMachine();
	}
	
	void Attacked()
    {
        m_animator.SetBool("Attacked" , true);
    }

    GravestoneState GetState()
	{
		return m_currentState;
	}

    void Idle()
    {
        m_animator.SetBool("Attacked" , false);
    }

    public void SetState(GravestoneState newState)
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
            case GravestoneState.ATTACKED:
                m_animator.SetBool("Attacked" , true);
            break;

			case GravestoneState.IDLE:
                m_animator.SetBool("Attacked" , false);
            break;
		}
	}

    void UpdateStateMachine()
	{
		switch(m_currentState)
		{
			case GravestoneState.ATTACKED:
				Attacked();
			break;
			
			case GravestoneState.IDLE: 
				Idle();
			break;
		}
	}
}
