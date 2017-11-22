using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsCurrency : MonoBehaviour
{
    [SerializeField] Text m_starScoreLabel;

    public enum Status {SUCCESS , FAILURE};

    public int m_starsCount;

    void Start()
    {
        m_starScoreLabel = GetComponent<Text>();
    }

    void Update()
    {
        if(Time.timeScale == 0)
        {
            return;
        }

        m_starScoreLabel.text = m_starsCount.ToString();
    }

    public Status UseStars(int amount)
    {
        if(m_starsCount >= amount)
        {
            m_starsCount -= amount;
            return Status.SUCCESS;
        }
        
        return Status.FAILURE;
    }
}
