using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;
using System;
using System.Net;

public class AuthenticationManager : MonoBehaviour
{
    [Header("Screens")]
    public GameObject loginInScreen;
    public GameObject RegisterScreen;
    public GameObject MainMenuScreen;
    public GameObject testScreen;
    public GameObject leaderboardScreen;
    public GameObject openingScreen;

    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth firebaseAuth;
    public FirebaseUser firebaseUser;
    public DatabaseReference databaseReference;

    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text messageLoginText;

    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterFieldOne;
    public TMP_InputField passwordRegisterFieldTwo;
    public TMP_Text messageRegisterText;

    public GameObject scoreElement;
    public Transform scoreboardContent;


    private void Awake()
    {
        // checks that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitialiseFirebase();
            }
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        // checks if the user is already logged in to an account
        if (firebaseAuth.CurrentUser != null)
        {
            // saves logged in user
            firebaseUser = firebaseAuth.CurrentUser;
            // ensures login screen is bypassed
            loginInScreen.SetActive(false);
            openingScreen.SetActive(false);
            MainMenuScreen.SetActive(true);
        }
    }

    private void InitialiseFirebase()
    {
        Debug.Log("Setting up Firebase Authentication");
        // set the authentication instance object
        firebaseAuth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void LoginButton()
    {
        // calls login coroutine passing email and password
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }

    public void RegisterButton()
    {
        // calls register coroutine passing username, email and password
        StartCoroutine(Register(usernameRegisterField.text, emailRegisterField.text, passwordRegisterFieldOne.text));
    }

    // calls the necessary functions to save the data and load it to the database and leaderboard
    public void SaveDailyChallengeData()
    {
        // stores user login data
        firebaseUser = firebaseAuth.CurrentUser;
        string username = firebaseUser.DisplayName;

        // stores daily challenge data
        int newDailyChallengeScore = StaticData.dailyChallengeScore;
        bool playedDailyChallenge = StaticData.dailyChallengePlayed;

        StartCoroutine(UpdateUsernameDatabase(username));
        StartCoroutine(UpdateDailyChallengeHighScoreDatabase(newDailyChallengeScore));
        StartCoroutine(UpdateDailyChallengeBooleanDatabase(playedDailyChallenge));
        StartCoroutine(LoadScoreBoardData());
    }

    private IEnumerator Login(string email, string password)
    {
        // calls Firebase authenticaion signin function passing email and password
        var LoginTask = firebaseAuth.SignInWithEmailAndPasswordAsync(email, password);
        // waits until task is complete
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        // error occurs
        if (LoginTask.Exception != null)
        {
            FirebaseException firebaseException = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseException.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "User not found";
                    break;
            }
            // sets error message to the appropriate error which occurred
            messageLoginText.text = message;
        }
        // no error occurs
        else
        {
            // user now logged in

            firebaseUser = LoginTask.Result.User;
            messageLoginText.text = "Logged in";

            // delays occurence of next part of function by 1 second
            yield return new WaitForSeconds(1);

            // updates daily challenge played boolean
            // stops players from repeatedly playing daily challenge game mode
            StartCoroutine(LoadDailyChallengeBooleanDatabase());

            // updates original high score
            // ensures it is displayed when the user first access the original game mode
            StartCoroutine(LoadOriginalHighscoreDatabase());

            loginInScreen.SetActive(false);
            MainMenuScreen.SetActive(true);
        }
    }

    private IEnumerator Register(string username, string email, string password)
    {
        // if username field is blank
        if (username == "")
        {
            messageRegisterText.text = "Missing Username";
        }
        // if username is greater than 8 characters long
        else if (username.Length > 8)
        {
            messageRegisterText.text = "Username is greater than 8 characters";
        }
        // if passwords do not match
        else if (passwordRegisterFieldOne.text != passwordRegisterFieldTwo.text)
        {
            messageRegisterText.text = "Passwords do not match";
        }
        else
        {
            // calls Firebase authenticaion signin function passing email and password
            var RegiserTask = firebaseAuth.CreateUserWithEmailAndPasswordAsync(email, password);
            // waits until task is complete
            yield return new WaitUntil(predicate: () => RegiserTask.IsCompleted);

            // error occurs
            if (RegiserTask.Exception != null)
            {
                FirebaseException firebaseException = RegiserTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseException.ErrorCode;

                string message = "Register Failed";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                // sets error message to the appropriate error which occurred
                messageRegisterText.text = message;
            }
            // no error occurs
            else
            {
                // user now created

                firebaseUser = RegiserTask.Result.User;

                if (firebaseUser != null)
                {
                    // creeates a user progile and sets the username provided
                    UserProfile userprofile = new UserProfile { DisplayName = username };

                    // calls Firebase authenticator user profile function passing the profile with the username
                    var ProfileTask = firebaseUser.UpdateUserProfileAsync(userprofile);
                    // waits until task is complete
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        // handles errors
                        FirebaseException firebaseException = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseException.ErrorCode;
                    }
                    else
                    {
                        loginInScreen.SetActive(true);
                        RegisterScreen.SetActive(false);
                    }
                }
            }
        }
    }

    private IEnumerator UpdateUsernameDatabase(string username)
    {
        // saves username of logged-in user
        var databaseTask = databaseReference.Child("users").Child(firebaseUser.UserId).Child("username").SetValueAsync(username);
        // waits until database operation is complete
        yield return new WaitUntil(predicate: () => databaseTask.IsCompleted);

        if (databaseTask.Exception != null)
        {
            Debug.Log("Failed to register");
        }
    }

    private IEnumerator UpdateDailyChallengeHighScoreDatabase(int highscore)
    {
        // saves daily challenge high score of logged-in user
        var databaseTask = databaseReference.Child("users").Child(firebaseUser.UserId).Child("daily challenge high score").SetValueAsync(highscore);
        // waits until database operation is complete
        yield return new WaitUntil(predicate: () => databaseTask.IsCompleted);

        if (databaseTask.Exception != null)
        {
            Debug.Log("Failed to register");
        }
    }

    private IEnumerator UpdateDailyChallengeBooleanDatabase(bool dailyChallengePlayed)
    {
        // saves daily challenge bolean of logged-in user
        var databaseTask = databaseReference.Child("users").Child(firebaseUser.UserId).Child("daily challenge played").SetValueAsync(dailyChallengePlayed);
        // waits until database operation is complete
        yield return new WaitUntil(predicate: () => databaseTask.IsCompleted);
        
        if (databaseTask.Exception != null)
        {
            Debug.Log("Failed to register");
        }
    }

    private IEnumerator LoadDailyChallengeBooleanDatabase()
    {
        // loads daily challenge boolean of logged-in user
        var databaseTask = databaseReference.Child("users").Child(firebaseUser.UserId).Child("daily challenge played").GetValueAsync();
        // waits until database operation is complete
        yield return new WaitUntil(predicate: () => databaseTask.IsCompleted);

        if (databaseTask.Exception != null)
        {
            Debug.Log("Failed to register");
        }
        // default if no data exists
        else if (databaseTask.Result.Value == null)
        {
            StaticData.dailyChallengePlayed = false;
        }

        else
        {
            // converts data stored in database (string) to a boolean value
            StaticData.dailyChallengePlayed = bool.Parse(databaseTask.Result.Value.ToString());
        }
    }

    public IEnumerator ChangeDailyChallengeBooleanDatabase()
    {
        // retrieves all users data
        var databaseTask = databaseReference.Child("users").GetValueAsync();
        // waits until database operation is complete
        yield return new WaitUntil(predicate: () => databaseTask.IsCompleted);

        if (databaseTask.Exception != null)
        {
            Debug.LogError("Failed to retrieve user data: " + databaseTask.Exception.Message);
            yield break;
        }

        // retrieve data
        DataSnapshot snapshot = databaseTask.Result;

        // loop through all users 
        foreach (DataSnapshot childSnapshot in snapshot.Children)
        {
            Debug.Log("Function is accessed");

            string userID = childSnapshot.Key;
            
            var updateTask = databaseReference.Child("users").Child(userID).Child("daily challenge played").SetValueAsync(false);

            yield return new WaitUntil(predicate: () => updateTask.IsCompleted);
        }
    }

    private IEnumerator UpdateOriginalHighscoreDatabase(float highscore)
    {
        // saves original high score of logged-in user
        var databaseTask = databaseReference.Child("users").Child(firebaseUser.UserId).Child("original high score").SetValueAsync(highscore);
        // waits until database operation is complete
        yield return new WaitUntil(predicate: () => databaseTask.IsCompleted);

        if (databaseTask.Exception != null)
        {
            Debug.Log("Failed to register");
        }
    }


    // called when the user presses the 'Save + Quit button'
    public void UpdateHighScore()
    {
        // stores original data
        int originalHighScore = (int)StaticData.originalHighScore;

        StartCoroutine(UpdateOriginalHighscoreDatabase(originalHighScore));
    }

    private IEnumerator LoadOriginalHighscoreDatabase()
    {
        // loads original high score of logged-in user
        var databaseTask = databaseReference.Child("users").Child(firebaseUser.UserId).Child("original high score").GetValueAsync();
        // waits until database operation is complete
        yield return new WaitUntil(predicate: () => databaseTask.IsCompleted);

        if (databaseTask.Exception != null)
        {
            Debug.Log("Failed to register");
        }
        // default if no data exists
        else if (databaseTask.Result.Value == null)
        {
            StaticData.originalHighScore = 0;
        }
        else
        {
            // converts data stored in database (string) to a float value
            StaticData.originalHighScore = float.Parse(databaseTask.Result.Value.ToString());
        }
    }


    // loads all users daily challenge high scores and adds them to leadboard
    private IEnumerator LoadScoreBoardData()
    {
        // retrieves all users data and orders by high score (descending)
        var databaseTask = databaseReference.Child("users").OrderByChild("daily challenge high score").GetValueAsync();
        // waits until database operation is complete
        yield return new WaitUntil(predicate: () => databaseTask.IsCompleted);

        // retrieve data
        DataSnapshot snapshot = databaseTask.Result;

        // destroys exisiting scoreboard elements
        foreach (Transform child in scoreboardContent.transform)
        {
            Destroy(child.gameObject);
        }

        // loop through all users 
        foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
        {
            // retrieve username and high score
            string username = childSnapshot.Child("username").Value.ToString();
            int highscore = int.Parse(childSnapshot.Child("daily challenge high score").Value.ToString());

            // creates new leaderboard entry for each user
            GameObject scoreboardElement = Instantiate(scoreElement, scoreboardContent);
            scoreboardElement.GetComponent<Leaderboard>().NewScoreElement(username, highscore);
        }
    }
}



