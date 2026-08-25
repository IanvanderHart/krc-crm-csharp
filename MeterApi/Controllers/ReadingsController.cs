using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MeterApi.Data;
using MeterApi.Models;

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
	try {
        Console.WriteLine($"Received: {System.Text.Json.JsonSerializer.Serialize(reading)}");
        if (!ModelState.IsValid) return BadRequest(ModelState);

	Console.WriteLine($"Received: {System.Text.Json.JsonSerializer.Serialize(reading)}");

        await _context.Readings.AddAsync(reading);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetReadings), new { id = reading.Id }, reading);
    }
	catch (Exception ex)
	{
	    Console.WriteLine($"Error: {ex.Message}");
	    throw;
	}
	}
}

