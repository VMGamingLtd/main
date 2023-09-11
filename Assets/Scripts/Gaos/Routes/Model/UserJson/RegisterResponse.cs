﻿#pragma warning disable 8632

namespace Gaos.Routes.Model.UserJson
{
    public enum RegisterResponseErrorKind
    {
        UsernameExistsError,
        UserNameIsEmptyError,
        EmailIsEmptyError,
        IncorrectEmailError,
        EmailExistsError,
        PasswordIsEmptyError,
        PasswordsDoNotMatchError,
        InternalError,
    };

    [System.Serializable]
    public class RegisterResponse
    {
        public bool? IsError;
        public string? ErrorMessage;

        public RegisterResponseErrorKind? ErrorKind;

        public Dbo.Model.User? User;

        public string? Jwt;
    }
}
