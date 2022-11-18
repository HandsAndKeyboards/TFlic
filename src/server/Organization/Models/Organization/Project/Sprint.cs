namespace Organization.Models.Organization.Project
{
    public class Sprint
    {
        /// <summary>
        /// Дата начала спринта
        /// </summary>
        public DateTime BeginDate{ get; set; }

        /// <summary>
        /// Дата окончания спринта
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Продолжительность спринта
        /// </summary>
        public DateTime Duration { get; set; }
    }
}
