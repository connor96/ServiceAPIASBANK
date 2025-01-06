namespace Domain.Entities { 
    public class EntradaPago
    {
        public string FechaTxn { get; set; }
        public string HoraTxn { get; set; }
        public string CanalPago { get; set; }
        public string CodigoBanco { get; set; }
        public string NumOperacionBanco { get; set; }
        public string FormaPago { get; set; }
        public string TipoConsulta { get; set; }
        public string IdConsulta { get; set; }
        public string CodigoProducto { get; set; }
        public string NumDocumento { get; set; }
        public double ImportePagado { get; set; }
        public string MonedaDoc { get; set; }
        public string CodigoEmpresa { get; set; }
    }
}
