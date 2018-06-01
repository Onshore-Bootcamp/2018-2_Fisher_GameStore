using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStoreBLL.Models;

namespace GameStoreBLL
{
    public class GamesBLO
    {
        public decimal CalculationSum(List<GamesBO> games)
        {
            decimal sum = 0;

            foreach (GamesBO ga in games)
            {
                sum += ga.Price;
            }
            return sum;
        }
    }
}
