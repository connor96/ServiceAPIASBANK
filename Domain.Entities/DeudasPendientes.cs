namespace Domain.Entities
{
    public class DeudasPendientes
    {
        public string CodigoProducto { get; set; }
        public string NumDocumento { get; set; }
        public string DescDocumento { get; set; }
        public string FechaVencimiento { get; set; }
        public string FechaEmision { get; set; }
        public double Deuda { get; set; }
        public double Mora { get; set; }
        public double GastosAdm { get; set; }
        public double PagoMinimo { get; set; }
        public string Periodo { get; set; }
        public int Anio { get; set; }
        public string Cuota { get; set; }
        public string MonedaDoc { get; set; }
    }
}
