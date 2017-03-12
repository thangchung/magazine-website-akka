using System;
using System.ComponentModel.DataAnnotations;
using Cik.Magazine.Shared.Messages.Category;

namespace Cik.Magazine.Shared.Models.Category
{
    [Serializable]
    public class CategoryModel
    {
        [Required]
        public string Name { get; set; }
        public Status Status { get; set; }
    }
}