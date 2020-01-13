﻿using PrimoDesktopUI.Library.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace PrimoDesktopUI.Library.API
{
    public interface IAPIHelper
    {
        HttpClient ApiClient { get; }

        Task<AuthenticatedUser> Authenticate(string username, string password);

        Task GetLoggedInUserInfo(string token);
    }
}