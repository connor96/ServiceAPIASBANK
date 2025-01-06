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
            SalidaConsultarDeuda _salidaConsulta = new SalidaConsultarDeuda();
            try
            {
                //_context.Productos.Add(objeto);
                //await _context.SaveChangesAsync();
                //return Ok("Creado con éxio");

                return Ok(_salidaConsulta);

            }
            catch (Exception ex)
            {
                return BadRequest("Error durante el proceso de almacenamiento");
            }

        }

        [HttpPost("NotificarPago")]
        public async Task<ActionResult<string>> NotificarPago(EntradaPago pago)
        {
            SalidaPago _salidaPago = new SalidaPago();
            try
            {
                //_context.Productos.Add(objeto);
                //await _context.SaveChangesAsync();
                //return Ok("Creado con éxio");

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
            SalidaRevertirPago _salidaRevertir = new SalidaRevertirPago();
            try
            {
                //_context.Productos.Add(objeto);
                //await _context.SaveChangesAsync();
                //return Ok("Creado con éxio");

                return Ok(_salidaRevertir);

            }
            catch (Exception ex)
            {
                return BadRequest("Error durante el proceso de almacenamiento");
            }

        }


    }
}
