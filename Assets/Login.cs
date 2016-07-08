using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using PlayGen.SUGAR.Client;
using PlayGen.SUGAR.Contracts;

public class Login : MonoBehaviour
{
	private AccountClient _accountClient;
	public InputField UsernameInput;
	public InputField PasswordInput;
	public Text StatusText;
	public Button LoginButton;
	public Button RegisterButton;

	void Start ()
	{
		_accountClient = Controller.Factory.Account;
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
				Controller.UserId = accountResponse.User.Id;
			}
			catch (Exception ex)
			{
				StatusText.text = "Failed Registration. " + ex.Message;
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
				Controller.UserId = accountResponse.User.Id;
				//Controller.LoginToken = accountResponse.Token;
				StatusText.text = "Login Successful!";
				Controller.ActivateAchievementPanels();
				Controller.NextView();
			}
			catch (Exception ex)
			{
				StatusText.text = "Failed Login. " + ex.Message;
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
