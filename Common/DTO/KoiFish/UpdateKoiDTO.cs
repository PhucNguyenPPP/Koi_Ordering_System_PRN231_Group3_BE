﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Common.DTO.KoiFish
{
    public class UpdateKoiDTO
    {
        [Required(ErrorMessage = "Vui lòng nhập id cá Koi")]
        public Guid KoiId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên")]
        public string Name { get; set; } = null!;

        public IFormFile? AvatarLink { get; set; } = null!;

        public IFormFile? CertificationLink { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập giá")]
        [Range(1, int.MaxValue, ErrorMessage = "Giá không hợp lệ")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập ngày sinh")]
        public DateTime Dob { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giới tính")]
        public string Gender { get; set; } = null!;
        
        [Required(ErrorMessage = "Vui lòng nhập giống")]
        public List<Guid> BreedId { get; set; }

    }
}
