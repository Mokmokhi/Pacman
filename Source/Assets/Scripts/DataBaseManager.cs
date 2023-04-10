using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Firebase.Auth.FirebaseAuth auth;
    public Firebase.Auth.FirebaseUser user;
    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += authStateChanged;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Register(string email, string password) {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled) {
                return;
            }
            if (task.IsFaulted) {
                print(task.Exception.InnerException.Message);
                return;
            }
            if (task.IsCompletedSuccessfully) {
                print("Register success");
            }
        });
    }

    public void Login(string email, string password) {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsFaulted) {
                print(task.Exception.InnerException.Message);
                return;
            }
            if (task.IsCompletedSuccessfully) {
                print("Login success");
            }
        });
    }

    public void Logout() {
        auth.SignOut();
    }

    public string GetEmail() {
        return user.Email;
    }

    private void authStateChanged(object sender, System.EventArgs e) {
        if (auth.CurrentUser != user) {
            user = auth.CurrentUser;
            if (user!= null) {
                print("Current User: " + user.Email);
            }
        }
    }

    private void onDestroy() {
        auth.StateChanged -= authStateChanged;
    }
}