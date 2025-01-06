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
                    _salidaCliente.DescripcionResp = "NO EXISTE";
                    _salidaCliente.NombreCliente = "NO EXISTE";
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

      
    }
}
