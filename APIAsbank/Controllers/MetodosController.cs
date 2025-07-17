using Domain.Entities;
using Infraestructure.UnitOfWorkProc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIAsbank.Controllers
{




    [Route("api/[controller]")]
    [ApiController]
    public class MetodosController : ControllerBase
    {

        //Variables constantes
        readonly string _monedaPago = "1";
        readonly string _codigoEmpresa = "727";
        readonly string _codigoProducto = "001";
        readonly string[] _canalesPago = { "10", "14", "20", "30", "40", "50","60","70","80","99" };
        readonly string[] _tipoConsulta = { "0", "1", "2" };
        readonly string[] _bancos = { "1020", "1022", "1024", "1023", "1021", "1026", "1025" };
        readonly string[] _formasPago = { "01", "02", "03", "04", "05", "06", "07", "08" };


        //Llamada a UnitOfWork
        private IUnitOfWorkProc _unitOfWorkProc;

        //Variables
        SalidaValidarCliente _salidacliente;
        SalidaValidarCliente _salidaClienteResultado;

        SalidaConsultarDeuda _salidaConsulta;
        SalidaConsultarDeuda _salidaConsultaRevertir;

        SalidaPago _salidaPago;
        SalidaPago _salidaPagoRevertir;

        SalidaRevertirPago _salidaRevertir;
        SalidaRevertirPago _salidaRevertirRevertir;



        
        [HttpPost("ValidarCliente")]
        public async Task<ActionResult<string>> ValidarCliente(EntradaValidarCliente cliente)
        {
            _salidacliente = new SalidaValidarCliente();
            _salidacliente = VBasicasValidarCliente(cliente);

            if (_salidacliente.CodigoRespuesta is not null)
            {
                return Ok(_salidacliente);
            }
            else
            {
                _unitOfWorkProc = new UnitOfWorkProc();
                _salidacliente = new SalidaValidarCliente();

                //TODO


                _salidacliente = _unitOfWorkProc.metodosRepository.ValidarCliente(cliente);

                return Ok(_salidacliente);
            }


        }

        private SalidaValidarCliente VBasicasValidarCliente(EntradaValidarCliente cliente)
        {
            _salidaClienteResultado = new SalidaValidarCliente();
            if (cliente.CodigoEmpresa != _codigoEmpresa)
            {
                _salidaClienteResultado.CodigoRespuesta = "13";
                _salidaClienteResultado.DescripcionResp = "AFIL. EMPR.-SERVICIO NO EXISTE";
                _salidaClienteResultado.NombreCliente = "";
            }
            else if (_tipoConsulta.Contains(cliente.TipoConsulta) is false)
            {
                _salidaClienteResultado.CodigoRespuesta = "99";
                _salidaClienteResultado.DescripcionResp = "ERROR DESCONOCIDO";
                _salidaClienteResultado.NombreCliente = "";
            }
            else if (cliente.CodigoProducto != _codigoProducto)
            {
                _salidaClienteResultado.CodigoRespuesta = "13";
                _salidaClienteResultado.DescripcionResp = "AFIL. EMPR.-SERVICIO NO EXISTE";
                _salidaClienteResultado.NombreCliente = "";
            }

            return _salidaClienteResultado;
        }

       
        [HttpPost("ConsultarDeuda")]
        public async Task<ActionResult<string>> ConsultarDeuda(EntradaConsultarDeuda consultardeuda)
        {
            _unitOfWorkProc = new UnitOfWorkProc();
            _salidaConsulta = new SalidaConsultarDeuda();

            _salidaConsulta = vBasicasConsultarDeuda(consultardeuda);

            if(_salidaConsulta.CodigoRespuesta is not null)
            {
                return Ok(_salidaConsulta);
            }
            else
            {
                if(consultardeuda.CodigoBanco=="1020")
                {
                     _salidaConsulta = _unitOfWorkProc.metodosRepository.ConsultarDeuda(consultardeuda);
                     return Ok(_salidaConsulta);
                }
                else if(consultardeuda.CodigoBanco=="1022")
                {
                    _salidaConsulta = _unitOfWorkProc.metodosRepository.ConsultarDeudaItbk(consultardeuda);
                    return Ok(_salidaConsulta);
                }
                else
                {
                    _salidaConsulta = _unitOfWorkProc.metodosRepository.ConsultarDeuda(consultardeuda);
                    return Ok(_salidaConsulta);
                }

            }
        }

        private SalidaConsultarDeuda vBasicasConsultarDeuda(EntradaConsultarDeuda consultardeuda)
        {
            _salidaConsultaRevertir = new SalidaConsultarDeuda();
            if (consultardeuda.CodigoProducto != _codigoProducto)
            {
                _salidaConsultaRevertir.CodigoRespuesta = "13";
                _salidaConsultaRevertir.DescripcionResp = "AFIL. EMPR.-SERVICIO NO EXISTE";
                _salidaConsultaRevertir.DeudasPendientes = [];
            }else if (_tipoConsulta.Contains(consultardeuda.TipoConsulta) is false)
            {
                _salidaConsultaRevertir.CodigoRespuesta = "99";
                _salidaConsultaRevertir.DescripcionResp = "ERROR DESCONOCIDO";
                _salidaConsultaRevertir.DeudasPendientes = [];
            }else if(_bancos.Contains(consultardeuda.CodigoBanco) is false){
                _salidaConsultaRevertir.CodigoRespuesta = "82";
                _salidaConsultaRevertir.DescripcionResp = "CODIGO DE BANCO INVÁLIDO";
                _salidaConsultaRevertir.DeudasPendientes = [];
            }else if(_canalesPago.Contains(consultardeuda.CanalPago) is false)
            {
                _salidaConsultaRevertir.CodigoRespuesta = "99";
                _salidaConsultaRevertir.DescripcionResp = "ERROR DESCONOCIDO";
                _salidaConsultaRevertir.DeudasPendientes = [];
            }else if (_codigoEmpresa != consultardeuda.CodigoEmpresa)
            {
                _salidaConsultaRevertir.CodigoRespuesta = "13";
                _salidaConsultaRevertir.DescripcionResp = "AFIL. EMPR.-SERVICIO NO EXISTE";
                _salidaConsultaRevertir.DeudasPendientes = [];
            }

            return _salidaConsultaRevertir;

        }

       
        [HttpPost("NotificarPago")]
        public async Task<ActionResult<string>> NotificarPago(EntradaPago pago)
        {
            _unitOfWorkProc = new UnitOfWorkProc();
            _salidaPago = new SalidaPago();

            _salidaPago = vBasicasNotificarPago(pago);
            
            if(_salidaPago.CodigoRespuesta is not null)
            {
                return Ok(_salidaPago);
            }
            else
            {
                try
                {
                    if (pago.CodigoBanco=="1020")
                    {
                        _salidaPago = _unitOfWorkProc.metodosRepository.NotificarPago(pago);
                        return Ok(_salidaPago);
                    }
                    else if(pago.CodigoBanco == "1022")
                    {
                        _salidaPago = _unitOfWorkProc.metodosRepository.NotificarPagoItbk(pago);
                        return Ok(_salidaPago);
                    }
                    else
                    {
                        _salidaPago = _unitOfWorkProc.metodosRepository.NotificarPago(pago);
                        return Ok(_salidaPago);
                    }

                    

                }
                catch (Exception ex)
                {
                    return BadRequest("Error durante el proceso de almacenamiento");
                }
            }
        }

        private SalidaPago vBasicasNotificarPago(EntradaPago pago)
        {
            _salidaPagoRevertir = new SalidaPago();
            if (pago.FechaTxn.Length != 8)
            {
                _salidaPagoRevertir.CodigoRespuesta = "99";
                _salidaPagoRevertir.NombreCliente = "";
                _salidaPagoRevertir.NumOperacionERP = "";
                _salidaPagoRevertir.DescripcionResp = "ERROR DESCONOCIDO";
            }else if (pago.HoraTxn.Length != 6)
            {
                _salidaPagoRevertir.CodigoRespuesta = "99";
                _salidaPagoRevertir.NombreCliente = "";
                _salidaPagoRevertir.NumOperacionERP = "";
                _salidaPagoRevertir.DescripcionResp = "ERROR DESCONOCIDO";
            }else if (_canalesPago.Contains(pago.CanalPago) is false)
            {
                _salidaPagoRevertir.CodigoRespuesta = "13";
                _salidaPagoRevertir.NombreCliente = "";
                _salidaPagoRevertir.NumOperacionERP = "";
                _salidaPagoRevertir.DescripcionResp = "AFIL. EMPR.-SERVICIO NO EXISTE";
            }else if (_bancos.Contains(pago.CodigoBanco) is false)
            {
                _salidaPagoRevertir.CodigoRespuesta = "82";
                _salidaPagoRevertir.NombreCliente = "";
                _salidaPagoRevertir.NumOperacionERP = "";
                _salidaPagoRevertir.DescripcionResp = "CODIGO DE BANCO INVÁLIDO";
            }else if (_formasPago.Contains(pago.FormaPago) is false)
            {
                _salidaPagoRevertir.CodigoRespuesta = "99";
                _salidaPagoRevertir.NombreCliente = "";
                _salidaPagoRevertir.NumOperacionERP = "";
                _salidaPagoRevertir.DescripcionResp = "ERROR DESCONOCIDO";
            }else if (_tipoConsulta.Contains(pago.TipoConsulta) is false)
            {
                _salidaPagoRevertir.CodigoRespuesta = "99";
                _salidaPagoRevertir.NombreCliente = "";
                _salidaPagoRevertir.NumOperacionERP = "";
                _salidaPagoRevertir.DescripcionResp = "ERROR DESCONOCIDO";
            }else if (_codigoProducto!=pago.CodigoProducto)
            {
                _salidaPagoRevertir.CodigoRespuesta = "13";
                _salidaPagoRevertir.NombreCliente = "";
                _salidaPagoRevertir.NumOperacionERP = "";
                _salidaPagoRevertir.DescripcionResp = "AFIL. EMPR.-SERVICIO NO EXISTE";
            }else if (_monedaPago!=pago.MonedaDoc)
            {
                _salidaPagoRevertir.CodigoRespuesta = "81";
                _salidaPagoRevertir.NombreCliente = "";
                _salidaPagoRevertir.NumOperacionERP = "";
                _salidaPagoRevertir.DescripcionResp = "MONEDA DE PAGO INVALIDO";
            }else if (_codigoEmpresa!=pago.CodigoEmpresa)
            {
                _salidaPagoRevertir.CodigoRespuesta = "13";
                _salidaPagoRevertir.NombreCliente = "";
                _salidaPagoRevertir.NumOperacionERP = "";
                _salidaPagoRevertir.DescripcionResp = "AFIL. EMPR.-SERVICIO NO EXISTE";
            }

            return _salidaPagoRevertir;



        }

        
        [HttpPost("RevertirPago")]
        public async Task<ActionResult<string>> RevertirPago(EntradaRevertirPago pagorevertir)
        {
            _unitOfWorkProc = new UnitOfWorkProc();
            _salidaRevertir = new SalidaRevertirPago();

            _salidaRevertir = vBasicasRevertirpago(pagorevertir);
            if(_salidaRevertir.CodigoRespuesta is not null)
            {
                return Ok(_salidaRevertir);
            }
            else
            {
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

        private SalidaRevertirPago vBasicasRevertirpago(EntradaRevertirPago pagorevertir)
        {
            _salidaRevertirRevertir= new SalidaRevertirPago();
            if(_bancos.Contains(pagorevertir.CodigoBanco) is false)
            {
                _salidaRevertirRevertir.CodigoRespuesta = "82";
                _salidaRevertirRevertir.NombreCliente = "";
                _salidaRevertirRevertir.NumOperacionERP = "";
                _salidaRevertirRevertir.DescripcionResp = "CODIGO DE BANCO INVALIDO";
            }else if (_codigoEmpresa != pagorevertir.CodigoEmpresa)
            {
                _salidaRevertirRevertir.CodigoRespuesta = "13";
                _salidaRevertirRevertir.NombreCliente = "";
                _salidaRevertirRevertir.NumOperacionERP = "";
                _salidaRevertirRevertir.DescripcionResp = "AFIL.EMPR.- SERVICIO NO EXISTE";
            }

            return _salidaRevertirRevertir;
        }
    }
}
