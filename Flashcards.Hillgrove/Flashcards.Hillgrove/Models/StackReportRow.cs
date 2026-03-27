namespace Flashcards.Hillgrove.Models
{
    internal class StackReportRow
    {
        public string StackName { get; set; } = string.Empty;
        public double January { get; set; }
        public double February { get; set; }
        public double March { get; set; }
        public double April { get; set; }
        public double May { get; set; }
        public double June { get; set; }
        public double July { get; set; }
        public double August { get; set; }
        public double September { get; set; }
        public double October { get; set; }
        public double November { get; set; }
        public double December { get; set; }

        public double Total =>
            January
            + February
            + March
            + April
            + May
            + June
            + July
            + August
            + September
            + October
            + November
            + December;

        public double Average
        {
            get
            {
                var monthValues = new[]
                {
                    January,
                    February,
                    March,
                    April,
                    May,
                    June,
                    July,
                    August,
                    September,
                    October,
                    November,
                    December,
                };

                var activeMonthValues = monthValues.Where(value => value > 0).ToArray();
                return activeMonthValues.Length == 0 ? 0 : activeMonthValues.Average();
            }
        }
    }
}
