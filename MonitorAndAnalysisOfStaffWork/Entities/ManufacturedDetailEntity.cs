using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitorAndAnalysisOfStaffWork.Entities
{
    /// <summary>
    /// Изготовленная деталь
    /// Содержит информацию о количестве произведенных деталей, дате производства, а также связь с сотрудником и деталью.
    /// </summary>
    [Comment("Изготовленная деталь")]
    public class ManufacturedDetailEntity
    {
        /// <summary>
        /// Идентификатор произведенной детали
        /// </summary>
        [Key]
        [Comment("Идентификатор произведенной детали")]
        public int Id { get; set; }

        /// <summary>
        /// Количество изготовленных деталей
        /// </summary>
        [Comment("Количество изготовленных деталей")]
        public required int Quantity { get; set; }

        /// <summary>
        /// Дата изготовления деталей
        /// </summary>
        [Comment("Дата изготовления деталей")]
        public required DateTime ManufactureDate { get; set; }

        /// <summary>
        /// Дата изготовления деталей
        /// </summary>
        [NotMapped]
        public string ManufactureDateString { get { return ManufactureDate.ToString("dd.MM.yyyy"); } }


        /// <summary>
        /// Идентификатор сотрудника, который произвел детали
        /// </summary>
        [Comment("Идентификатор сотрудника, который произвел детали")]
        public required int EmployeeId { get; set; }

        /// <summary>
        /// Ссылка на сотрудника, который произвел детали
        /// </summary>
        [ForeignKey(nameof(EmployeeId))]
        public virtual EmployeeEntity Employee { get; set; }

        /// <summary>
        /// Полное имя сотрудника
        /// </summary>
        [NotMapped]
        public string EmployeeFullName { get { return Employee.FullName; } }

        /// <summary>
        /// Идентификатор детали, которая была произведена
        /// </summary>
        [Comment("Идентификатор детали, которая была произведена")]
        public required int DetailId { get; set; }

        /// <summary>
        /// Ссылка на деталь, которая была произведена.
        /// </summary>
        [ForeignKey(nameof(DetailId))]
        public virtual DetailEntity Detail { get; set; }

        /// <summary>
        /// Сводное наименование детали
        /// </summary>
        [NotMapped]
        public string DetailName { get { return $"{Detail.Number}.{Detail.Name} - {Detail.Designation}"; } }

    }
}
