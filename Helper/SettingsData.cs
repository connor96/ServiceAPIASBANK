using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public class SettingsData
    {
        public string _CadenaConeccion=null;

        public SettingsData()
        {

            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

            configurationBuilder.AddJsonFile(path, false);

            var root = configurationBuilder.Build();

            string _ConnectionString = root.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            _CadenaConeccion = _ConnectionString;
        }

    }
}
