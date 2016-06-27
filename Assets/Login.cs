using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using PlayGen.SUGAR.Client;
using PlayGen.SUGAR.Contracts;

public class Login : MonoBehaviour
{
	private AccountClient _account;
	public InputField UsernameInput;
	public InputField PasswordInput;
	public Text StatusText;
	public Button LoginButton;
	public Button RegisterButton;

	void Start ()
	{
		_account = Controller.Factory.GetAccountClient;
		LoginButton.onClick.AddListener(LoginUser);
		RegisterButton.onClick.AddListener(Register);
	}

	private void Register()
	{
		if (CheckFields())
		{
			try
			{
				var accountResponse = _account.Register(CreateAccountRequest());
				StatusText.text = "Successfully Registered. ID:" + accountResponse.User.Id + ". Please Login.";
				Controller.UserId = accountResponse.User.Id;
			}
			catch (Exception ex)
			{
				StatusText.text = "Failed Registration. " + ex.Message;
			}
		}
	}

	private void LoginUser()
	{
		if (CheckFields())
		{
			try
			{
				var accountRequest = CreateAccountRequest();
				var accountResponse = _account.Login(accountRequest);
				Controller.UserId = accountResponse.User.Id;
				Controller.LoginToken = accountResponse.Token;
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

	private AccountRequest CreateAccountRequest()
	{
		
		return new AccountRequest()
		{
			Name = UsernameInput.text,
			Password = PasswordInput.text
		};
	}
   
}
