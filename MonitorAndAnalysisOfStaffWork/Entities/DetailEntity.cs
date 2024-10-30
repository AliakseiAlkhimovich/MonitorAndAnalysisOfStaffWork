using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorAndAnalysisOfStaffWork.Entities
{
    /// <summary>
    /// Класс, представляющий деталь
    /// Содержит информацию о номере и названии детали, а также список операций, связанных с её изготовлением.
    /// </summary>
    [Comment("Деталь")]
    public class DetailEntity
    {
        /// <summary>
        /// Идентификатор детали
        /// </summary>
        [Key]
        [Comment("Идентификатор детали")]
        public int Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [Comment("Наименование")]
        public required string Name { get; set; }

        /// <summary>
        /// Номер детали (артикул)
        /// </summary>
        [Comment("Номер детали - артикул")]
        public required string Number { get; set; }

        /// <summary>
        /// Обозначение
        /// </summary>
        [Comment("Обозначение")]
        public required string Designation { get; set; }

        /// <summary>
        /// Список операций, связанных с изготовлением данной детали
        /// </summary>
        public virtual ICollection<OperationEntity> Operations { get; set; } = [];
    }
}
