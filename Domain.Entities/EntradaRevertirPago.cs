namespace Domain.Entities 
{ 
    public class EntradaRevertirPago
    {
        public string FechaTxn { get; set; }
        public string HoraTxn { get; set; }
        public string CodigoBanco { get; set; }
        public string TipoConsulta { get; set; }
        public string IdConsulta { get; set; }
        public string NumOperacionBanco { get; set; }
        public string NumDocumento { get; set; }
        public string CodigoEmpresa { get; set; }
    }
}
