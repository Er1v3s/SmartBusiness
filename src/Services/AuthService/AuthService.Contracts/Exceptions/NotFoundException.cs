﻿namespace AuthService.Contracts.Exceptions
{
    public class NotFoundException(string message) : Exception(message);
}
