namespace Flashcards.Hillgrove.Dtos
{
    internal class FlashCardDto
    {
        public int DisplayIndex { get; set; }
        public required string Question { get; set; }
        public required string Answer { get; set; }
    }
}
