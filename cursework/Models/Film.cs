using System;
using System.Collections.Generic;
using System.Linq;

namespace cursework.Models;

public class Film
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Studio { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public string? Director { get; set; }
    public HashSet<string>? Actors { get; set; }
    public HashSet<Genre>? Genres { get; set; }
    public Rating? Rating { get; set; }
    public string? FilePath { get; set; }
    public string? FileSize { get; set; }
    public string? Length { get; set; }

    public override string? ToString()
    {
        return this.Title;
    }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        Film film = (Film)obj;

        bool title       = this.Title == film.Title;
        bool description = this.Description == film.Description;
        bool studio      = this.Studio == film.Studio;
        bool release     = this.ReleaseDate == film.ReleaseDate;
        bool director    = this.Director == film.Director;
        bool rating      = this.Rating == film.Rating;
        bool path        = this.FilePath == film.FilePath;
        bool size = this.FileSize == film.FileSize;
        bool lenght = this.Length == film.Length;
        bool actors = film.Actors != null
                ? this.Actors.SetEquals(film.Actors)
                : this.Actors == film.Actors
            ;
        bool geners = this.Genres != null && film.Genres != null
            ? this.Genres.SetEquals(film.Genres)
            : this.Genres == film.Genres;

        List<bool> list = [title, description, studio, release, director, rating, path, size, lenght, actors, geners];
        return list.All(n => n);
    }
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(this.Title);
        hash.Add(this.Description);
        hash.Add(this.Studio);
        hash.Add(this.ReleaseDate);
        hash.Add(this.Director);
        if (this.Actors != null)
        {
            foreach (var actor in this.Actors)
            {
                hash.Add(actor);
            }
        }
        if (this.Genres != null)
        {
            foreach (var genre in this.Genres)
            {
                hash.Add(genre);
            }
        }
        hash.Add(this.Rating);
        hash.Add(this.FilePath);
        hash.Add(this.FileSize);
        hash.Add(this.Length);
        return hash.ToHashCode();
    }
}