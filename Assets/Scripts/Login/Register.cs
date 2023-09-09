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

}
