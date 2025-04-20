﻿using Azure.Core;
using MediatR;
using Mind_Map.Models;

public class RegisterUserCommand : IRequest<int>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Gender { get; set; } = string.Empty;

}
