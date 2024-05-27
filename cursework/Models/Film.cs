using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PropertyModels.Collections;
using PropertyModels.ComponentModel;
using PropertyModels.ComponentModel.DataAnnotations;

namespace cursework.Models;

public class Film
{
    [DisplayName("Title")]
    [Watermark("Title")]
    public string Title { get; set; } = "";          
    
    [DisplayName("Description")]
    [Watermark("Description")]
    public string Description { get; set; } = "";
    
    [DisplayName("Studio")]
    [Watermark("Studio")]
    public string Studio { get; set; } = "";

    [DisplayName("Release Date")]
    [Watermark("Release Date")]
    public DateTime ReleaseDate { get; set; } = DateTime.UnixEpoch;     
    
    [DisplayName("Director")]
    [Watermark("Director")]
    public string Director { get; set; } = "";       
    public BindingList<string> Actors { get; set; } = [];
    public BindingList<Genre> Genres { get; set; } = [];
    
    [DisplayName("File Path")]
    [Watermark("File Path")]
    [PathBrowsable(Filters = "Video Files(*.mp4;*.mkv)|*.mp4;*.mkv")]
    public string FilePath { get; set; } = "";
    
    private double _rating = 0;
    [DisplayName("Rating")]
    [Watermark("Rating")]
    public double Rating                             
    {
        get => _rating;
        set => _rating = value <= 10 ? value : 10;
    }
    
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
        bool actors = film.Actors != null && this.Actors != null
                ? this.Actors.Equals(film.Actors)
                : this.Actors == film.Actors
            ;
        bool geners = this.Genres != null && film.Genres != null
            ? this.Genres.Equals(film.Genres)
            : this.Genres == film.Genres;

        List<bool> list = [title, description, studio, release, director, rating, path, actors, geners];
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
        return hash.ToHashCode();
    }
}