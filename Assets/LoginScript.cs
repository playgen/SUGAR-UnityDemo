using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using PlayGen.SGA.ClientAPI;
using PlayGen.SGA.Contracts;

public class LoginScript : MonoBehaviour
{
	private AccountClientProxy _accountProxy;
	public InputField UsernameInput;
	public InputField PasswordInput;
	public Text StatusText;
	public Button LoginButton;
	public Button RegisterButton;

	void Start ()
	{
		_accountProxy = ControllerScript.ProxyFactory.GetAccountClientProxy;
		LoginButton.onClick.AddListener(Login);
		RegisterButton.onClick.AddListener(Register);
	}

	private void Register()
	{
		if (CheckFields())
		{
			try
			{
				var accountResponse = _accountProxy.Register(CreateAccountRequest());
				StatusText.text = "Successfully Registered. ID:" + accountResponse.User.Id + ". Please Login.";
				ControllerScript.UserId = accountResponse.User.Id;
			}
			catch (Exception ex)
			{
				StatusText.text = "Failed Registration. " + ex.Message;
			}
		}
	}

	private void Login()
	{
		if (CheckFields())
		{
			try
			{
				var accountRequest = CreateAccountRequest();
				var accountResponse = _accountProxy.Login(accountRequest);
				ControllerScript.UserId = accountResponse.User.Id;
				ControllerScript.LoginToken = accountResponse.Token;
				StatusText.text = "Login Successful!";
				ControllerScript.ActivateAchievementPanels();
				ControllerScript.NextView();
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
