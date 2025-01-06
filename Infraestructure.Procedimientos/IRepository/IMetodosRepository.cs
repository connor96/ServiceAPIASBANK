using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Procedimientos.IRepository
{
    public interface IMetodosRepository
    {
        SalidaValidarCliente ValidarCliente(EntradaValidarCliente cliente);
    }
}
