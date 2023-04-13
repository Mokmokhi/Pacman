using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RegisterManager : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField]
    public TMP_InputField inputUserName;
    [SerializeField]
    public TMP_InputField inputEmail;
    [SerializeField]
    public TMP_InputField inputPassword;
    [SerializeField]
    public TMP_InputField inputConfirmPassword;

    public void Register() {

        if (checkPasswordConfirmation() && VerificationRegister()) {
            DataBaseManager.Instance.Register(inputUserName.text, inputEmail.text, inputPassword.text);
            inputUserName.text = "";
            inputEmail.text = "";
            inputPassword.text = "";
            inputConfirmPassword.text = "";

        } else {
            inputConfirmPassword.text = "";
            Debug.Log("Passwords do not match");
        }
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
