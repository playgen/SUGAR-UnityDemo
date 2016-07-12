using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using PlayGen.SUGAR.Client;
using PlayGen.SUGAR.Contracts;

public class LoginController : MonoBehaviour
{
	private AccountClient _accountClient;
	public InputField UsernameInput;
	public InputField PasswordInput;
	public Text StatusText;
	public Button LoginButton;
	public Button RegisterButton;

	void Awake ()
	{
		_accountClient = ScriptLocator.Controller.Factory.Account;
	}

	void Start()
	{
		LoginButton.onClick.AddListener(LoginUser);
		RegisterButton.onClick.AddListener(RegisterUser);
	}

	private void RegisterUser()
	{
		if (CheckFields())
		{
			try
			{
				var accountResponse = GetRegisterAccountResponse(UsernameInput.text, PasswordInput.text);
				StatusText.text = "Successfully Registered. ID:" + accountResponse.User.Id + ". Please Login.";
				ScriptLocator.Controller.UserId = accountResponse.User.Id;
			}
			catch (WebException exception)
			{
				StatusText.text = "Failed Registration. " + exception.Response;
				Debug.LogError(exception);
			}
		}
	}

	public AccountResponse GetRegisterAccountResponse(string username, string password, bool autoLogin = false)
	{
		var accountRequest = CreateAccountRequest(username, password, autoLogin);
		return _accountClient.Register(accountRequest);
	}

	private void LoginUser()
	{
		if (CheckFields())
		{
			try
			{
				var accountResponse = GetLoginAccountResponse(UsernameInput.text, PasswordInput.text);
				ScriptLocator.Controller.UserId = accountResponse.User.Id;
				//Controller.LoginToken = accountResponse.Token;
				StatusText.text = "Login Successful!";
				ScriptLocator.Controller.ActivateAchievementPanels();
				ScriptLocator.ResourceController.AddResource("Daily Chocolate", 1, ScriptLocator.Controller.UserId.Value);
				ScriptLocator.Controller.NextView();
			}
			catch (WebException exception)
			{
				StatusText.text = "Failed Login. " + exception;
				Debug.LogError(exception);
			}
		}
	}

	public AccountResponse GetLoginAccountResponse(string username, string password)
	{
		var accountRequest = CreateAccountRequest(username, password);
		return _accountClient.Login(accountRequest);
		
	}

	private bool CheckFields()
	{
		StatusText.text = "";
		if (string.IsNullOrEmpty(UsernameInput.text) || string.IsNullOrEmpty(PasswordInput.text))
		{
			StatusText.text = "Username or Password field missing";
			return false;
		}
		return true;
	}

	private AccountRequest CreateAccountRequest(string user, string pass, bool autoLogin = false)
	{
		
		return new AccountRequest()
		{
			Name = user,
			Password = pass,
			AutoLogin = autoLogin
		};
	}
   
}
