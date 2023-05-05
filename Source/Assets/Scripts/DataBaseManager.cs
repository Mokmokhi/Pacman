using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

// DatabaseManager is a module that manage the communication between the database and the game.
public class DataBaseManager : Singleton<DataBaseManager>
{
    // auth is a variable that store the authentication status of the user.
    public Firebase.Auth.FirebaseAuth auth;
    
    // user is a variable that stores the user account information.

    public Firebase.Auth.FirebaseUser user;

    // profile is a variable that stores the user profile information.
    public PlayerProfile profile;

    // the class structure of the player profile.
    
    public class PlayerProfile {

    // UserName stores the user name of the current user.
    public string UserName;
    // HighestScore stores the highest score of the current user.
    public int HighestScore;
    // Coins stores the number coin of the current user.
    public int Coins;
    // Has Skin stores the skins the current user has. It stores the skin id by bit-wise combination.
    public int HasSkin;
    // UsingSkin stores the skin id the user is using.
    public int UsingSkin;
    // PowerLevel stores the powerpellet level of the current user.
    public int PowerLevel;
    // Constructor of the playerprofile.
    public PlayerProfile() {
        UserName = "User";
        HighestScore = 0;
        Coins = 0;
        HasSkin = 1;
        UsingSkin = 0;
        PowerLevel = 1;
    }
    public PlayerProfile(string name) {
        UserName = name;
        HighestScore = 0;
        Coins = 0;
        HasSkin = 1;
        UsingSkin = 0;
        PowerLevel = 1;
    }
    // ResetProfile for dev.
    public void resetProfile() {
        UserName = "User";
        HighestScore = 0;
        Coins = 0;
        HasSkin = 1;
        UsingSkin = 0;
        PowerLevel = 1;
    }
}

    // When the program starts, Start function is called.
    void Start()
    {
        // Make a new local profile to store data from the database later.
        profile = new PlayerProfile();
        // Link auth to the Authentication Instance of the firebase.
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        // Listen to the authentication state change event.
        auth.StateChanged += authStateChanged;
    }

    // Register function to create new user account on firebase authentication.
    public void Register(string name, string email, string password) {

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            // If creating user account is failed, print error message.
            if (task.IsFaulted) {
                print(task.Exception.InnerException.Message);
            }
            // else, print the new username.
            if (task.IsCompletedSuccessfully) {
                profile.UserName = name;
                printInfo();
                SaveData();
            }
        });
    }
    // Login function to sign in with account on firebase authentication.
    public void Login(string email, string password) {

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsFaulted) {
                print(task.Exception.InnerException.Message);
            }
            if (task.IsCompletedSuccessfully) {
                print("Login success");
            }
        });
    }
    // SaveData function to save the local player profile to the realtime database on Firebase.
    public void SaveData() {
        // check if there is a user logged in.
        if (user != null) {
            var record = JsonUtility.ToJson(profile);
            GetReference().Child(user.UserId).SetRawJsonValueAsync(record).ContinueWithOnMainThread(task => {
                if (task.IsFaulted) {
                print(task.Exception.InnerException.Message);
            }
                if (task.IsCompletedSuccessfully) {
                    print("Save Success.");
                }
            });
        } else {
            print("Not logged in.");
        }
    }
    // SaveAndQuit function to quit the game.
    public void SaveAndQuit() {
        if (user != null) {
            var record = JsonUtility.ToJson(profile);
            GetReference().Child(user.UserId).SetRawJsonValueAsync(record).ContinueWithOnMainThread(task => {
                if (task.IsFaulted) {
                    print(task.Exception.InnerException.Message);
                }
                if (task.IsCompletedSuccessfully) {
                    print("Save Success.");
                }
                
            });
        } 
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
    // LoadData function to load the player profile data from the database to the local player profile.
    public void LoadData() {
        if (user != null) {
            GetReference().Child(user.UserId).GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsCompletedSuccessfully) {
                    var val = task.Result.GetRawJsonValue();
                    profile = JsonUtility.FromJson<PlayerProfile>(val);
                    print("Data loaded."); 
                    printInfo();
                } else {
                    print("Load Failure.");
       
                }
            });
        } else print("No user.");

    }
    // Logout function to log out the game.
    public void Logout() {
        auth.SignOut();
    }
    // printInfo function to print the current username.
    public void printInfo() {
        print("Current User: " + profile.UserName);
    }
    // ChangeName function to change the username of the current user.
    public void ChangeName(string name) {
        // Check if the username is empty.
        if (name != "") {
            profile.UserName = name;
            SaveData();
        }
        else print ("User name cannot be null.");
    }

    // GetReference function to get the reference to the database instance.
    public DatabaseReference GetReference() {
        DatabaseReference reference = FirebaseDatabase.GetInstance("https://unitypacman-b8e1d-default-rtdb.asia-southeast1.firebasedatabase.app/").RootReference;
        return reference;
    }

    // authStateChanged function to do something when the auth state is changed.
    private void authStateChanged(object sender, System.EventArgs e) {
        if (auth.CurrentUser != user) {
            user = auth.CurrentUser;
            //whenver there is a user logged in, switch to the main menu and load the player profile.
            if (user != null) {
                UIManager.Instance.GetComponent<PanelSwitcher>().SwitchActivePanelByName("1-Main");
                printInfo();
                print(user.Email);
                LoadData();
            } // If there is not user, switch to the login panel.
            else {
                UIManager.Instance.GetComponent<PanelSwitcher>().SwitchActivePanelByName("0-Login");
            }
        }
    }

}

