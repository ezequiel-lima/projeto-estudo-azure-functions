using Microsoft.Azure.Cosmos;

namespace Organico.Library
{
    public abstract class BaseCosmosClient
    {
        protected CosmosClient _client;
        protected Database _database;
        protected Container _container;

        protected abstract string ContainerId { get; set; }

        public void InitializeAsync()
        {
            _client = new CosmosClient("https://cosmos-db-ezequiel.documents.azure.com:443/", "cbEok5GHdblrssuhQ6mYmbiGZxmrKJGP6oz0pddRjI4bn23GbzZ3ZuuH2bvRC8ORyjlW427ynoQ7ACDbpXzEKQ==");
            _database = _client.GetDatabase("organico");
            _container = _database.GetContainer(ContainerId);
        }
    }
}
