using Microsoft.AspNetCore.Mvc;
using Libreria.Models;
using System.Collections.Generic;
using System.Linq;

[Route("api/[controller]")]
[ApiController]
public class LibrosController : ControllerBase
{
    private static List<Libros> libros = new List<Libros>
    {
        new Libros { Id = 1, Title = "1984", Author = "George Orwell", Genre = "Dystopian", PublishedYear = 1949 },
        new Libros { Id = 2, Title = "To Kill a Mockingbird", Author = "Harper Lee", Genre = "Fiction", PublishedYear = 1960 }
    };

    [HttpGet]
    public ActionResult<IEnumerable<Libros>> GetLibros()
    {
        return Ok(libros);
    }

    [HttpGet("{id}")]
    public ActionResult<Libros> GetLibro(int id)
    {
        var libro = libros.FirstOrDefault(l => l.Id == id);
        if (libro == null)
            return NotFound($"Libro con ID {id} no encontrado.");
        return Ok(libro);
    }

    [HttpPost]
    public ActionResult<Libros> CreateLibro([FromBody] Libros libro)
    {
        if (string.IsNullOrWhiteSpace(libro.Title) || string.IsNullOrWhiteSpace(libro.Author) || string.IsNullOrWhiteSpace(libro.Genre))
            return BadRequest("Title, Author, y Genre son obligatorios.");

        if (libro.PublishedYear <= 0)
            return BadRequest("Published Year debe ser un número positivo.");

        libro.Id = libros.Max(l => l.Id) + 1;
        libros.Add(libro);
        return CreatedAtAction(nameof(GetLibro), new { id = libro.Id }, libro);
    }

    [HttpPut("{id}")]
    public ActionResult UpdateLibro(int id, [FromBody] Libros libro)
    {
        if (string.IsNullOrWhiteSpace(libro.Title) || string.IsNullOrWhiteSpace(libro.Author) || string.IsNullOrWhiteSpace(libro.Genre))
            return BadRequest("Title, Author, y Genre son obligatorios.");

        if (libro.PublishedYear <= 0)
            return BadRequest("Published Year debe ser un número positivo.");

        var existingLibro = libros.FirstOrDefault(l => l.Id == id);
        if (existingLibro == null)
            return NotFound($"Libro con ID {id} no encontrado.");

        existingLibro.Title = libro.Title;
        existingLibro.Author = libro.Author;
        existingLibro.Genre = libro.Genre;
        existingLibro.PublishedYear = libro.PublishedYear;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteLibro(int id)
    {
        var libro = libros.FirstOrDefault(l => l.Id == id);
        if (libro == null)
            return NotFound($"Libro con ID {id} no encontrado.");

        libros.Remove(libro);
        return NoContent();
    }
}
