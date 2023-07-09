using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigStore.BusinessObject
{
    public class BaseEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Display(Name = "Ngày tạo")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Display(Name = "Ngày cập nhật")] 
        public DateTime UpdatedAt { get;set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
    }
}
