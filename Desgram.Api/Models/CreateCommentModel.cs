﻿using System.ComponentModel.DataAnnotations;
using Desgram.DAL.Entities;

namespace Desgram.Api.Models
{
    public class CreateCommentModel
    {
        [Required]
        public string Content { get; set; } = null!;

        [Required]
        public Guid PublicationId { get; set; }
    }
}
