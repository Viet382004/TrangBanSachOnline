using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.Metrics;
using TrangBanSachOnline.Data;
using TrangBanSachOnline.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TrangBanSachOnline.Repositories
{

    public interface IGenreRepository
    {
        Task AddGenre(Genre genre);
        Task UpdateGenre(Genre genre);
        Task DeleteGenre(Genre genre);
        Task<Genre?> GetGenreById(int genreId);
        Task<IEnumerable<Genre>> GetGenres();
    }
    public class GenreRepository : IGenreRepository
    {
        private readonly ApplicationDbContext _applincationDbContext;

        public GenreRepository(ApplicationDbContext applincationDbContext)
        {
            _applincationDbContext = applincationDbContext;
        }

        public async Task AddGenre(Genre genre)
        {
            _applincationDbContext.Genres.Add(genre);
            await _applincationDbContext.SaveChangesAsync();
        }
        public async Task UpdateGenre(Genre genre)
        {
            _applincationDbContext.Genres.Update(genre);
            await _applincationDbContext.SaveChangesAsync();
        }
        public async Task DeleteGenre(Genre genre)
        {
            _applincationDbContext.Genres.Remove(genre);
            await _applincationDbContext.SaveChangesAsync();
        }

        public async Task<Genre?> GetGenreById(int genreId)
        {
            return await _applincationDbContext.Genres.FindAsync(genreId);
        }

        public async Task<IEnumerable<Genre>> GetGenres()
        {
            return await _applincationDbContext.Genres.ToListAsync();
        }

        
    }
}
