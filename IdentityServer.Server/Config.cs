using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace IdentityServer.Server {
    public class Config {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource> {
                new IdentityResources.OpenId(), new IdentityResources.Profile(),
                new IdentityResources.Email()
            };

        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource> {new ApiResource("api", "My Test Api")};

        public static IEnumerable<Client> GetClients() =>
            new List<Client> {new Client {
                ClientId = "native.hybrid",
                ClientName = "Native Client (Hybrid with PKCE)",
                RedirectUris = {"cn.lovelymother://callback"},
                PostLogoutRedirectUris = {"https://notused"},
                RequireClientSecret = false,
                AllowedGrantTypes = GrantTypes.Hybrid,
                RequirePkce = true,
                AllowedScopes = {"openid", "profile", "email", "api"},
                AllowOfflineAccess = true,
                RefreshTokenUsage = TokenUsage.ReUse
            }};
    }
}