using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using УчетПлатежей.Models;

namespace УчетПлатежей
{
    public class SData
    {
        public static void Initialize(PaymentContext context)
        {
            if (!context.Payments.Any())
            {
                context.Payments.AddRange(
                    new Payment
                    {
                        Date = new DateTime(2019, 08, 21),
                        FullName = "John",
                        Type = "Тип 1",
                        Positions = new List<Position>() {
                            new Position { Claim = 1000.0, Text = "John 1" },
                            new Position { Claim = 1500.0, Text = "John 2" },
                            new Position { Claim = 2000.0, Text = "John 3" }
                        },
                        Sum = 4500.0
                    },
                    new Payment
                    {
                        Date = new DateTime(2019, 08, 22),
                        FullName = "Alex",
                        Type = "Тип 2",
                        Positions = new List<Position>() {
                            new Position { Claim = 1000.3, Text = "Alex 1" },
                            new Position { Claim = 1500.3, Text = "Alex 2" },
                            new Position { Claim = 2000.3, Text = "Alex 3" }
                        },
                        Sum = 4500.9
                    },
                    new Payment
                    {
                        Date = new DateTime(2019, 08, 23),
                        FullName = "Ann",
                        Type = "Тип 1",
                        Positions = new List<Position>() {
                            new Position { Claim = 1000.9, Text = "Ann 1" },
                            new Position { Claim = 1500.9, Text = "Ann 2" },
                            new Position { Claim = 2000.9, Text = "Ann 3" }
                        },
                        Sum = 4502.7
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
