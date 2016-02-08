using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scambio.DataAccess.EntityFramework;
using Scambio.Domain.Models;

namespace Scambio.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ScambioContext cont = new ScambioContext())
            {
                IList<ExternalLogin> aaa = cont.Logins.ToList();
            }
        }
    }
}
