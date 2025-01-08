using Domain.Entities;
using Infraestructure.UnitOfWorkProc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIAsbank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetodosController : ControllerBase
    {
        //Llamada a UnitOfWork
        private IUnitOfWorkProc _unitOfWorkProc;

        //Variables
        SalidaValidarCliente _salidacliente;
        SalidaConsultarDeuda _salidaConsulta;
        SalidaPago _salidaPago;
        SalidaRevertirPago _salidaRevertir;

        [HttpPost("ValidarCliente")]
        public async Task<ActionResult<string>> ValidarCliente(EntradaValidarCliente cliente)
        {
            _unitOfWorkProc= new UnitOfWorkProc();
            _salidacliente = new SalidaValidarCliente();
            _salidacliente = _unitOfWorkProc.metodosRepository.ValidarCliente(cliente);

            return Ok(_salidacliente);


            //try
            //{
            //    //_context.Productos.Add(objeto);
            //    //await _context.SaveChangesAsync();
            //    //return Ok("Creado con éxio");

            //    return Ok(_salidacliente);

            //}
            //catch (Exception ex)
            //{
            //    return BadRequest("Error durante el proceso de almacenamiento");
            //}

        }



        [HttpPost("ConsultarDeuda")]
        public async Task<ActionResult<string>> ConsultarDeuda(EntradaConsultarDeuda consultardeuda)
        {
            _unitOfWorkProc = new UnitOfWorkProc();
            _salidaConsulta = new SalidaConsultarDeuda();
            _salidaConsulta = _unitOfWorkProc.metodosRepository.ConsultarDeuda(consultardeuda);

            return Ok(_salidaConsulta);

            //try
            //{
            //    //_context.Productos.Add(objeto);
            //    //await _context.SaveChangesAsync();
            //    //return Ok("Creado con éxio");

            //    return Ok(_salidaConsulta);

            //}
            //catch (Exception ex)
            //{
            //    return BadRequest("Error durante el proceso de almacenamiento");
            //}

        }

        [HttpPost("NotificarPago")]
        public async Task<ActionResult<string>> NotificarPago(EntradaPago pago)
        {
            _unitOfWorkProc = new UnitOfWorkProc();
            _salidaPago = new SalidaPago();
            try
            {
                _salidaPago = _unitOfWorkProc.metodosRepository.NotificarPago(pago);
                return Ok(_salidaPago);

            }
            catch (Exception ex)
            {
                return BadRequest("Error durante el proceso de almacenamiento");
            }

        }

        [HttpPost("RevertirPago")]
        public async Task<ActionResult<string>> RevertirPago(EntradaRevertirPago pagorevertir)
        {
            _unitOfWorkProc = new UnitOfWorkProc();
            _salidaRevertir = new SalidaRevertirPago();
            try
            {

                _salidaRevertir = _unitOfWorkProc.metodosRepository.RevertirPago(pagorevertir);

                return Ok(_salidaRevertir);

            }
            catch (Exception ex)
            {
                return BadRequest("Error durante el proceso de almacenamiento");
            }

        }


    }
}
