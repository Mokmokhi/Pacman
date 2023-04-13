using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogIOManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    TMP_InputField inputEmail;
    [SerializeField]
    TMP_InputField inputPassword;



    // Update is called once per frame
    public void Login() {
            DataBaseManager.Instance.Login(inputEmail.text, inputPassword.text);
    }
    public void Logout()
    {
        DataBaseManager.Instance.SaveData();
        DataBaseManager.Instance.Logout();
        DataBaseManager.Instance.profile.resetProfile();
    }

     private void authStateChanged(object sender, System.EventArgs e) {
        if (DataBaseManager.Instance.user == null) {
            //Todo: Switch panel to login panel
        } else {
            //Todo: Switch panel to Main menu
        }
    }
    private void onDestroy() {
        DataBaseManager.Instance.auth.StateChanged -= authStateChanged;
    }
}
