namespace Flashcards.Hillgrove.Models
{
    public class Flashcard
    {
        public int Id { get; set; }
        public int StackId { get; set; }
        public required string Question { get; set; }
        public required string Answer { get; set; }
    }
}