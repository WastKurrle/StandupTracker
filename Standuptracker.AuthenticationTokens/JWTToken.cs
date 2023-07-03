﻿using StandupTracker.Database.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Standuptracker.AuthenticationTokens.Dtos;
using System.Diagnostics;

namespace Standuptracker.AuthenticationTokens;

public class JWTToken
{
    // JWT Configuration
    private static readonly string keyString = "StandupTrackerJWTSecretTokenKeyForAuthentication";

    private static readonly SymmetricSecurityKey Key = 
        new(Encoding.UTF8.GetBytes(keyString));

    private static readonly SigningCredentials Creds = 
        new(Key, SecurityAlgorithms.HmacSha256);


    public static string CreateToken(User user)
    {
        var claims = new[]
        {
            new Claim("id", user.Id.ToString()),
            new Claim("username", user.Username)
        };

        var token = new JwtSecurityToken(
            issuer: "StandupTracker",
            audience: "User",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: Creds
        );  

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static void GetLoggedInUserFromToken(string tokenString)
    {
        LoggedInUser loggedInUser = new("", "");

        JwtSecurityTokenHandler tokenHandler = new();

        JwtSecurityToken token = tokenHandler.ReadJwtToken(tokenString);

        IEnumerable<Claim> claims = token.Claims;

        foreach (Claim claim in claims)
        {
            Debug.WriteLine(claim);
        }
    }
}
