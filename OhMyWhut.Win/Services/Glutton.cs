using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OhMyWhut.Engine;

namespace OhMyWhut.Win.Services
{
    internal class Glutton
    {
        private readonly Gluttony gluttony;

        Glutton()
        {
            gluttony = new Gluttony();
        }

        public async Task LoginAsync(string username, string password)
        {
            await gluttony.LoginAsync(username, password);
        }

        public async Task<IEnumerable<Engine.Data.Book>> GetBooksAsync()
        {
            return await gluttony.GetBooksAsync();
        }
    }
}
