using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Register : MonoBehaviour
{

    public Animation EmailErrorAnim;
    public Animation UsernameErrorAnim;
    public Animation PasswordErrorAnim;



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
