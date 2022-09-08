using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.ServerWide.Operations;
using Raven.Client.ServerWide;
using Raven.Client.Exceptions.Database;
using System.Security.Cryptography.X509Certificates;
using Raven.Client.Documents.Session;

namespace backend
{
    public class RavenDB
    {
        private readonly IDocumentStore _store;
        private IConfigurationSection _config;

        public RavenDB(IConfigurationSection config)
        {
            _config = config;
            _store = new DocumentStore()
            {
                Database = _config["Name"],
                Urls = new[] { _config["URL"] },
                Certificate = new X509Certificate2(_config["CertificateLocation"])
            };

            _store.Initialize();
        }

        public void MakeSureExists()
        {
            try
            {   
                // try to get info, if we want get info then we dont have the database
                _store.Maintenance.ForDatabase(_config["Name"]).Send(new GetStatisticsOperation());
            }
            catch (DatabaseDoesNotExistException)
            {
                // create db
                _store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(_config["Name"])));
            }
        }

        public IDocumentSession createSession()
        {
            return _store.OpenSession();
        }
    }
}
