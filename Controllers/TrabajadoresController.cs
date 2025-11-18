using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using MantenimientoTrabajadores.Data;
using MantenimientoTrabajadores.Models;

namespace MantenimientoTrabajadores.Controllers
{
    public class TrabajadoresController : Controller
    {
        private readonly AppDbContext _context;

        public TrabajadoresController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // LISTAR (CON FILTRO)
        [HttpGet]
        public async Task<IActionResult> Listar(string sexo)
        {
            var trabajadores = await _context.Trabajadores
                .FromSqlRaw("EXEC sp_ListarTrabajadoresFiltrado @p0",
                    string.IsNullOrEmpty(sexo) ? DBNull.Value : sexo)
                .ToListAsync();

            return Json(new { data = trabajadores });
        }

        // GET CREAR
        public IActionResult Crear()
        {
            return PartialView("_Crear");
        }

        // POST CREAR
        [HttpPost]
        public async Task<IActionResult> Crear(Trabajador trabajador, IFormFile? FotoArchivo)
        {
            if (!ModelState.IsValid)
                return BadRequest("Datos no válidos");

            try
            {
                // FOTO
                string? nombreFoto = null;
                if (FotoArchivo != null && FotoArchivo.Length > 0)
                {
                    string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/fotos");
                    if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);

                    nombreFoto = Guid.NewGuid().ToString() + Path.GetExtension(FotoArchivo.FileName);
                    string ruta = Path.Combine(carpeta, nombreFoto);

                    using var stream = new FileStream(ruta, FileMode.Create);
                    await FotoArchivo.CopyToAsync(stream);
                }

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_RegistrarTrabajador @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7",
                    trabajador.Nombres ?? (object)DBNull.Value,
                    trabajador.Apellidos ?? (object)DBNull.Value,
                    trabajador.TipoDocumento ?? (object)DBNull.Value,
                    trabajador.NumeroDocumento ?? (object)DBNull.Value,
                    trabajador.Sexo ?? (object)DBNull.Value,
                    trabajador.FechaNacimiento,
                    nombreFoto ?? (object)DBNull.Value,
                    trabajador.Direccion ?? (object)DBNull.Value
                );

                return Ok(new { message = "Trabajador registrado correctamente" });
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                return BadRequest("Ya existe un trabajador registrado con este número de documento.");
            }
            catch (SqlException)
            {
                return BadRequest("Error al registrar trabajador.");
            }
        }

        // GET EDITAR
        public IActionResult Editar(int id)
        {
            var trabajador = _context.Trabajadores
                .FromSqlRaw("EXEC sp_ObtenerTrabajador {0}", id)
                .AsEnumerable()
                .FirstOrDefault();

            if (trabajador == null)
                return NotFound();

            return PartialView("_Editar", trabajador);
        }

        // POST EDITAR
        [HttpPost]
        public async Task<IActionResult> Editar(Trabajador trabajador, IFormFile? FotoArchivo)
        {
            if (!ModelState.IsValid)
                return BadRequest("Datos no válidos");

            try
            {
                string? nombreFoto = trabajador.Foto;

                // ¿Subió nueva foto?
                if (FotoArchivo != null && FotoArchivo.Length > 0)
                {
                    string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/fotos");
                    if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);

                    nombreFoto = Guid.NewGuid().ToString() + Path.GetExtension(FotoArchivo.FileName);
                    string ruta = Path.Combine(carpeta, nombreFoto);

                    using var stream = new FileStream(ruta, FileMode.Create);
                    await FotoArchivo.CopyToAsync(stream);
                }

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_ActualizarTrabajador @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8",
                    trabajador.Id,
                    trabajador.Nombres ?? (object)DBNull.Value,
                    trabajador.Apellidos ?? (object)DBNull.Value,
                    trabajador.TipoDocumento ?? (object)DBNull.Value,
                    trabajador.NumeroDocumento ?? (object)DBNull.Value,
                    trabajador.Sexo ?? (object)DBNull.Value,
                    trabajador.FechaNacimiento,
                    nombreFoto ?? (object)DBNull.Value,
                    trabajador.Direccion ?? (object)DBNull.Value
                );

                return Ok(new { message = "Trabajador actualizado correctamente" });
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                return BadRequest("Ya existe un trabajador registrado con este número de documento.");
            }
            catch (SqlException)
            {
                return BadRequest("Error al actualizar trabajador.");
            }
        }

        // GET ELIMINAR
        public IActionResult Eliminar(int id)
        {
            var trabajador = _context.Trabajadores
                .FromSqlRaw("EXEC sp_ObtenerTrabajador {0}", id)
                .AsEnumerable()
                .FirstOrDefault();

            if (trabajador == null)
                return NotFound();

            return PartialView("_Eliminar", trabajador);
        }

        // POST ELIMINAR
        [HttpPost]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC sp_EliminarTrabajador {0}", id);
            return Ok(new { message = "Trabajador eliminado correctamente" });
        }
    }
}
