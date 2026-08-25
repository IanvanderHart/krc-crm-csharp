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
    public async Task<IActionResult> CreateReading([FromBody] Reading reading)
    {
        try
        {
            // Логируем сырой JSON
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            Console.WriteLine($"Raw JSON: {json}");

            // Десериализуем вручную с нечувствительностью к регистру
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var readingFromJson = JsonSerializer.Deserialize<Reading>(json, options);

            if (readingFromJson == null)
            {
                Console.WriteLine("Deserialization returned null");
                return BadRequest("Invalid JSON");
            }

            Console.WriteLine($"Deserialized: {JsonSerializer.Serialize(readingFromJson)}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid");
                return BadRequest(ModelState);
            }

            await _context.Readings.AddAsync(readingFromJson);
            await _context.SaveChangesAsync();

            Console.WriteLine($"Saved successfully. Id: {readingFromJson.Id}");
            return CreatedAtAction(nameof(GetReadings), new { id = readingFromJson.Id }, readingFromJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in CreateReading: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
