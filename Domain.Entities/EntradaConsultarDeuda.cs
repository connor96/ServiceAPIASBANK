namespace Domain.Entities { 
    public class EntradaConsultarDeuda
    {
        public string CodigoProducto { get; set; }
        public string TipoConsulta { get; set; }
        public string IdConsulta { get; set; }
        public string CodigoBanco { get; set; }
        public string CanalPago { get; set; }
        public string CodigoEmpresa { get; set; }
    }
}
