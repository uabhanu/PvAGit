using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FacebookManager : MonoBehaviour 
{
	int m_currentLevel;

	[SerializeField] bool m_loggedIn;

	[SerializeField] Color m_inviteButtonColour; //Do this for Nappyville Game

	[SerializeField] Image /*m_inviteButtonImage ,*/ m_logInButtonImage , m_profilePicImage , m_shareButtonImage;

	[SerializeField] string m_appLinkURL = "http://google.co.uk";

	[SerializeField] Text m_noInternetText , m_username;

	public Color m_shareButtonColour;

	void Start()
	{
		m_currentLevel = SceneManager.GetActiveScene().buildIndex;

		if(!FB.IsInitialized) 
		{
			FB.Init(SetInit, OnHideUnity);	
		} 
		else 
		{
			LoggedIn();
		}

		if(m_currentLevel > 1)
		{
			//m_inviteButtonColour = m_inviteButtonImage.color;
			m_shareButtonColour = m_shareButtonImage.color;	
		}
	}

	void Update()
	{
		if(Time.timeScale == 0)
		{
			return;
		}

		if(m_currentLevel == 1)
		{
			if(!m_loggedIn)
			{
				m_logInButtonImage.enabled = true;
				m_profilePicImage.enabled = false;
				m_username.enabled = false;	
			}

			if(m_loggedIn)
			{
				m_logInButtonImage.enabled = false;
				m_profilePicImage.enabled = true;
				m_username.enabled = true;	
			}
		}

		if(LevelManager.m_levelCompleteVisible && LevelManager.m_continueButtonColour.a >= 1)
		{
			if(m_inviteButtonColour.a < 1)
			{
				m_inviteButtonColour.a += 0.05f;
				//m_inviteButtonImage.color = m_inviteButtonColour;
			}

			if(m_inviteButtonColour.a >= 1 && m_shareButtonColour.a < 1)
			{
				m_shareButtonColour.a += 0.05f;
				m_shareButtonImage.color = m_shareButtonColour;
			}
		}
	}

	void AppLink(IAppLinkResult applinkResult)
	{
		if(!string.IsNullOrEmpty(applinkResult.Url))
		{
			m_appLinkURL = applinkResult.Url; 
		}
	}

	void AuthCallBack(IResult authResult)
	{
		if(authResult.Error != null) 
		{
			Debug.LogError("Sir Bhanu, there is an issue : " + authResult.Error);	
			m_loggedIn = false;
			m_noInternetText.enabled = true;
		} 
		else 
		{
			if(FB.IsLoggedIn) 
			{
				//Debug.Log("Player Logged in"); //If you get 400 error, it means the user token of https://developers.facebook.com/tools/accesstoken/?app_id=142429536402184 you noted down is incorrect which is easy to resolve so not to worry
				FB.API("/me?fields=first_name" , HttpMethod.GET , UsernameDisplay);
				FB.API("/me/picture?type=square&height=480&width=480" , HttpMethod.GET , ProfilePicDisplay);
				m_loggedIn = true;
			} 
			else 
			{
				//Debug.LogError("Sir Bhanu, Player hasn't logged in on Facebook");
				m_loggedIn = false;
			}
		}
	}

	public void Invite()
	{
		FB.Mobile.AppInvite
		(
			new System.Uri(m_appLinkURL),
			new System.Uri("http://google.co.uk"), //Correct URL may be just like the one in Share() below
			callback: InviteRewardUser
		);
	}

	void InviteRewardUser(IResult inviteResult)
	{
		if(inviteResult.Cancelled || !string.IsNullOrEmpty (inviteResult.Error)) 
		{
			Debug.LogError("Sir Bhanu, there is an " + inviteResult.Error);
		} 

		else if(!string.IsNullOrEmpty(inviteResult.RawResult)) 
		{
			Debug.Log(inviteResult.RawResult);
		} 

		else 
		{
			Debug.Log("Share Succeeded");
			//You can Reward/Thank Player here
		}
	}

	void LoggedIn()
	{
		if(m_currentLevel == 1)
		{
			m_loggedIn = true;
			m_logInButtonImage.enabled = false;
			FB.API("/me?fields=first_name" , HttpMethod.GET , UsernameDisplay);
			FB.API("/me/picture?type=square&height=480&width=480" , HttpMethod.GET , ProfilePicDisplay);	
		}
	}

	public void Login()
	{
		List<string> permissions = new List<string>();
		permissions.Add("public_profile");
		FB.LogInWithReadPermissions(permissions , AuthCallBack);
	}

	public void LogOut()
	{
		Debug.Log("Facebook Logout");
		FB.LogOut(); //Not Working
	}

	void OnHideUnity(bool isGameShown)
	{
		if(!isGameShown) 
		{
			Time.timeScale = 0;
		} 
		else 
		{
			Time.timeScale = 1;	
		}
	}

	void ProfilePicDisplay(IGraphResult graphicResult)
	{
		if(graphicResult.Texture != null)
		{
			m_profilePicImage.sprite = Sprite.Create(graphicResult.Texture , new Rect(0 , 0 , 480 , 480) , new Vector2());
			m_profilePicImage.enabled = true;
		}
	}

	void SetInit()
	{
		if(FB.IsLoggedIn) 
		{
			//Debug.Log("Player Logged in"); //If you get 400 error, it means the user token of https://developers.facebook.com/tools/accesstoken/?app_id=142429536402184 you noted down is incorrect which is easy to resolve so not to worry
			FB.API("/me?fields=first_name" , HttpMethod.GET , UsernameDisplay);
			FB.API("/me/picture?type=square&height=480&width=480" , HttpMethod.GET , ProfilePicDisplay);
			m_loggedIn = true;
		} 
		else 
		{
			//Debug.LogError("Sir Bhanu, Player hasn't logged in on Facebook");
			m_loggedIn = false;
		}
	}

	public void Share()
	{
		FB.ShareLink
		(
			contentTitle: "Fourth Lion Studios Message",
			contentURL: new System.Uri("http://google.co.uk"), //Correct URL will be Play Store one when this game is reinstated
			contentDescription: "We really hope you love the game",
			callback: ShareRewardUser
		);
	}

	void ShareRewardUser(IShareResult shareResult)
	{
		if(shareResult.Cancelled || !string.IsNullOrEmpty (shareResult.Error)) 
		{
			Debug.LogError("Sir Bhanu, there is an " + shareResult.Error);
		} 

		else if(!string.IsNullOrEmpty(shareResult.PostId)) 
		{
			Debug.Log(shareResult.PostId);
		} 

		else 
		{
			Debug.Log("Share Succeeded");	
			//You can Reward/Thank Player here
		}
	}

	void UsernameDisplay(IResult result)
	{
		if(result.Error == null)
		{
			//Debug.Log(result.ResultDictionary["first_name"]);
			m_username.text =  "Hi " + result.ResultDictionary["first_name"];
			m_username.enabled = true;
		}
	}
}
