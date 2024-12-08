namespace Fase1PED.Models
{
    public static class ClusterLogic
    {
        // Método para calcular el Score avanzado
        public static decimal CalcularScoreAvanzado(decimal salario, decimal? segundoIngreso, int tiempoAFP, int tiempoEmpresa)
        {
            const decimal pesoSalario = 0.5m;
            const decimal pesoSegundoIngreso = 0.2m;
            const decimal pesoTiempoAFP = 0.15m;
            const decimal pesoTiempoEmpresa = 0.15m;

            return ((salario * pesoSalario) +
                    ((segundoIngreso ?? 0) * pesoSegundoIngreso) +
                    (tiempoAFP * pesoTiempoAFP) +
                    (tiempoEmpresa * pesoTiempoEmpresa)) / 100000m;
        }

        // Método para categorizar usuarios en clusters según el Score
        public static string CalcularCluster(decimal score)
        {
            if (score >= 0.1m && score <= 0.9m)
                return "Cluster A (Top)";
            else if (score >= 0.04m && score < 0.1m)
                return "Cluster B (Medio)";
            else
                return "Cluster C (Bajo)";
        }
    }
}