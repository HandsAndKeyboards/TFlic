namespace Organization.Models.Organization.Project
{
    public interface ISprint
    {
        /// <summary>
        /// Дата начала спринта
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// Дата окончания спринта
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Продолжительность спринта
        /// </summary>
        public ulong Duration { get; set; }

        /// <summary>
        /// Номер спринта
        /// </summary>
        public uint Number { get; set; }
    }
}
