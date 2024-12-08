namespace Fase1PED.Models
{
    public class ClienteClusterViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Score { get; set; }
        public decimal Saldo { get; set; }
        public int TiempoEmpresa { get; set; }
        public string Cluster { get; set; }
        public string TipoTarjeta { get; set; }
        public bool PagoPendiente { get; set; }
        public string ClusterDetallado { get; set; } // Nueva propiedad para el cluster avanzado
    }
}