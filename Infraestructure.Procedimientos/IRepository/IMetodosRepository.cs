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
        //Validar Cliente
        SalidaValidarCliente ValidarCliente(EntradaValidarCliente cliente);


        //Consultar Deuda
        SalidaConsultarDeuda ConsultarDeuda(EntradaConsultarDeuda entradaConsultarDeuda);
        IEnumerable<DeudasPendientes> _ListaDeudas(EntradaConsultarDeuda entradaConsultarDeuda);


        SalidaConsultarDeuda ConsultarDeudaItbk(EntradaConsultarDeuda entradaConsultarDeuda);
        IEnumerable<DeudasPendientes> _ListaDeudasItbk(EntradaConsultarDeuda entradaConsultarDeuda);



        //Registrar pago
        SalidaPago NotificarPago(EntradaPago entradaPago);

        SalidaPago NotificarPagoItbk(EntradaPago entradaPago);




        //Extorno
        SalidaRevertirPago RevertirPago(EntradaRevertirPago entradaRevertirPago);


    }
}
