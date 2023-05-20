using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerCoroutineManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnable()
    {
        Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 1000");
        StartCoroutine(Gaos.Device.Manager.Registration.RegisterDevice());
        StartCoroutine(Gaos.User.Manager.GuestLogin.Login(OnGuestLoginComplete));
    }

    public void OnGuestLoginComplete()
    {
        if (Gaos.User.Manager.GuestLogin.IsLoggedIn == true)
        {
            Debug.Log($"Guest logged in: {Gaos.User.Manager.GuestLogin.GuestLoginResponse.userName}");
            UserName.userName = Gaos.User.Manager.GuestLogin.GuestLoginResponse.userName; 
        }
        else
        {
            throw new System.Exception("Guest login failed");
        }

    }

    public void RegisterUser(string email, string userName, string password)
    {
        StartCoroutine(Gaos.User.Manager.UserRegister.Register(email, userName, password, OnRegisterUserComplete));
    }

    public void OnRegisterUserComplete()
    {
        if (Gaos.User.Manager.UserRegister.IsRegistered == true)
        {
            Debug.Log($"User registered: {Gaos.User.Manager.UserRegister.RegisterResponse.jwt}");
        }
        else
        {
            throw new System.Exception("User registration failed");
        }
    }




}
