using System.Data.Entity;
using Raven.Client;
using Raven.Client.Document;

namespace Aperea.Infrastructure.Data
{
    public interface IDocumentSessionFactory
    {
        IDocumentSession CreateDocumentSession();
    }
}