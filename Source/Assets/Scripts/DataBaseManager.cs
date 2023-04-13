using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class DataBaseManager : Singleton<DataBaseManager>
{
    // Start is called before the first frame update
    public Firebase.Auth.FirebaseAuth auth;
    public Firebase.Auth.FirebaseUser user;
    public PlayerProfile profile;
    [SerializeField]
    public UIManager uiManager;

    public class PlayerProfile {
    public string UserName;
    public int HighestScore;
    public int Coins;
    public int HasSkin;
    public int UsingSkin;
    public uint PowerLevel;
    public PlayerProfile() {
        UserName = "User";
        HighestScore = 0;
        Coins = 0;
        HasSkin = 1;
        UsingSkin = 0;
        PowerLevel = 0;
    }
    public PlayerProfile(string name) {
        UserName = name;
        HighestScore = 0;
        Coins = 0;
        HasSkin = 1;
        UsingSkin = 0;
        PowerLevel = 0;
    }
    public void resetProfile() {
        UserName = "User";
        HighestScore = 0;
        Coins = 0;
        HasSkin = 1;
        UsingSkin = 0;
        PowerLevel = 0;
    }
}
    void Start()
    {
        profile = new PlayerProfile();
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += authStateChanged;
    }

    public void Register(string name, string email, string password) {

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsFaulted) {
                print(task.Exception.InnerException.Message);
            }
            if (task.IsCompletedSuccessfully) {
                
                profile.UserName = name;
                print("Register success: " + profile.UserName);
                SaveData();
            }
        });
    }

    public void Login(string email, string password) {

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsFaulted) {
                print(task.Exception.InnerException.Message);
            }
            if (task.IsCompletedSuccessfully) {
                print("Login success");
            }
            password = "";
        });
    }

    public void SaveData() {
        if (user != null) {
            var record = JsonUtility.ToJson(profile);
            GetReference().Child(user.UserId).SetRawJsonValueAsync(record).ContinueWithOnMainThread(task => {
                if (task.IsCompletedSuccessfully) {
                    print("Save Success.");
                }
            });
        } else {
            print("Save Failure.");
        }
    }
    public void SaveAndQuit() {
        if (user != null) {
            var record = JsonUtility.ToJson(profile);
            GetReference().Child(user.UserId).SetRawJsonValueAsync(record).ContinueWithOnMainThread(task => {
                if (task.IsCompletedSuccessfully) {
                    print("Save Success.");
                    #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                    #endif
                    Application.Quit();
                }
            });
        } else {
            print("Save Failure.");
        }
    }

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

    public void Logout() {
        auth.SignOut();
    }

    public string GetEmail() {
        return user.Email;
    }

    public void printInfo() {
        print("User: " + profile.UserName);
    }
    public DatabaseReference GetReference() {
        DatabaseReference reference = FirebaseDatabase.GetInstance("https://unitypacman-b8e1d-default-rtdb.asia-southeast1.firebasedatabase.app/").RootReference;
        return reference;
    }

    private void authStateChanged(object sender, System.EventArgs e) {
        if (auth.CurrentUser != user) {
            user = auth.CurrentUser;
            if (user != null) {
                uiManager.GetComponent<PanelSwitcher>().SwitchActivePanelByName("1-Main");
                print("Current User: " + user.Email);
                LoadData();
            } else {
                uiManager.GetComponent<PanelSwitcher>().SwitchActivePanelByName("0-Login");
            }
        }
    }

    private void onDestroy() {
        auth.StateChanged -= authStateChanged;
    }
    
}

