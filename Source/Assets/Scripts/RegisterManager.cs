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

    // Regester function to receive the input from the user and pass it to the databaseManager.
    public void Register() {
        // Check if the password and confirm password are matached, and the condition or username and password are valid.
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

}
