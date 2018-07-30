using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginSystem : MonoBehaviour
{
    public string username, email, password;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Sending...");
            StartCoroutine(CreateAccount(username, email, password));
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
}
