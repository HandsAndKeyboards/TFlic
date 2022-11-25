namespace Organization.Models.Organization.Graphs
{
    public class DateTimePoint
    {
        public DateTimePoint(DateTime dateTime, ulong v)
        {
            this.Point = dateTime;
            this.Value = v;
        }

        public DateTime Point { get; set; }
        public ulong Value { get; set; }
    }
}
