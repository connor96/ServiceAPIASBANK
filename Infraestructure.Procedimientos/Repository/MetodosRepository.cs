using Domain.Entities;
using Helper;
using Infraestructure.Procedimientos.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Infraestructure.Procedimientos.Repository
{
    public class MetodosRepository : IMetodosRepository
    {

        //Configuraciones
        SecurityManager _manager = new SecurityManager();
        SettingsData _set;

        //Variables
        SalidaValidarCliente _salidaCliente;
        SalidaConsultarDeuda _salidaConsultarDeuda;
        List<DeudasPendientes> _listaDeudas;
        DeudasPendientes _deudaPendienteObjeto;
        DeudasPendientes _deudaPendienteAuxiliar;
        SalidaPago _salidaPago;
        SalidaRevertirPago _salidaRevertirPago;

        
        public SalidaValidarCliente ValidarCliente(EntradaValidarCliente cliente)
        {
            _set = new SettingsData();
            _salidaCliente = new SalidaValidarCliente();
            try
            {
                
                string resultado = "";
                using (SqlConnection _connection = new SqlConnection(_set._CadenaConeccion))
                {
                    if (ConnectionState.Closed == _connection.State)
                    {
                        _connection.Open();
                    }

                    SqlCommand comand = new SqlCommand("Intranet.sp_validarCliente", _connection);
                    comand.CommandType = CommandType.StoredProcedure;
                    comand.Parameters.Add("@tipoConsulta", SqlDbType.Int).Value = cliente.TipoConsulta;
                    comand.Parameters.Add("@idConsulta", SqlDbType.VarChar, 20).Value = cliente.IdConsulta;

                    SqlDataReader sqldataReader = comand.ExecuteReader();



                    if (sqldataReader.Read())
                    {
                        resultado = sqldataReader["nomApellidos"].ToString();
                        //_salidaCliente.NombreCliente = sqldataReader["nomApellidos"].ToString();

                    }

                    _connection.Close();

                }


                //Construccion del objeto
                if (resultado != "")
                {
                    _salidaCliente.CodigoRespuesta = "00";
                    _salidaCliente.DescripcionResp = "OK";
                    _salidaCliente.NombreCliente = resultado;

                }
                else
                {
                    _salidaCliente.CodigoRespuesta = "16";
                    _salidaCliente.DescripcionResp = "CLIENTE NO EXISTE";
                    _salidaCliente.NombreCliente = "CLIENTE NO EXISTE";
                }


                return _salidaCliente;
            }
            catch (Exception e)
            {

                _salidaCliente.CodigoRespuesta = "99";
                _salidaCliente.DescripcionResp = "ERROR";
                _salidaCliente.NombreCliente = e.Message;
                return _salidaCliente;
            }

            
        }

        public SalidaConsultarDeuda ConsultarDeuda(EntradaConsultarDeuda entradaConsultarDeuda)
        {
            _set = new SettingsData();
            _salidaConsultarDeuda = new SalidaConsultarDeuda();
           
            try
            {

                string resultado = "";
                using (SqlConnection _connection = new SqlConnection(_set._CadenaConeccion))
                {
                    if (ConnectionState.Closed == _connection.State)
                    {
                        _connection.Open();
                    }

                    SqlCommand comand = new SqlCommand("Intranet.sp_consultaDeudaCabecera", _connection);
                    comand.CommandType = CommandType.StoredProcedure;
                    comand.Parameters.Add("@tipoConsulta", SqlDbType.Int).Value = entradaConsultarDeuda.TipoConsulta;
                    comand.Parameters.Add("@idConsulta", SqlDbType.VarChar, 20).Value = entradaConsultarDeuda.IdConsulta;
                    comand.Parameters.Add("@canalpago", SqlDbType.VarChar, 2).Value = entradaConsultarDeuda.CanalPago;

                    SqlDataReader sqldataReader = comand.ExecuteReader();



                    if (sqldataReader.Read())
                    {
                        resultado = sqldataReader["codigo"].ToString();
                        //_salidaCliente.NombreCliente = sqldataReader["nomApellidos"].ToString();

                    }

                    _connection.Close();

                }

                //Consulta de Deudas


                //Construccion del objeto
                if (resultado != "")
                {

                    _salidaConsultarDeuda.DeudasPendientes = (List<DeudasPendientes>)_ListaDeudas(entradaConsultarDeuda);

                    if (_salidaConsultarDeuda.DeudasPendientes.Count()< 1)
                    {
                        _salidaConsultarDeuda.CodigoRespuesta = "22";
                        _salidaConsultarDeuda.DescripcionResp = "CLIENTE SIN DEUDA PENDIENTE";
                        _salidaConsultarDeuda.DeudasPendientes = [];
                    }
                    else
                    {
                        _salidaConsultarDeuda.CodigoRespuesta = "00";
                        _salidaConsultarDeuda.DescripcionResp = "OK";
                        
                    }

                }
                else
                {
                    _salidaConsultarDeuda.CodigoRespuesta = "16";
                    _salidaConsultarDeuda.DescripcionResp = "CLIENTE NO EXISTE";
                    _salidaConsultarDeuda.DeudasPendientes = [];
                }

                return _salidaConsultarDeuda;
            }
            catch (Exception e)
            {

                _salidaConsultarDeuda.CodigoRespuesta = "99";
                _salidaConsultarDeuda.DescripcionResp = "ERROR "+e.Message;
                _salidaConsultarDeuda.DeudasPendientes = [];


                return _salidaConsultarDeuda;
            }

        }

        public IEnumerable<DeudasPendientes> _ListaDeudas(EntradaConsultarDeuda entradaConsultarDeuda)
        {
            _set = new SettingsData();
            _listaDeudas = new List<DeudasPendientes>();
            try
            {
                //string cPension, cOtros, cDescuento, cMatricula = "";
                DateTime fechaVencimientoTemporal,fechaEmisionTemporal;
                using (SqlConnection _connection2 = new SqlConnection(_set._CadenaConeccion))
                {
                    if (ConnectionState.Closed == _connection2.State)
                    {
                        _connection2.Open();
                    }

                    SqlCommand comand = new SqlCommand("Intranet.sp_consultaDeudaLista", _connection2);
                    comand.CommandType = CommandType.StoredProcedure;
                    comand.Parameters.Add("@tipoConsulta", SqlDbType.Int).Value = entradaConsultarDeuda.TipoConsulta;
                    comand.Parameters.Add("@idConsulta", SqlDbType.VarChar, 20).Value = entradaConsultarDeuda.IdConsulta;
                    comand.Parameters.Add("@canalpago", SqlDbType.VarChar, 2).Value = entradaConsultarDeuda.CanalPago;

                    SqlDataReader sqldataReader = comand.ExecuteReader();

                    while (sqldataReader.Read())
                    {
                        _deudaPendienteObjeto = new DeudasPendientes();
                        //cPension = sqldataReader["cPension"].ToString();
                        //cOtros = sqldataReader["cOtros"].ToString();
                        //cDescuento = sqldataReader["cDescuento"].ToString();
                        //cMatricula = sqldataReader["cMatricula"].ToString();

                        //if (cPension != "")
                        //{
                        //    _deudaPendienteObjeto.CodigoProducto = cPension;
                        //}else if(cOtros!="")
                        //{
                        //    _deudaPendienteObjeto.CodigoProducto = cOtros;
                        //}
                        //else if (cDescuento != "")
                        //{
                        //    _deudaPendienteObjeto.CodigoProducto = cDescuento;
                        //}
                        //else if (cMatricula != "")
                        //{
                        //    _deudaPendienteObjeto.CodigoProducto = cMatricula;
                        //}



                        _deudaPendienteObjeto.CodigoProducto = sqldataReader["codigoProducto"].ToString();
                        _deudaPendienteObjeto.NumDocumento = sqldataReader["numDocumento"].ToString();
                        _deudaPendienteObjeto.DescDocumento= sqldataReader["descDocumento"].ToString();

                        fechaVencimientoTemporal = DateTime.Parse(sqldataReader["fechaVencimiento"].ToString());
                        fechaEmisionTemporal= DateTime.Parse(sqldataReader["fechaEmision"].ToString());
                        _deudaPendienteObjeto.FechaVencimiento = validarComponente(fechaVencimientoTemporal.Day.ToString())+validarComponente(fechaVencimientoTemporal.Month.ToString())+ fechaVencimientoTemporal.Year.ToString();
                        _deudaPendienteObjeto.FechaEmision = validarComponente(fechaEmisionTemporal.Day.ToString()) + validarComponente(fechaEmisionTemporal.Month.ToString()) + fechaEmisionTemporal.Year.ToString();
                        _deudaPendienteObjeto.Deuda = string.Format("{0:0.00}",double.Parse(sqldataReader["deuda"].ToString()));
                        _deudaPendienteObjeto.Mora = string.Format("{0:0.00}", double.Parse("0.00")) ;
                        _deudaPendienteObjeto.GastosAdm = string.Format("{0:0.00}", double.Parse("0.00"));
                        _deudaPendienteObjeto.PagoMinimo= string.Format("{0:0.00}", double.Parse(sqldataReader["pagoMinimo"].ToString()));
                        _deudaPendienteObjeto.Periodo = "00";
                        _deudaPendienteObjeto.Anio = int.Parse(sqldataReader["anio"].ToString());
                        //_deudaPendienteObjeto.Cuota = string.Format("{0:0.00}", double.Parse("00"));
                        _deudaPendienteObjeto.Cuota = "00";
                        _deudaPendienteObjeto.MonedaDoc = "1";
                        _listaDeudas.Add(_deudaPendienteObjeto);

                    }
                    _connection2.Close();

                }

                if (_listaDeudas.Count < 0)
                {
                    _listaDeudas = _validarListaDeudas(_listaDeudas);
                }

                return _listaDeudas;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private List<DeudasPendientes> _validarListaDeudas(List<DeudasPendientes> listaDeudas)
        {

            _listaDeudas = new List<DeudasPendientes>();
            _deudaPendienteAuxiliar = new DeudasPendientes();
            foreach (var item in listaDeudas)
            {
                if (item.CodigoProducto == "10" || item.CodigoProducto == "17" || item.CodigoProducto == "31"|| item.CodigoProducto == "34"|| item.CodigoProducto == "84"|| item.CodigoProducto == "85")
                {
                    _deudaPendienteAuxiliar = item;
                }
            }

            //TODO:Implementar validacion de descuento

            return listaDeudas;


        }

        private string validarComponente(string v)
        {
            if (v.Length == 1)
            {
                v = "0" + v;
            }
            return v;
        }

        public SalidaPago NotificarPago(EntradaPago entradaPago)
        {

            _set = new SettingsData();
            _salidaPago=new SalidaPago();
            string codigoRespuesta = "0";
            try
            {
                using (SqlConnection connection = new SqlConnection(_set._CadenaConeccion))
                {
                    if (ConnectionState.Closed == connection.State)
                    {
                        connection.Open();
                    }

                    SqlCommand comand = new SqlCommand("[Intranet].[sp_notificarPago]", connection);
                    comand.CommandType = CommandType.StoredProcedure;
                    comand.Parameters.Add("@fechaTxn", SqlDbType.VarChar, 8).Value = entradaPago.FechaTxn;
                    comand.Parameters.Add("@horaTxn", SqlDbType.VarChar, 6).Value = entradaPago.HoraTxn;
                    comand.Parameters.Add("@canalPago", SqlDbType.VarChar, 2).Value = entradaPago.CanalPago;
                    comand.Parameters.Add("@codigoBanco", SqlDbType.VarChar, 4).Value = entradaPago.CodigoBanco;
                    comand.Parameters.Add("@numOperacionBanco", SqlDbType.VarChar, 12).Value = entradaPago.NumOperacionBanco;
                    comand.Parameters.Add("@formaPago", SqlDbType.VarChar, 2).Value = entradaPago.FormaPago;
                    comand.Parameters.Add("@tipoConsulta", SqlDbType.Int).Value = entradaPago.TipoConsulta;
                    comand.Parameters.Add("@idConsulta", SqlDbType.VarChar, 14).Value = entradaPago.IdConsulta;
                    comand.Parameters.Add("@numDocumento", SqlDbType.VarChar, 16).Value = entradaPago.NumDocumento;
                    comand.Parameters.Add("@importePagado", SqlDbType.Decimal).Value = entradaPago.ImportePagado;

                    SqlDataReader sqldataReader = comand.ExecuteReader();

                    

                    if (sqldataReader.Read())
                    {
                        _salidaPago.NombreCliente = sqldataReader["apNombres"].ToString();
                        _salidaPago.NumOperacionERP = sqldataReader["idTransaccion"].ToString();
                        codigoRespuesta= sqldataReader["respuestaServicio"].ToString();
                    }
                    connection.Close();

                }

                if (codigoRespuesta == "25")
                {
                    _salidaPago.CodigoRespuesta = "25";
                    _salidaPago.NombreCliente = "";
                    _salidaPago.NumOperacionERP = "";
                    _salidaPago.DescripcionResp = "MONTO DE PAGO INVALIDO";
                }else if(codigoRespuesta=="20")
                {
                    _salidaPago.CodigoRespuesta = "20";
                    _salidaPago.NombreCliente = "";
                    _salidaPago.NumOperacionERP = "";
                    _salidaPago.DescripcionResp = "CUOTA PAGADA YA CANCELADA";
                }
                else if(codigoRespuesta=="99")
                {
                    _salidaPago.CodigoRespuesta = "99";
                    _salidaPago.NombreCliente = "";
                    _salidaPago.NumOperacionERP = "";
                    _salidaPago.DescripcionResp = "BOLETA NO COINCIDE";
                }else if (_salidaPago.NumOperacionERP == "")
                {
                    if (_salidaPago.NombreCliente != "")
                    {
                        _salidaPago.CodigoRespuesta = "99";
                        _salidaPago.NombreCliente = "";
                        _salidaPago.NumOperacionERP = "";
                        _salidaPago.DescripcionResp = "ERROR" + _salidaPago.NombreCliente;
                    }
                    else
                    {
                        _salidaPago.CodigoRespuesta = "99";
                        _salidaPago.NombreCliente = "";
                        _salidaPago.NumOperacionERP = "";
                        _salidaPago.DescripcionResp = "ERROR GENERAL";
                    }
                }
                else
                {
                    _salidaPago.CodigoRespuesta = "00";
                    _salidaPago.DescripcionResp = "OK";
                }


                return _salidaPago;

            }
            catch (Exception e)
            {

                _salidaPago.CodigoRespuesta = "99";
                _salidaPago.NombreCliente = "";
                _salidaPago.NumOperacionERP = "";
                _salidaPago.DescripcionResp = "ERROR: " + e.Message;

                return _salidaPago;
            }

        }

        public SalidaRevertirPago RevertirPago(EntradaRevertirPago entradaRevertirPago)
        {
            _set = new SettingsData();
            _salidaRevertirPago = new SalidaRevertirPago();
            string resultadoCodigo = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(_set._CadenaConeccion))
                {
                    if (ConnectionState.Closed == connection.State)
                    {
                        connection.Open();
                    }

                    SqlCommand comand = new SqlCommand("[Intranet].[sp_TransaccionesAnuladas]", connection);
                    comand.CommandType = CommandType.StoredProcedure;
                    comand.Parameters.Add("@fechaTxn", SqlDbType.VarChar, 8).Value = entradaRevertirPago.FechaTxn;
                    comand.Parameters.Add("@horaTxn", SqlDbType.VarChar, 6).Value = entradaRevertirPago.HoraTxn;
                    comand.Parameters.Add("@codigoBanco", SqlDbType.VarChar, 4).Value = entradaRevertirPago.CodigoBanco;
                    comand.Parameters.Add("@tipoConsulta", SqlDbType.Int).Value = entradaRevertirPago.TipoConsulta;
                    comand.Parameters.Add("@idConsulta", SqlDbType.VarChar, 14).Value = entradaRevertirPago.IdConsulta;
                    comand.Parameters.Add("@numOperacionBanco", SqlDbType.VarChar, 12).Value = entradaRevertirPago.NumOperacionBanco;
                    comand.Parameters.Add("@numDocumento", SqlDbType.VarChar, 16).Value = entradaRevertirPago.NumDocumento;

                    SqlDataReader sqldataReader = comand.ExecuteReader();

                    if (sqldataReader.Read())
                    {
                        _salidaRevertirPago.NombreCliente = sqldataReader["apNombres"].ToString();
                        _salidaRevertirPago.NumOperacionERP = sqldataReader["idTransaccion"].ToString();
                        resultadoCodigo= sqldataReader["resultadoConsulta"].ToString();
                    }
                    connection.Close();

                }

                if (resultadoCodigo == "68")
                {
                    _salidaRevertirPago.CodigoRespuesta = "68";
                    _salidaRevertirPago.NombreCliente = "";
                    _salidaRevertirPago.NumOperacionERP = "";
                    _salidaRevertirPago.DescripcionResp = "TXN ORIG. A ANULAR NO EXISTE";

                }else if (_salidaRevertirPago.NumOperacionERP == "")
                {
                    if (_salidaRevertirPago.NombreCliente != "")
                    {
                        _salidaRevertirPago.CodigoRespuesta = "99";
                        _salidaRevertirPago.NombreCliente = "";
                        _salidaRevertirPago.NumOperacionERP = "";
                        _salidaRevertirPago.DescripcionResp = "ERROR" + _salidaRevertirPago.NombreCliente;
                    }
                    else
                    {
                        _salidaRevertirPago.CodigoRespuesta = "99";
                        _salidaRevertirPago.NombreCliente = "";
                        _salidaRevertirPago.NumOperacionERP = "";
                        _salidaRevertirPago.DescripcionResp = "ERROR GENERAL";
                    }
                }
                else
                {
                    _salidaRevertirPago.CodigoRespuesta = "00";
                    _salidaRevertirPago.DescripcionResp = "OK";
                }


                return _salidaRevertirPago;

            }
            catch (Exception e)
            {

                _salidaRevertirPago.CodigoRespuesta = "99";
                _salidaRevertirPago.NombreCliente = "";
                _salidaRevertirPago.NumOperacionERP = "";
                _salidaRevertirPago.DescripcionResp = "ERROR: " + e.Message;

                return _salidaRevertirPago;
            }
        }
    }
}
