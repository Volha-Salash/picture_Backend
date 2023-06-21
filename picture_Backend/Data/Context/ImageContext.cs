using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;
using picture_Backend.Models;

namespace picture_Backend.Data.Context
{
    public class ImageContext
    {
        private ConnectionStringOptions connectionStringOptions;

        public ImageContext(IOptionsMonitor<ConnectionStringOptions> optionsMonitor)
        {
            connectionStringOptions=optionsMonitor.CurrentValue;
        }
        public IDbConnection CreateConnection() => new SqlConnection(connectionStringOptions.SqlConnection);
    }
    
}