using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lizard : MonoBehaviour
{
    public enum LizardState
	{
        APPEAR,
        ATTACK,
		WALK,
	};
	
	public LizardState m_currentState;
	public LizardState m_previousState;

    Animator m_animator;
    float m_currentWalkSpeed;
    Rigidbody2D m_lizardBody2D;

    [Range(0.0f , 2.5f)] [SerializeField] float m_walkSpeed;

    [SerializeField] GameObject m_currentTarget;

    [Range(20 , 100)] [SerializeField] int m_attack;

	void Start()
    {
	    m_animator = GetComponent<Animator>();
        m_currentWalkSpeed = m_walkSpeed;
        m_lizardBody2D = GetComponent<Rigidbody2D>();
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

    IEnumerator AppearRoutine()
    {
        yield return new WaitForSeconds(1f);
        SetState(LizardState.WALK);
    }
	
	void Attack()
    {
        m_walkSpeed = 0f;
        m_lizardBody2D.velocity = new Vector2(-m_walkSpeed , m_lizardBody2D.velocity.y);

        if(m_currentTarget == null)
        {
            SetState(LizardState.WALK);
        }   
    }

    void CauseDamage()
    {
        if(m_currentTarget != null)
        {
            m_currentTarget.gameObject.GetComponent<BhanuPlayer>().m_hitpoints -= m_attack;
        }
        else
        {
            Debug.LogError("Sir Bhanu, there is no Defender target anymore to cause damage to");
        }
    }

    LizardState GetState()
	{
		return m_currentState;
	}

    void OnTriggerEnter2D(Collider2D tri2D)
    {
        if(tri2D.gameObject.tag.Equals("Player"))
        {
            m_currentTarget = tri2D.gameObject;

            if(m_currentTarget != null && transform.position.x > m_currentTarget.transform.position.x)
            {
                SetState(LizardState.ATTACK);
            }
        }
    }

    public void SetState(LizardState newState)
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
            case LizardState.APPEAR:
                m_animator.SetBool("Appear" , true);
            break;

            case LizardState.ATTACK:
                m_animator.SetBool("Attack" , true);
                m_animator.SetBool("Walk" , false);
            break;

			case LizardState.WALK:
                m_animator.SetBool("Attack" , false);
                m_animator.SetBool("Walk" , true);
            break;
		}
	}

    void UpdateStateMachine()
	{
		switch(m_currentState)
		{
            case LizardState.APPEAR:
				StartCoroutine("AppearRoutine");
			break;

			case LizardState.ATTACK:
				Attack();
			break;
			
			case LizardState.WALK: 
				Walk();
			break;
		}
	}

    void Walk()
    {
        m_walkSpeed = m_currentWalkSpeed;        
		m_lizardBody2D.velocity = new Vector2(-m_walkSpeed , m_lizardBody2D.velocity.y);
    }
}
