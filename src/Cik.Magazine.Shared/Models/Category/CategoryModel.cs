using System;
using System.ComponentModel.DataAnnotations;

namespace Cik.Magazine.Shared.Models.Category
{
    [Serializable]
    public class CategoryModel
    {
        [Required]
        public string Name { get; set; }

        public Guid ParentId { get; set; }
    }
}