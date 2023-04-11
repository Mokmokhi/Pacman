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

        if (checkPasswordConfirmation() && VerificationRegister()) {
            firebasemanager.Register(inputEmail.text, inputPassword.text);
            firebasemanager.profile.setName(inputUserName.text);
            firebasemanager.SaveData();
        } else {
            inputConfirmPassword.text = "";
            Debug.Log("Passwords do not match");
        }
    }

    public void Login() {
            firebasemanager.Login(inputEmail.text, inputPassword.text);
            firebasemanager.LoadData();
    }
    public void Logout()
    {
        firebasemanager.SaveData();
        firebasemanager.Logout();
        firebasemanager.profile.resetProfile();
    }

    public string GetEmail() {
        return firebasemanager.GetEmail();
    }

    private bool VerificationRegister() {
        if (inputUserName.text.Length > 0 && inputPassword.text.Length >= 8) {
            return true;
        }
        else return false;
    }
    private bool checkPasswordConfirmation() {
        if (string.IsNullOrEmpty(inputPassword.text) || string.IsNullOrEmpty(inputConfirmPassword.text))
            return false;
        if (inputPassword.text != inputConfirmPassword.text)
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
