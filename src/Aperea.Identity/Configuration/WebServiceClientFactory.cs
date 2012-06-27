using System;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens;
using System.ServiceModel;
using System.ServiceModel.Security;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.Protocols.WSTrust.Bindings;

namespace Aperea.Identity.Configuration
{
    public class WebServiceClientFactory : IWebServiceClientFactory
    {
        readonly IRelyingPartyClientConfiguration configuration;

        readonly ConcurrentDictionary<Type, SecurityToken> securityTokens =
            new ConcurrentDictionary<Type, SecurityToken>();

        WebServiceClientFactory(IRelyingPartyClientConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public static IWebServiceClientFactory Create(IRelyingPartyClientConfiguration serviceConfiguration)
        {
            return new WebServiceClientFactory(serviceConfiguration);
        }

        public static IWebServiceClientFactory Create()
        {
            return Create(RelyingPartyClientConfiguration.Current);
        }

        public void CreateUserToken<T>(string username, string password)
        {
            var binding = new UserNameWSTrustBinding(SecurityMode.TransportWithMessageCredential);
            var factory = new WSTrustChannelFactory(binding, configuration.GetStsEndpoint());
            factory.Credentials.UserName.UserName = username;
            factory.Credentials.UserName.Password = password;
            factory.TrustVersion = TrustVersion.WSTrust13;
            var channel = factory.CreateChannel();
            var requestSecurityToken = new RequestSecurityToken
                                           {
                                               RequestType = WSTrust13Constants.RequestTypes.Issue,
                                               AppliesTo = configuration.GetEndpointFor<T>(),
                                               KeyType = WSTrust13Constants.KeyTypes.Symmetric,
                                               RequestDisplayToken = true
                                           };

            securityTokens.AddOrUpdate(typeof (T), t => channel.Issue(requestSecurityToken),
                                       (t, st) => channel.Issue(requestSecurityToken));
        }

        public T CreateChannel<T>()
        {
            var binding = new WS2007FederationHttpBinding(WSFederationHttpSecurityMode.TransportWithMessageCredential);
            binding.Security.Message.EstablishSecurityContext = false;
            var factory = new ChannelFactory<T>(binding, configuration.GetEndpointFor<T>());
            factory.Credentials.SupportInteractive = false;
            factory.ConfigureChannelFactory();

            return factory.CreateChannelWithIssuedToken(securityTokens[typeof (T)]);
        }
    }
}