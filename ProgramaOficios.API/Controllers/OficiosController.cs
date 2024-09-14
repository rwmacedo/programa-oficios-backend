using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgramaOficios.Application.Entities;
using ProgramaOficios.Application.Interfaces.Services;
using ProgramaOficios.Infrastructure.Context;

[Route("api/[controller]")]
[ApiController]
public class OficiosController : ControllerBase
{
    private readonly OficioDbContext _context;
    private readonly IOficioService _oficioService;

    public OficiosController(OficioDbContext context, IOficioService oficioService)
    {
        _context = context;
        _oficioService = oficioService;
    }

    // GET: api/Oficios/{id} - Usando o serviço para obter por ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var oficio = await _oficioService.GetByIdAsync(id);
        if (oficio == null)
            return NotFound();

        return Ok(oficio);
    }

    // GET: api/Oficios - Usando o contexto para obter todos os ofícios
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Oficio>>> GetOficios()
    {
        try
        {
            return await _context.Oficios.ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao obter ofícios: {ex.Message}");
            return StatusCode(500, $"Erro ao obter ofícios: {ex.Message}");
        }
    }

    // POST: api/Oficios - Adicionar um novo ofício
    [HttpPost]
    public async Task<ActionResult<Oficio>> PostOficio(Oficio oficio)
    {
        // Supondo que 'oficio.Data' seja o campo com problema
     if (oficio.Data.Kind == DateTimeKind.Unspecified)
     {
         oficio.Data = DateTime.SpecifyKind(oficio.Data, DateTimeKind.Utc);
     }

        _context.Oficios.Add(oficio);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = oficio.Id }, oficio);
    }

    // PUT: api/Oficios/{id} - Atualizar um ofício existente
    [HttpPut("{id}")]
    public async Task<IActionResult> PutOficio(int id, Oficio oficio)
    {
        if (id != oficio.Id)
        {
            return BadRequest();
        }
       // Supondo que 'oficio.Data' seja o campo com problema
       if (oficio.Data.Kind == DateTimeKind.Unspecified)
       {
           oficio.Data = DateTime.SpecifyKind(oficio.Data, DateTimeKind.Utc);
       }

        _context.Entry(oficio).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!OficioExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Oficios/{id} - Remover um ofício
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOficio(int id)
    {
        var oficio = await _context.Oficios.FindAsync(id);
        if (oficio == null)
        {
            return NotFound();
        }

        _context.Oficios.Remove(oficio);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Verificar se o ofício existe
    private bool OficioExists(int id)
    {
        return _context.Oficios.Any(e => e.Id == id);
    }
}
