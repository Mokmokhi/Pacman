using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    // Start is called before the first frame update
   
    [SerializeField]
    DataBaseManager firebasemanager;
    [SerializeField]
    TMP_InputField inputEmail;
    [SerializeField]
    TMP_InputField inputUserName;
    [SerializeField]
    TMP_InputField inputPassword;
    [SerializeField]
    TMP_InputField inputConfirmPassword;
    void Awake()
    {
        firebasemanager.auth.StateChanged += authStateChanged;
    }

    // Update is called once per frame

    public void Register() {
        firebasemanager.Register(inputEmail.text, inputPassword.text);
        firebasemanager.SaveData("UserName", inputUserName.text);
    }

    public void Login() {
        if (checkPasswordConfirmation(inputPassword.text, inputConfirmPassword.text))
            firebasemanager.Login(inputEmail.text, inputPassword.text);
        else {
            inputConfirmPassword.text = "";
            Debug.Log("Passwords do not match");
        }
    }
    public void Logout()
    {
        firebasemanager.Logout();
    }

    public string GetEmail() {
        return firebasemanager.GetEmail();
    }

    private bool checkPasswordConfirmation(string password, string confirm) {
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirm))
            return false;
        if (password != confirm)
            return false;
        return true;
    }

     private void authStateChanged(object sender, System.EventArgs e) {
        if (firebasemanager.user == null) {
            //Todo: Switch panel to login panel
        } else {
            //Todo: Switch panel to Main menu
        }
    }
    private void onDestroy() {
        firebasemanager.auth.StateChanged -= authStateChanged;
    }
}
