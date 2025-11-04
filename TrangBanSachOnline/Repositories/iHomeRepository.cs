namespace TrangBanSachOnline
{
    public interface iHomeRepository
    {
        Task<IEnumerable<Models.Book>> GetBooks(string sTerm = "", int genreId = 0);
        Task<IEnumerable<Genre>> Genres();
        public string STerm { get; set; }
        public int GenreId { get; set; }
    }
}