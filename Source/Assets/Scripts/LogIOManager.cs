using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogIOManager : MonoBehaviour
{
    // Start is called before the first frame update
   [SerializeField]
    DataBaseManager firebasemanager;
    [SerializeField]
    TMP_InputField inputEmail;
    [SerializeField]
    TMP_InputField inputPassword;

    void Awake()
    {
    }

    // Update is called once per frame
    public void Login() {
            firebasemanager.Login(inputEmail.text, inputPassword.text);
    }
    public void Logout()
    {
        firebasemanager.SaveData();
        firebasemanager.Logout();
        firebasemanager.profile.resetProfile();
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
