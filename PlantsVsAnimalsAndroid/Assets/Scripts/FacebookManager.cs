using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FacebookManager : MonoBehaviour 
{
	int m_currentLevel;

	[SerializeField] bool m_isLoggedIn;

	//[SerializeField] Color m_inviteButtonColour; //Do this for Nappyville Game

	[SerializeField] GameObject m_loggedInObj , m_loggedOutObj , m_noInternetObj;

	[SerializeField] Image /*m_inviteButtonImage ,*/ m_profilePicImage , m_shareButtonImage;

	//[SerializeField] string m_appLinkURL = "https://play.google.com/store/apps/details?id=com.FLSs.PA";

	[SerializeField] Text m_noInternetText , m_username;

	public Color m_shareButtonColour;

	void Start()
	{
		m_currentLevel = SceneManager.GetActiveScene().buildIndex;

		if(!FB.IsInitialized) 
		{
			FB.Init(SetInit , OnHideUnity);	
		}

		if(m_currentLevel == 1)
		{
			if(FB.IsLoggedIn)
			{
				LoggedIn();
			}

			else if(!FB.IsLoggedIn)
			{
				LoggedOut();
			}
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
			if(!m_isLoggedIn)
			{
				LoggedOut();
			}

			if(m_isLoggedIn)
			{
				LoggedIn();
			}
		}

		if(LevelManager.m_levelCompleteVisible && LevelManager.m_continueButtonColour.a >= 1)
		{
//			if(m_inviteButtonColour.a < 1)
//			{
//				m_inviteButtonColour.a += 0.05f;
//				//m_inviteButtonImage.color = m_inviteButtonColour;
//			}

			if(/*m_inviteButtonColour.a >= 1 && */m_shareButtonColour.a < 1)
			{
				m_shareButtonColour.a += 0.05f;
				m_shareButtonImage.color = m_shareButtonColour;
			}
		}
	}

//	void AppLink(IAppLinkResult applinkResult)
//	{
//		if(!string.IsNullOrEmpty(applinkResult.Url))
//		{
//			m_appLinkURL = applinkResult.Url; 
//		}
//	}

	void AuthCallBack(IResult authResult)
	{
		if(authResult.Error != null) 
		{
			Debug.LogError("Sir Bhanu, there is an issue : " + authResult.Error);	
			m_noInternetObj.SetActive(true);
		} 

		else if(authResult.Error == null)
		{
			if(FB.IsLoggedIn) 
			{
				Debug.Log("Player Logged in"); //If you get 400 error, it means the user token of https://developers.facebook.com/tools/accesstoken/?app_id=142429536402184 you noted down is incorrect which is easy to resolve so not to worry
				//FB.API("/me?fields=first_name" , HttpMethod.GET , UsernameDisplay);
				//FB.API("/me/picture?type=square&height=480&width=480" , HttpMethod.GET , ProfilePicDisplay);
				LoggedIn();
			} 
			else 
			{
				//Debug.LogError("Sir Bhanu, Player hasn't logged in on Facebook");
				LoggedOut();
			}
		}
	}

//	public void Invite()
//	{
//		FB.Mobile.AppInvite
//		(
//			new System.Uri(m_appLinkURL),
//			new System.Uri("http://google.co.uk"), //Fourth Lion Studios Home Page here
//			callback: InviteRewardUser
//		);
//	}

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
		m_isLoggedIn = true;

		FB.API("/me?fields=first_name" , HttpMethod.GET , UsernameDisplay);
		FB.API("/me/picture?type=square&height=480&width=480" , HttpMethod.GET , ProfilePicDisplay);

		if(m_currentLevel == 1)
		{
			m_loggedInObj.SetActive(true);
			m_loggedOutObj.SetActive(false);
		}
	}

	void LoggedOut()
	{
		m_isLoggedIn = false;

		if(m_currentLevel == 1)
		{
			m_loggedInObj.SetActive(false);
			m_loggedOutObj.SetActive(true);
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
		if(graphicResult.Texture != null && m_currentLevel == 1)
		{
			m_profilePicImage.sprite = Sprite.Create(graphicResult.Texture , new Rect(0 , 0 , 480 , 480) , new Vector2());
		}
	}

	void SetInit()
	{
		if(FB.IsLoggedIn) 
		{
			Debug.Log("Player Logged in"); //If you get 400 error, it means the user token of https://developers.facebook.com/tools/accesstoken/?app_id=142429536402184 you noted down is incorrect which is easy to resolve so not to worry
			//FB.API("/me?fields=first_name" , HttpMethod.GET , UsernameDisplay);
			//FB.API("/me/picture?type=square&height=480&width=480" , HttpMethod.GET , ProfilePicDisplay);
			LoggedIn();
		} 
		else 
		{
			//Debug.LogError("Sir Bhanu, Player hasn't logged in on Facebook");
			LoggedOut();
		}
	}

	public void Share()
	{
		Screen.orientation = ScreenOrientation.Portrait;

		FB.ShareLink
		(
			contentTitle: "Fourth Lion Studios Message",
			contentURL: new System.Uri("https://play.google.com/store/apps/details?id=com.FLSs.PA"),
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
			Screen.orientation = ScreenOrientation.Landscape;
			//You can Reward/Thank Player here
		}
	}

	void UsernameDisplay(IResult result)
	{
		if(result.Error == null && m_currentLevel == 1)
		{
			//Debug.Log(result.ResultDictionary["first_name"]);
			m_username.text =  "Hi " + result.ResultDictionary["first_name"];
		}
	}
}
