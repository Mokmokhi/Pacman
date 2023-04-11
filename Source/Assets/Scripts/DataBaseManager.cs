using Firebase.Database;
using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Firebase.Auth.FirebaseAuth auth;
    public Firebase.Auth.FirebaseUser user;
    public PlayerProfile profile;
    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += authStateChanged;
        profile = new PlayerProfile();
        
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

    public void SaveData() {
        if (user != null) {
            var record = JsonUtility.ToJson(profile);
            GetUserReference().Push().SetRawJsonValueAsync(record).ContinueWith(task => {
                if (task.IsCompletedSuccessfully) {
                    print("Save Success.");
                }
            });
        } else {
            print("Save Failure.");
        }
    }



    public void LoadData() {
        if (user != null) {
            GetUserReference().GetValueAsync().ContinueWith(task => {
                if (task.IsCompletedSuccessfully) {
                    var val = task.Result.GetRawJsonValue();
                    profile = JsonUtility.FromJson<PlayerProfile>(val);
                    print("Data loaded."); 
                }
            });
        }
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

    private DatabaseReference GetUserReference() {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        return reference.Child(user.UserId);
    }
}

public class PlayerProfile {
    private string UserName;
    private int HighestScore;
    private bool HasSkin;
    public PlayerProfile() {
        UserName = "User";
        HighestScore = 0;
        HasSkin = false;
    }
    public PlayerProfile(string name) {
        UserName = name;
        HighestScore = 0;
        HasSkin = false;
    }

    public string getName() {
        return UserName;
    }
    public void setName(string name) {
        UserName = name;
    }

    public int getScore() {
        return HighestScore;
    }
    public void setScore(int score) {
        HighestScore = score;
    }

    public bool hasSkin() {
        return HasSkin;
    }
    public void setSkin() {
        HasSkin = true;
    }

    public void resetProfile() {
        UserName = "User";
        HighestScore = 0;
        HasSkin = false;
    }
}