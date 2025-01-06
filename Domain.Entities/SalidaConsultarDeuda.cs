namespace Domain.Entities
{
    public class SalidaConsultarDeuda
    {
        public string CodigoRespuesta { get; set; }
        public string DescripcionResp { get; set; }
        public List<DeudasPendientes> DeudasPendientes { get; set; }
    }
}
