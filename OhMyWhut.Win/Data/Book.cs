using System;
using System.Text.Json.Serialization;
using OhMyWhut.Win.Extentions;

namespace OhMyWhut.Win.Data
{
    public class Book
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateOnly BorrowDate { get; set; }

        public DateOnly ExpireDate { get; set; }
    }
}
