namespace Entities
{
    public class CreditsStudent
    {
        public int Id { get; set; }
        public int Total { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

    }
}
