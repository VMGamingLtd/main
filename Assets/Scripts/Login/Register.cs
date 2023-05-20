using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Register : MonoBehaviour
{
    public readonly static string CLASS_NAME = typeof(Register).Name;

    public Animation EmailErrorAnim;
    public Animation UsernameErrorAnim;
    public Animation PasswordErrorAnim;

    public TMP_InputField EmailInputField;
    public TMP_InputField UserNameInputField;
    public TMP_InputField PasswordInputField;



    public void RegisterPlayer()
    {
        string email = Email.email;
        string username = UserName.userName;
        string password = Password.password;


         if (email.Length < 3)
         {

         }
         else if (username.Length < 2)
         {

         }
         else if (password.Length < 2)
         {

         }
    }

    public void OnButtonClick() {
        StartCoroutine(RegisterUser());
    }

    private IEnumerator RegisterUser() 
    { 
        yield return new WaitUntil(() => Gaos.Device.Manager.Registration.IsDeviceRegistered == true);
        StartCoroutine(Gaos.User.Manager.UserRegister.Register(EmailInputField.text, UserNameInputField.text, PasswordInputField.text, OnUserRegisterComplete));


    }
    public void OnUserRegisterComplete()
    {
        const string METHOD_NAME = "OnUserRegisterComplete()";

        if (Gaos.User.Manager.UserRegister.IsRegistered == true) {
            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: User registered: {Gaos.User.Manager.UserRegister.RegisterResponse.jwt}");
        } 
        else
        {
            throw new System.Exception("User registration failed");
        }

    }
     
    

}
