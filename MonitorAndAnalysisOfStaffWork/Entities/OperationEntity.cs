using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorAndAnalysisOfStaffWork.Entities
{
    /// <summary>
    /// Класс, представляющий операцию.
    /// Содержит информацию о названии операции и стандартном времени выполнения.
    /// </summary>
    [Comment("Операция")]
    public class OperationEntity
    {
        /// <summary>
        /// Идентификатор операции
        /// </summary>
        [Key]
        [Comment("Идентификатор операции")]
        public int Id { get; set; }


        /// <summary>
        /// Стандартное время выполнения операции
        /// </summary>
        [Comment("Стандартное время выполнения операции")]
        public required TimeSpan StandardTime { get; set; }

        /// <summary>
        /// Идентификатор детали, к которой относится операция
        /// </summary>
        [Comment("Идентификатор детали, к которой относится операция")]
        public required int DetailId { get; set; }

        /// <summary>
        /// Ссылка на деталь, к которой относится операция
        /// </summary>
        [ForeignKey(nameof(DetailId))]
        public virtual DetailEntity Detail { get; set; }

        /// <summary>
        /// Идентификатор типа операции
        /// </summary>
        [Comment("Идентификатор типа операции")]
        public required int OperationTypeId { get; set; }

        /// <summary>
        /// Тип операции
        /// </summary>
        [ForeignKey(nameof(OperationTypeId))]   
        public virtual OperationTypeEntity OperationType { get; set; }

        /// <summary>
        /// Наименование типа операции
        /// </summary>
        [NotMapped]
        public string OperationTypeName { get { return OperationType != null ? OperationType.Name : ""; } }


        /// <summary>
        /// Стандартное время выполнения операции
        /// </summary>
        [NotMapped]
        public string StandardTimeString { get { return string.Format("{0:hh\\:mm}", StandardTime); } }
    }
}
