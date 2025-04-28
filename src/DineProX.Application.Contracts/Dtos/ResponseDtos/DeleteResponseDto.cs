using System;
using System.Collections.Generic;
using System.Text;

namespace DineProX.Dtos.ResponseDtos
{
    public class DeleteResponseDto
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
