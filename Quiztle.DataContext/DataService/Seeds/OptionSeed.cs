﻿//using Quiztle.CoreBusiness.Entities.Quiz;

//namespace Quiztle.DataContext
//{
//    public class OptionSeed
//    {
//        private string RandomOne(string[] list)
//        {
//            var idx = new Random().Next(list.Length);
//            return list[idx];
//        }

//        private Option MakeOption()
//        {
//            var option = new Option
//            {
//                Name = RandomOne(_names),
//                Created = DateTime.Now
//            };
//            return option;
//        }

//        public async Task SeedDatabaseWithOptionsCountAsync(PostgreQuiztleContext context, int totalCount)
//        {
//            var count = 0;
//            var currentCycle = 0;
//            while (count < totalCount)
//            {
//                var list = new List<Option>();
//                while (currentCycle++ < 100 && count++ < totalCount)
//                {
//                    list.Add(MakeOption());
//                }
//                if (list.Count > 0)
//                {
//                    context.Options?.AddRange(list);
//                    await context.SaveChangesAsync();
//                }
//                currentCycle = 0;
//            }
//        }

//        private readonly string[] _names = new[]
//        {
//            "Option 1",
//            "Option 2",
//            "Option 3",
//            "Option 4",
//            "Option 5",
//            "Option 6",
//            "Option 7",
//            "Option 8",
//            "Option 9",
//            "Option 10",
//            "Option 11",
//            "Option 12",
//            "Option 13",
//            "Option 14",
//            "Option 15",
//            "Option 16",
//            "Option 17",
//            "Option 18",
//            "Option 19",
//            "Option 20",
//            "Option 21",
//            "Option 22",
//            "Option 23",
//            "Option 24",
//            "Option 25",
//            "Option 26",
//            "Option 27",
//            "Option 28",
//            "Option 29",
//            "Option 30",
//            "Option 31",
//            "Option 32",
//            "Option 33",
//            "Option 34",
//            "Option 35",
//            "Option 36",
//            "Option 37",
//            "Option 38",
//            "Option 39",
//            "Option 40",
//            "Option 41",
//            "Option 42",
//            "Option 43",
//            "Option 44",
//            "Option 45",
//            "Option 46",
//            "Option 47",
//            "Option 48",
//            "Option 49",
//            "Option 50",
//            "Option 51",
//            "Option 52",
//            "Option 53",
//            "Option 54",
//            "Option 55",
//            "Option 56",
//            "Option 57",
//            "Option 58",
//            "Option 59",
//            "Option 60",
//            "Option 61",
//            "Option 62",
//            "Option 63",
//            "Option 64",
//            "Option 65",
//            "Option 66",
//            "Option 67",
//            "Option 68",
//            "Option 69",
//            "Option 70",
//            "Option 71",
//            "Option 72",
//            "Option 73",
//            "Option 74",
//            "Option 75",
//            "Option 76",
//            "Option 77",
//            "Option 78",
//            "Option 79",
//            "Option 80",
//            "Option 81",
//            "Option 82",
//            "Option 83",
//            "Option 84",
//            "Option 85",
//            "Option 86",
//            "Option 87",
//            "Option 88",
//            "Option 89",
//            "Option 90",
//            "Option 91",
//            "Option 92",
//            "Option 93",
//            "Option 94",
//            "Option 95",
//            "Option 96",
//            "Option 97",
//            "Option 98",
//            "Option 99",
//            "Option 100"
//        };

//    }
//}
