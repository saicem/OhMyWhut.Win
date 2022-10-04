using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using OhMyWhut.Engine.Extentions;
using OhMyWhut.Engine.Responses;

namespace OhMyWhut.Engine.Data
{
    public class Book
    {
        /// <summary>
        /// 书名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 借阅日期
        /// </summary>
        public DateOnly BorrowDate { get; set; }
        
        /// <summary>
        /// 到期时间
        /// </summary>
        public DateOnly ExpireDate { get; set; }

        public Book(string name, DateOnly borrowDate, DateOnly expireDate)
        {
            Name = name;
            BorrowDate = borrowDate;
            ExpireDate = expireDate;
        }

        public Book(BookItem bookItem)
        {
            Name = bookItem.Zbt;
            BorrowDate = bookItem.Jyrq;
            ExpireDate = bookItem.Dqrq;
        }
    }
}
