using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginSystem : MonoBehaviour
{
    public bool isLogin = true;
    public string username, email, password;
    // Update is called once per frame
    void Update()
    {
        if(isLogin)
        {

        }
    }

    IEnumerator CreateAccount(string username, string email, string password)
    {
        string createUserURL = "http://localhost/sqlsystem/createuser.php";
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
        StartCoroutine(CreateAccount(username, email, password));
    }

    public void Login()
    {
        Debug.Log("Logging in...");
        StartCoroutine(Login(username, password));
    }

    IEnumerator Login(string username, string password)
    {
        string loginURL = "http://localhost/sqlsystem/login.php";
        WWWForm user = new WWWForm();
        user.AddField("username_Post", username);
        user.AddField("password_Post", password);
        WWW www = new WWW(loginURL, user);
        yield return www;
        Debug.Log(www.text);
    }
}