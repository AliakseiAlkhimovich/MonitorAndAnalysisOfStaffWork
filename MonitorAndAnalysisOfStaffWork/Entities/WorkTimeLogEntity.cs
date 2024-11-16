using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitorAndAnalysisOfStaffWork.Entities
{
    /// <summary>
    /// Класс, представляющий запись рабочего времени сотрудника
    /// Содержит дату, количество отработанных часов и связь с сотрудником.
    /// </summary>
    [Comment("Рабочее время сотрудника")]
    public class WorkTimeLogEntity
    {
        /// <summary>
        /// Идентификатор записи рабочего времени
        /// </summary>
        [Key]
        [Comment("Идентификатор записи рабочего времени")]
        public int Id { get; set; }

        /// <summary>
        /// Дата записи рабочего времени
        /// </summary>
        [Comment("Дата записи рабочего времени")]
        public required DateTime Date { get; set; } 

        /// <summary>
        /// Количество отработанных часов
        /// </summary>
        [Comment("Количество отработанных часов")]
        public required TimeSpan HoursWorked { get; set; }

        /// <summary>
        /// Идентификатор сотрудника, к которому относится запись
        /// </summary>
        [Comment("Идентификатор сотрудника, к которому относится запись")]
        public required int EmployeeId { get; set; }

        /// <summary>
        /// Ссылка на сотрудника, к которому относится запись рабочего времени.
        /// </summary>
        [ForeignKey(nameof(EmployeeId))]
        public virtual EmployeeEntity Employee { get; set; }

        /// <summary>
        /// Дата записи рабочего времени
        /// </summary>
        [NotMapped]
        public string DateString { get { return Date.ToString("dd.MM.yyyy"); } }


        /// <summary>
        /// Количество отработанных часов
        /// </summary>
        [NotMapped]
        public string HoursWorkedString { get { return string.Format("{0:hh\\:mm}", HoursWorked); } }

    }
}
