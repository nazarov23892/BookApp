using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using ServiceLayer.Abstract;
using System.Security.Claims;
using ServiceLayer.Interfaces;

namespace WebApplication.Infrastructure
{
    public class SignInContext : ISignInContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public SignInContext(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public bool IsSignedIn 
        { 
            get => httpContextAccessor?.HttpContext?.User
                .Identity.IsAuthenticated ?? false; 
        }

        public string UserId 
        { 
            get => httpContextAccessor?.HttpContext?.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
