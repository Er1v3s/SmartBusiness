﻿namespace AccountService.Contracts.Exceptions.Users
{
    public class UserNotFoundException(string message = "User not found") : Exception(message) { }
}
