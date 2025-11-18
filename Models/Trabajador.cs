namespace MantenimientoTrabajadores.Models
{
    public class Trabajador
    {
        public int Id { get; set; }

        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }

        public string? TipoDocumento { get; set; }
        public string? NumeroDocumento { get; set; }

        public string? Sexo { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public string? Foto { get; set; }
        public string? Direccion { get; set; }
    }
}
