using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Text.Encodings.Web;

namespace cursework.Models;

public class Collection
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public HashSet<Film> Films { get; set; } = new();

    public Result Add(Film film)
    {
        if (this.Films.Add(film))
        {
            return Result.Success();
        }

        return Result.Failure($"Can't add Film \"{film}\" twice.");
    }
    public Result Remove(string title)
    {
        Film? film = this.Films.FirstOrDefault((film) => film.Title == title);
        if (film != null)
        {
            this.Films.Remove(film);
            return Result.Success();
        }

        return Result.Failure($"Can't find Film with title \"{title}\".");
    }

    public HashSet<Film> Search(string searchTerm)
    {
        
        return this.Films.Where(film =>
            (film.Title?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (film.Description?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (film.Studio?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (film.ReleaseDate?.ToString().Contains(searchTerm) ?? false) ||
            (film.Director?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (film.Actors?.Any(actor => actor.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ?? false) ||
            (film.Genres?.Any(genre => genre.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ?? false) ||
            (film.Rating?.Rate.ToString().Contains(searchTerm) ?? false) ||
            (film.FilePath?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (film.FileSize?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (film.Length?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false)
        ).ToHashSet();
    }
    
    public static Result Serialize(Collection collection, string path, string name)
    {
        try
        {
            var options =  new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, 
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.KebabCaseLower) },
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
        
            string jsonString = JsonSerializer.Serialize(collection, options);
            File.WriteAllText(Path.Combine(path, name), jsonString);
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e);
        }
    }
    public static Result<Collection?, object?> Deserialize(string path)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.KebabCaseLower) },
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
            
            string jsonString = File.ReadAllText(path);
            Collection? collection = JsonSerializer.Deserialize<Collection>(jsonString, options);
            return Result<Collection?, object?>.Success(collection);
        }
        catch (Exception e)
        {
            return Result<Collection?, object?>.Failure(e);
        }
    }
}