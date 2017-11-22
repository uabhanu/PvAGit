using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    int m_level;

    public AudioClip[] m_soundsArray;
	public static AudioSource m_audioSource;
	public static bool m_mute;

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
