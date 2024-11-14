﻿using Microsoft.EntityFrameworkCore;
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
    /// Класс, представляющий роль пользователя в системе.
    /// Содержит информацию о названии роли и её описании.
    /// </summary>
    [Comment("Роль пользователя в системе")]
    public class RoleEntity
    {
        /// <summary>
        /// Идентификатор роли
        /// </summary>
        [Key]
        [Comment("Идентификатор роли")]
        public int Id { get; set; }

        /// <summary>
        /// Название роли (например, Администратор, Пользователь)
        /// </summary>
        [Comment("Название роли (например, Администратор, Пользователь)")]
        public required string Name { get; set; }

        /// <summary>
        /// Описание роли (например, Полный доступ, Ограниченный доступ)
        /// </summary>
        [Comment("Описание роли")]
        public string? Title { get; set; }
    }
}