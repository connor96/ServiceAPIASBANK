using Infraestructure.Procedimientos.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.UnitOfWorkProc
{
    public interface IUnitOfWorkProc:IDisposable
    {
        IMetodosRepository  metodosRepository { get; }
    }
}
