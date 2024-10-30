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
    /// Тип операции
    /// </summary>
    public class OperationTypeEntity
    {
        /// <summary>
        /// Идентификатор типа операции
        /// </summary>
        [Key]
        [Comment("Идентификатор типа операции")]
        public int Id { get; set; }

        /// <summary>
        /// Название операции (например, токарная, фрезерная и т.д.)
        /// </summary>
        [Comment("Название операции")]
        public required string Name { get; set; }

        /// <summary>
        /// Операции
        /// </summary>
        public virtual ICollection<OperationEntity> Operations { get; set; } = [];
    }
}
