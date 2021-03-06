﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEngine.UI;

using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class LoginSystem : MonoBehaviour
{
    public bool isLogin = true;
    public string username, email, password;

    private string codeLength;
    private string loginMessage;
    public GameObject loginPage, registerPage;
    public Text userText, passText;

    public Text userNameText, emailText, passwordText;
    // Update is called once per frame
    void Update()
    {
        if (!isLogin)
        {
            registerPage.SetActive(true);
            loginPage.SetActive(false);
        }
        else
        {
            registerPage.SetActive(false);
            loginPage.SetActive(true);
        }
    }

    IEnumerator CreateAccount(string username, string email, string password)
    {
        string createUserURL = "http://localhost/sqlassessment/createuser.php";
        WWWForm user = new WWWForm();
        user.AddField("username_Post", username);
        user.AddField("email_Post", email);
        user.AddField("password_Post", password);
        WWW www = new WWW(createUserURL, user);
        yield return www;
        Debug.Log(www.text);
    }

    public void SwapSets()
    {
        isLogin = !isLogin;
    }


    public void Register()
    {
        Debug.Log("Sending...");
        StartCoroutine(CreateAccount(userNameText.text, emailText.text, passwordText.text));
    }

    public void Login()
    {
        Debug.Log("Logging in...");
        StartCoroutine(Login(userText.text, passText.text));
        //SceneManager.LoadScene("Game");
    }

    public void ResetPassword()
    {
        StartCoroutine(CheckEmail(email));
    }

    IEnumerator Login(string username, string password)
    {
        string loginURL = "http://localhost/sqlassessment/login.php";
        WWWForm user = new WWWForm();
        user.AddField("username_Post", username);
        user.AddField("password_Post", password);
        WWW www = new WWW(loginURL, user);
        yield return www;
        Debug.Log(www.text);
        loginMessage = www.text;
        if (loginMessage.Contains("go"))
            SceneManager.LoadScene(1);
    }

    void SendEmail(string email, string debugUser)
    {
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("sqlunityclasssydney@gmail.com");
        mail.To.Add(email);
        mail.Subject = "Password Reset";
        StartCoroutine(CreateCode(8));
        mail.Body = "Hello " + username + "\nReset your account using the code below" + "\n\n" + codeLength;

        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 25;
        smtpServer.Credentials = new System.Net.NetworkCredential("sqlunityclasssydney@gmail.com", "sqlpassword") as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate cert, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        };
        smtpServer.Send(mail);
        Debug.Log("Sending email... please wait a few minutes");
    }

    IEnumerator CheckEmail(string email)
    {
        string loginURL = "http://localhost/sqlassessment/CheckEmail.php";
        WWWForm checkEmail = new WWWForm();
        checkEmail.AddField("email_Post", email);
        WWW www = new WWW(loginURL, checkEmail);
        yield return www;
        Debug.Log(www.text);
        if (www.text != "There is no account associated with this email")
        {
            SendEmail(email, www.text);
        }
    }

    IEnumerator CreateCode(int length)
    {
        for (int i = 0; i < length; i++)
        {
            i = Random.Range(0033, 0127);
            Convert.ToChar(i);
            codeLength += i;
        }
        yield return codeLength;
    }

    IEnumerator ResetPassword(string email, string password)
    {
        string resetURL = "http://localhost/sqlassessment/ResetPassword.php";
        WWWForm resetPassword = new WWWForm();
        resetPassword.AddField("email_Post", email);
        resetPassword.AddField("password_Post", password);
        WWW www = new WWW(resetURL, resetPassword);
        yield return www;
        if (www.text.Contains("Password Updated"))
        {
            ResetPassword();
        }
    }

}