using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogIOManager : MonoBehaviour
{
    // inputEmail, inputPassword and inputChangeName are used to pass the user input to the database.
    [SerializeField]
    TMP_InputField inputEmail;
    [SerializeField]
    TMP_InputField inputPassword;
    [SerializeField]
    TMP_InputField inputChangeName;



    // function Login to login the authentication.
    public void Login() {
            DataBaseManager.Instance.Login(inputEmail.text, inputPassword.text);
            inputEmail.text = "";
            inputPassword.text = "";
    }
    // function Logout to logout the authentication and save the data.
    public void Logout()
    {
        DataBaseManager.Instance.SaveData();
        DataBaseManager.Instance.Logout();
        DataBaseManager.Instance.profile.resetProfile();
    }
    // function ChangeName to change the name of the player profile.
    public void ChangeName() {
        if (inputChangeName.text.Length > 0) {
            DataBaseManager.Instance.ChangeName(inputChangeName.text);
            inputChangeName.text = "";
        }
    }
}
