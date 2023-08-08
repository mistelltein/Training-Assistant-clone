﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Newtonsoft.Json;

namespace CS
{
    public class Card
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public int Rating { get; set; }
        public string Section { get; set; }

        public Card(string question, string answer, string section)
        {
            Question = question;
            Answer = answer;
            Rating = 0;
            Section = section;
        }
    }

    public class UserInterface
    {
        private List<Card> cards = new List<Card>();
        private string currentUser;

        public void Run()
        {
            LoadFromFile("cards.txt");
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("===== Training assistant =====");
                Console.WriteLine("1. Create a new card");
                Console.WriteLine("2. View all cards");
                Console.WriteLine("3. Edit a card");
                Console.WriteLine("4. Delete card");
                Console.WriteLine("5. Test your knowledge");
                Console.WriteLine("6. Exit");
                Console.WriteLine("Select an option: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        {
                            CreateNewCard();
                            break;
                        }
                    case "2":
                        {
                            DisplayAllCards();
                            break;
                        }
                    case "3":
                        {
                            EditCard();
                            break;
                        }
                    case "4":
                        {
                            DeleteCard();
                            break;
                        }
                    case "5":
                        {
                            TestMode();
                            break;
                        }
                    case "6":
                        {
                            exit = true;
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Incorrect choice. Please choose a valid option.");
                            break;
                        }
                }
            }

            SaveToFile("cards.txt");
        }

        private void CreateNewCard()
        {
            Console.Write("Enter a question: ");
            string question = Console.ReadLine();

            Console.Write("Enter an answer: ");
            string answer = Console.ReadLine();

            Console.WriteLine("Enter a section: ");
            string section = Console.ReadLine();

            Card newCard = new Card(question, answer, section);
            cards.Add(newCard);
            Console.WriteLine("The card has been added successfully.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void DisplayAllCards()
        {
            Console.WriteLine("==== All cards ====");

            for (int i = 0; i < cards.Count; i++)
            {
                Console.WriteLine($"Index: {i}");
                Console.WriteLine($"Question: {cards[i].Question}");
                Console.WriteLine($"Answer: {cards[i].Answer}");
                Console.WriteLine("=========");
            }

            Console.WriteLine("Total cards: " + cards.Count);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void EditCard()
        {
            DisplayAllCards();
            Console.WriteLine("Enter an index of the card to edit: ");

            if (int.TryParse(Console.ReadLine(), out int cardIndex)
                && cardIndex >= 0 && cardIndex < cards.Count)
            {
                Console.WriteLine("Enter a new question: ");
                string newQuestion = Console.ReadLine();

                Console.WriteLine("Enter a new answer: ");
                string newAnswer = Console.ReadLine();

                cards[cardIndex].Question = newQuestion;
                cards[cardIndex].Answer = newAnswer;

                Console.WriteLine("The card has been updated successfully.");
            }
            else
            {
                Console.WriteLine("Invalid card index.");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void DeleteCard()
        {
            DisplayAllCards();

            Console.WriteLine("Enter the index of the card to delete: ");
            if (int.TryParse(Console.ReadLine(), out int cardIndex)
                && cardIndex >= 0 && cardIndex < cards.Count)
            {
                cards.RemoveAt(cardIndex);
                Console.WriteLine("The card has been deleted successfully.");
            }
            else
            {
                Console.WriteLine("Invalid card index.");
            }

            Console.WriteLine("Total cards: " + cards.Count);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void TestMode()
        {
            Console.WriteLine("Select a section: ");
            string selectedSection = Console.ReadLine();

            List<Card> sectionCards = cards.Where(card => card.Section == selectedSection).ToList();

            if (sectionCards.Count == 0)
            {
                Console.WriteLine($"No cards available in the selected section '{selectedSection}'. Please add cards to this section.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"==== Test mode - Section: {selectedSection} ====");

            int correctAnswers = 0;
            int totalQuestions = sectionCards.Count;

            foreach (var card in sectionCards)
            {
                Console.WriteLine($"Question: {card.Question}");
                Console.WriteLine($"Your answer: ");
                string userAnswer = Console.ReadLine();

                if (userAnswer == card.Answer)
                {
                    Console.WriteLine("Correct!");
                    card.Rating++;
                    correctAnswers++;
                }
                else
                {
                    Console.WriteLine($"Incorrect. The correct answer is {card.Answer}");
                }
                Console.WriteLine("=========");
            }

            double scorePercentage = (double)correctAnswers / totalQuestions * 100;
            Console.WriteLine($"Test completed. Your score: {scorePercentage}%");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void LoadFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                cards = JsonConvert.DeserializeObject<List<Card>>(json);

                if (cards == null)
                {
                    cards = new List<Card>();
                }
            }
        }

        private void SaveToFile(string filePath)
        {
            string json = JsonConvert.SerializeObject(cards, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;

            UserInterface ui = new UserInterface();
            ui.Run();
        }
    }
}