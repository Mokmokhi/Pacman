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
    [SerializeField]
    TMP_InputField inputChangeName;



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

    public void ChangeName() {
        if (inputChangeName.text.Length > 0) {
            DataBaseManager.Instance.ChangeName(inputChangeName.text);
        }
    }
}
