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
    /// Класс, представляющий учетную запись пользователя.
    /// Содержит имя пользователя, хэш пароля и связанную с ним роль.
    /// </summary>
    [Comment("Учетную запись пользователя")]
    public class UserEntity
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [Key]
        [Comment("Идентификатор пользователя")]
        public int Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        [Comment("Имя пользователя")]
        public required string Username { get; set; }

        /// <summary>
        /// Хэш пароля пользователя
        /// </summary>
        [Comment("Хэш пароля пользователя")]
        public required string PasswordHash { get; set; }

        /// <summary>
        /// Полное имя пользователя
        /// </summary>
        [Comment("Полное имя пользователя")]
        public required string FullName {  get; set; }

        /// <summary>
        /// Идентификатор роли пользователя
        /// </summary>
        [Comment("Идентификатор роли пользователя")]
        public required int RoleId { get; set; }

        /// <summary>
        /// Ссылка на роль пользователя.
        /// </summary>
        [ForeignKey(nameof(RoleId))]
        public virtual RoleEntity Role { get; set; }


        /// <summary>
        /// Задан ли пароль
        /// </summary>
        [NotMapped]
        public string HasPasswordString { get { return string.IsNullOrEmpty(PasswordHash) ? "Пароль не задан" : "Пароль задан"; } }


        /// <summary>
        /// Наименование роли
        /// </summary>
        [NotMapped]
        public string? RoleName { get { return Role != null ? Role.Title : ""; } }
    }
}
