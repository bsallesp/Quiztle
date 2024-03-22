﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BrunoTheBot.CoreBusiness;

namespace BrunoTheBot.DataContext
{
    public class QuestionSeed
    {
        private string RandomOne(string[] list)
        {
            var idx = new Random().Next(list.Length);
            return list[idx];
        }

        private Question MakeQuestion()
        {
            var question = new Question
            {
                Name = RandomOne(_names),
                Created = DateTime.Now
            };
            return question;
        }

        public async Task SeedDatabaseWithQuestionCountAsync(SqliteDataContext context, int totalCount)
        {
            var count = 0;
            var currentCycle = 0;
            while (count < totalCount)
            {
                var list = new List<Question>();
                while (currentCycle++ < 100 && count++ < totalCount)
                {
                    list.Add(MakeQuestion());
                }
                if (list.Count > 0)
                {
                    context.Questions?.AddRange(list);
                    await context.SaveChangesAsync();
                }
                currentCycle = 0;
            }
        }

        private readonly string[] _names = new[]
        {
            "Question 1",
            "Question 2",
            "Question 3",
            "Question 4",
            "Question 5",
            "Question 6",
            "Question 7",
            "Question 8",
            "Question 9",
            "Question 10",
            "Question 11",
            "Question 12",
            "Question 13",
            "Question 14",
            "Question 15",
            "Question 16",
            "Question 17",
            "Question 18",
            "Question 19",
            "Question 20",
            "Question 21",
            "Question 22",
            "Question 23",
            "Question 24",
            "Question 25",
            "Question 26",
            "Question 27",
            "Question 28",
            "Question 29",
            "Question 30",
            "Question 31",
            "Question 32",
            "Question 33",
            "Question 34",
            "Question 35",
            "Question 36",
            "Question 37",
            "Question 38",
            "Question 39",
            "Question 40",
            "Question 41",
            "Question 42",
            "Question 43",
            "Question 44",
            "Question 45",
            "Question 46",
            "Question 47",
            "Question 48",
            "Question 49",
            "Question 50",
            "Question 51",
            "Question 52",
            "Question 53",
            "Question 54",
            "Question 55",
            "Question 56",
            "Question 57",
            "Question 58",
            "Question 59",
            "Question 60",
            "Question 61",
            "Question 62",
            "Question 63",
            "Question 64",
            "Question 65",
            "Question 66",
            "Question 67",
            "Question 68",
            "Question 69",
            "Question 70",
            "Question 71",
            "Question 72",
            "Question 73",
            "Question 74",
            "Question 75",
            "Question 76",
            "Question 77",
            "Question 78",
            "Question 79",
            "Question 80",
            "Question 81",
            "Question 82",
            "Question 83",
            "Question 84",
            "Question 85",
            "Question 86",
            "Question 87",
            "Question 88",
            "Question 89",
            "Question 90",
            "Question 91",
            "Question 92",
            "Question 93",
            "Question 94",
            "Question 95",
            "Question 96",
            "Question 97",
            "Question 98",
            "Question 99",
            "Question 100"
        };
    }
}
