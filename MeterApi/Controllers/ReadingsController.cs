using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MeterApi.Data;
using MeterApi.Models;
using System.Text.Json;

namespace MeterApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReadingsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReadingsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetReadings()
    {
        var readings = await _context.Readings.ToListAsync();
        return Ok(readings);
    }

    [HttpPost]
    public async Task<IActionResult> CreateReading()
    {
        try
        {
            // Читаем сырой JSON из тела запроса
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            Console.WriteLine($"Raw JSON: {json}");

            if (string.IsNullOrWhiteSpace(json))
            {
                Console.WriteLine("Empty JSON received");
                return BadRequest("Empty JSON");
            }

            // Десериализуем вручную
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var reading = JsonSerializer.Deserialize<Reading>(json, options);

            if (reading == null)
            {
                Console.WriteLine("Deserialization returned null");
                return BadRequest("Invalid JSON structure");
            }

            Console.WriteLine($"Deserialized: {JsonSerializer.Serialize(reading)}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState invalid");
                return BadRequest(ModelState);
            }

            await _context.Readings.AddAsync(reading);
            await _context.SaveChangesAsync();

            Console.WriteLine($"Saved successfully. Id: {reading.Id}");
            return CreatedAtAction(nameof(GetReadings), new { id = reading.Id }, reading);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON parse error: {ex.Message}");
            return BadRequest(new { error = "Invalid JSON format", detail = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
