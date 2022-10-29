using System.ComponentModel.DataAnnotations;
using Desgram.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Desgram.Api.Models
{
    public class CreatePublicationModel
    {
        public string Description { get; set; } = String.Empty;
        [Required]
        public List<IFormFile> Images { get; set; } 
    }
}
