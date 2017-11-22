using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviour
{
    GameObject m_playerParent;
	LevelManager m_levelManager;
    StarsCurrency m_starsCurrency;

    [SerializeField] Camera m_myCamera;

    [SerializeField] PlayerButton m_playerButton;

	void Start()
    {
		m_levelManager = FindObjectOfType<LevelManager>();
		m_playerButton = FindObjectOfType<PlayerButton>();
		m_playerParent = GameObject.Find("PlayerParent");
        m_starsCurrency = FindObjectOfType<StarsCurrency>();

        if(m_playerParent == null)
        {
            m_playerParent = new GameObject("PlayerParent");
        }
	}
	
	void OnMouseDown()
    {
        GameObject playerToSpawn = PlayerButton.m_playerToSpawn;

        if(playerToSpawn != null)
        {
            int playerCost = playerToSpawn.GetComponent<BhanuPlayer>().m_playerCost;

            Vector2 rawWorldPos = WorldPointOfMouseClick();
            Vector2 roundedPos = SnapToGrid(rawWorldPos);

            if(m_starsCurrency.UseStars(playerCost) == StarsCurrency.Status.SUCCESS)
            {
                SpawnPlayer(roundedPos , playerToSpawn);
            }
            else
            {
                Debug.LogError("Sir Bhanu, You can't afford this player");
				LevelManager.Enable(LevelManager.m_notEnoughStarsText);
                m_playerButton.ResetSelection();
            }
        }
        else
        {
            Debug.LogError("Sir Bhanu, You haven't selected any player yet");
			LevelManager.Enable(LevelManager.m_playerSelectText);
        }
	}

    Vector2 SnapToGrid(Vector2 rawWorldPos)
    {
        float newX = Mathf.RoundToInt(WorldPointOfMouseClick().x);
        float newY = Mathf.RoundToInt(WorldPointOfMouseClick().y);
        return new Vector2(newX , newY);
    }

    void SpawnPlayer(Vector2 roundedPos , GameObject playerToSpawn)
    {
        m_playerButton.ResetSelection();

        if(playerToSpawn != null)
        {
            Quaternion defaultRot = Quaternion.identity;
            GameObject newPlayer = Instantiate(playerToSpawn , roundedPos , defaultRot) as GameObject;
            newPlayer.transform.parent = m_playerParent.transform;
        }
        else
        {
            Debug.LogError("Sir Bhanu, no player has been selected yet. Please select the Player you want to Spawn");
        }
    }

    Vector2 WorldPointOfMouseClick()
    {
        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;
        float distanceFromCamera = 1f;

        Vector3 weirdTriplet = new Vector3(mouseX , mouseY , distanceFromCamera);
        Vector2 worldPos = m_myCamera.ScreenToWorldPoint(weirdTriplet);

        return worldPos;
    }
}
