using Infraestructure.Procedimientos.IRepository;
using Infraestructure.Procedimientos.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.UnitOfWorkProc
{
    public class UnitOfWorkProc : IUnitOfWorkProc
    {
        public IMetodosRepository metodosRepository => new MetodosRepository();

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
