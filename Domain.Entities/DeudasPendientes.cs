namespace Domain.Entities
{
    public class DeudasPendientes
    {
        public string CodigoProducto { get; set; }
        public string NumDocumento { get; set; }
        public string DescDocumento { get; set; }
        public string FechaVencimiento { get; set; }
        public string FechaEmision { get; set; }
        public string Deuda { get; set; }
        public string Mora { get; set; }
        public string GastosAdm { get; set; }
        public string PagoMinimo { get; set; }
        public string Periodo { get; set; }
        public int Anio { get; set; }
        public string Cuota { get; set; }
        public string MonedaDoc { get; set; }
    }
}
