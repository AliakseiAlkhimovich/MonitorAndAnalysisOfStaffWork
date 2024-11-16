using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MonitorAndAnalysisOfStaffWork.Entities
{
    /// <summary>
    /// Класс, представляющий сотрудника.
    /// Содержит информацию о ФИО, должности, табельном номере и связанный с ним учет рабочего времени.
    /// </summary>
    [Comment("Сотрудник")]
    public class EmployeeEntity
    {
        /// <summary>
        /// Идентификатор сотрудника
        /// </summary>
        [Key]
        [Comment("Идентификатор сотрудника")]
        public int Id { get; set; }

        /// <summary>
        /// Полное имя сотрудника
        /// </summary>
        [Comment("Полное имя сотрудника")]
        public required string FullName { get; set; }

        /// <summary>
        /// Должность сотрудника
        /// </summary>
        [Comment("Должность сотрудника")]
        public required string Position { get; set; }

        /// <summary>
        /// Табельный номер сотрудника
        /// </summary>
        [Comment("Табельный номер сотрудника")]
        public required string Number { get; set; }

        /// <summary>
        /// Лог записей рабочего времени сотрудника.
        /// </summary>
        public virtual ICollection<WorkTimeLogEntity> WorkTimeLogs { get; set; } = [];
    }
}
