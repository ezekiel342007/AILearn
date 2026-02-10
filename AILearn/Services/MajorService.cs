using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using AILearn.Models;
using Avalonia.Platform;

namespace AILearn.Services;

public class MajorService
{
    // Static cache so we only read the file once per app session
    private static List<Major>? _cachedMajors;

    public async Task<List<Major>> GetMajorsAsync()
    {
        // 1. If we already loaded it, return the cache (Instant!)
        if (_cachedMajors != null) return _cachedMajors;

        try 
        {
            // 2. Read the file from Assets
            var uri = new Uri("avares://AILearn/Assets/majors.json");
            using var stream = AssetLoader.Open(uri);
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();

            // 3. Deserialize
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _cachedMajors = JsonSerializer.Deserialize<List<Major>>(json, options);
            
            return _cachedMajors ?? new List<Major>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($" [Error] Error loading majors: {ex.Message}");
            return new List<Major>(); // Return empty list on error
        }
    }
}