using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using OhMyWhut.Win.Extentions;

namespace OhMyWhut.Win.Data
{
    [Table(nameof(Book))]
    public class Book
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateOnly BorrowDate { get; set; }

        public DateOnly ExpireDate { get; set; }
    }
}
