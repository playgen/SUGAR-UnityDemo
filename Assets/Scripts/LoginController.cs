using System;
using UnityEngine;
using UnityEngine.UI;
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

	private string _defaultStatusText;

	void Awake ()
	{
		_accountClient = ScriptLocator.Controller.Factory.Account;
		_defaultStatusText = StatusText.text;
	}

	private void Reset()
	{
		StatusText.text = _defaultStatusText;
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
			var accountResponse = GetRegisterAccountResponse(UsernameInput.text, PasswordInput.text);
			if (accountResponse != null)
			{
				StatusText.text = "Successfully Registered. ID:" + accountResponse.User.Id + ". Please Login.";
				ScriptLocator.Controller.UserId = accountResponse.User.Id;
			}
		}
	}

	public AccountResponse GetRegisterAccountResponse(string username, string password, bool autoLogin = false)
	{
		var accountRequest = CreateAccountRequest(username, password, autoLogin);
		try
		{
			return _accountClient.Register(accountRequest);
		}
		catch (Exception ex)
		{
			StatusText.text = "Failed Register. " + ex.Message;
			Debug.LogError(ex);
			return null;
		}
	}

	private void LoginUser()
	{
		if (CheckFields())
		{
			var accountResponse = GetLoginAccountResponse(UsernameInput.text, PasswordInput.text);
			if (accountResponse != null)
			{
				ScriptLocator.Controller.UserId = accountResponse.User.Id;
				//Controller.LoginToken = accountResponse.Token;
				UsernameInput.text = "";
				PasswordInput.text = "";
				StatusText.text = "";
				ScriptLocator.Controller.ActivateUiPanels();
				ScriptLocator.ResourceController.AddResource("Daily Chocolate", 1, accountResponse.User.Id);
				ScriptLocator.Controller.NextView();
			}
		}
	}

	public AccountResponse GetLoginAccountResponse(string username, string password)
	{
		var accountRequest = CreateAccountRequest(username, password);
		try
		{
			var logged = _accountClient.Login(accountRequest);
			return logged;
		}
		catch (Exception ex)
		{
			StatusText.text = "Failed Login. " + ex.Message;
			Debug.LogError(ex);
			return null;
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
