using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace cursework.Models;

public class Library
{
    public List<Collection> Collections { get; set; } = [];
    
    public List<Collection> Filtered(Dictionary<PropertyInfo, string> searchPairs)
    {
        List<Collection> filtered = [];

        foreach (var (prop, term) in searchPairs)
        {
            filtered.AddRange(this.Collections.Where(
                collection
                    =>
                prop.GetValue(collection)?.ToString()?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false
            ).ToList());
        }

        return filtered;
    }
    public List<Collection> Filtered(List<string> searchTerms)
    {
        var filtered = this.Collections;

        foreach (var term in searchTerms)
        {
            filtered = filtered.Where(collection =>
            {
                foreach (var prop in typeof(Collection).GetProperties())
                {
                    if (prop.GetValue(collection)?.ToString()?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false)
                    {
                        return true;
                    }
                }

                return false;
            }).ToList();
        }

        return filtered;
    }

    public void Add(Models.Collection collection)
    {
        this.Collections.Add(collection);
    }
    public Result Remove(string title)
    {
        Collection? collection = this.Collections.FirstOrDefault((collection) => collection.Title == title);
        if (collection != null)
        {
            this.Collections.Remove(collection);
            return Result.Success();
        }

        return Result.Failure($"Can't find Collection with title \"{title}\".");
    }

    public static Result Serialize(Library library, string path, string name)
    {
        return Library.Serialize(library, Path.Combine(path, name));
    }
    public static Result Serialize(Library library, string path)
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
        
            string jsonString = JsonSerializer.Serialize(library, options);
            File.WriteAllText(path, jsonString);
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e);
        }
    }
    public static Result<Library?, object?> Deserialize(string path)
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
            Library? library = JsonSerializer.Deserialize<Library>(jsonString, options);
            return Result<Library?, object?>.Success(library);
        }
        catch (Exception e)
        {
            return Result<Library?, object?>.Failure(e);
        }
    }
}