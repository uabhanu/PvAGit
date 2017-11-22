using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    AudioSource m_audioSource;
    int m_level;

    [SerializeField] AudioClip[] m_soundsArray;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);   
    }

    void Start()
    {
        m_level = SceneManager.sceneCount;
        AudioClip currentLevelMusic = m_soundsArray[m_level];
        m_audioSource = GetComponent<AudioSource>();
        m_audioSource.clip = currentLevelMusic;
        m_audioSource.loop = true;
        m_audioSource.Play();
	}
}
