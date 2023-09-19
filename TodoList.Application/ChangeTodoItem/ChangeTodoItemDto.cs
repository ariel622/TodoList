﻿using System;
using System.ComponentModel.DataAnnotations;

namespace TodoList.Application.ChangeTodoItem
{
    public class ChangeTodoItemDto
    {
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string? Title { get; set; }

        [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        public bool? IsIncomplete { get; set; }
    }
}
