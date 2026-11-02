using System;
using System.Collections.Generic;

namespace ExaminationSystem
{
    enum QuestionLevel
    {
        Easy = 1,
        Medium = 2,
        Hard = 3
    }

    enum QuestionType
    {
        TrueFalse = 1,
        ChooseOne = 2,
        MultipleChoice = 3
    }

    enum ExamType
    {
        Practical = 1,
        Final = 2
    }

    class Question
    {
        public string Header;
        public int Marks;
        public QuestionLevel Level;
        public QuestionType Type;

        private List<string> Choices;
        private string CorrectAnswer;

        public Question(
            string header,
            int marks,
            QuestionLevel level,
            QuestionType type,
            List<string> choices,
            string correctAnswer)
        {
            Header = header;
            Marks = marks;
            Level = level;
            Type = type;
            Choices = choices;
            CorrectAnswer = correctAnswer;
        }

        public void Display()
        {
            Console.WriteLine(Header);

            if (Type == QuestionType.TrueFalse)
            {
                Console.WriteLine("1. True");
                Console.WriteLine("2. False");
            }
            else
            {
                for (int i = 0; i < Choices.Count; i++)
                {
                    Console.WriteLine(
                        (i + 1) + ". " + Choices[i]);
                }

                if (Type == QuestionType.MultipleChoice)
                {
                    Console.WriteLine(
                        "Enter answers separated by commas (e.g. 1,3)");
                }
            }
        }

        public bool CheckAnswer(string answer)
        {
            answer = answer.Trim();

            if (Type == QuestionType.TrueFalse)
            {
                if (answer == "1")
                    answer = "True";

                if (answer == "2")
                    answer = "False";

                return answer.Equals(
                    CorrectAnswer,
                    StringComparison.OrdinalIgnoreCase);
            }

            if (Type == QuestionType.ChooseOne)
            {
                return answer == CorrectAnswer;
            }

            string[] userAnswers = answer.Split(',');
            string[] correctAnswers = CorrectAnswer.Split(',');

            List<int> userNumbers = new List<int>();
            List<int> correctNumbers = new List<int>();

            foreach (string item in userAnswers)
            {
                int number;

                if (!int.TryParse(item.Trim(), out number))
                    return false;

                userNumbers.Add(number);
            }

            foreach (string item in correctAnswers)
            {
                int number;

                if (!int.TryParse(item.Trim(), out number))
                    return false;

                correctNumbers.Add(number);
            }

            userNumbers.Sort();
            correctNumbers.Sort();

            if (userNumbers.Count != correctNumbers.Count)
                return false;

            for (int i = 0; i < userNumbers.Count; i++)
            {
                if (userNumbers[i] != correctNumbers[i])
                    return false;
            }

            return true;
        }
    }

    class ExaminationSystem
    {
        private List<Question> QuestionBank =
            new List<Question>();

        public void MainMenu()
        {
            while (true)
            {
                Console.WriteLine("\n===== EXAMINATION SYSTEM =====");
                Console.WriteLine("1. Doctor Mode");
                Console.WriteLine("2. Student Mode");
                Console.WriteLine("3. Exit");

                int choice = ReadInt("Choose: ", 1, 3);

                if (choice == 1)
                {
                    DoctorMode();
                }
                else if (choice == 2)
                {
                    StudentMode();
                }
                else
                {
                    Console.WriteLine("Goodbye!");
                    break;
                }
            }
        }

        public void DoctorMode()
        {
            Console.WriteLine("\n===== DOCTOR MODE =====");

            int count = ReadInt(
                "How many questions do you want to add? ",
                1, 100);

            for (int i = 1; i <= count; i++)
            {
                Console.WriteLine("\nQuestion " + i);

                Console.WriteLine("1. True / False");
                Console.WriteLine("2. Choose One");
                Console.WriteLine("3. Multiple Choice");

                int typeNumber = ReadInt("Choose type: ", 1, 3);
                QuestionType type = (QuestionType)typeNumber;

                Console.WriteLine("\n1. Easy");
                Console.WriteLine("2. Medium");
                Console.WriteLine("3. Hard");

                int levelNumber = ReadInt("Choose level: ", 1, 3);
                QuestionLevel level = (QuestionLevel)levelNumber;

                string header = ReadText("Enter question: ");
                int marks = ReadInt("Enter marks: ", 1, 1000);

                List<string> choices = new List<string>();
                string correctAnswer = "";

                if (type == QuestionType.TrueFalse)
                {
                    Console.WriteLine("1. True");
                    Console.WriteLine("2. False");

                    int answer = ReadInt(
                        "Enter correct answer: ", 1, 2);

                    if (answer == 1)
                        correctAnswer = "True";
                    else
                        correctAnswer = "False";
                }
                else
                {
                    for (int j = 1; j <= 4; j++)
                    {
                        choices.Add(
                            ReadText("Enter choice " + j + ": "));
                    }

                    if (type == QuestionType.ChooseOne)
                    {
                        int answer = ReadInt(
                            "Enter correct choice number: ", 1, 4);

                        correctAnswer = answer.ToString();
                    }
                    else
                    {
                        while (true)
                        {
                            correctAnswer = ReadText(
                                "Enter correct choices (e.g. 1,3): ");

                            string[] answers =
                                correctAnswer.Split(',');

                            bool valid = true;
                            List<int> numbers = new List<int>();

                            foreach (string item in answers)
                            {
                                int number;

                                if (!int.TryParse(
                                    item.Trim(), out number)
                                    || number < 1 || number > 4
                                    || numbers.Contains(number))
                                {
                                    valid = false;
                                    break;
                                }

                                numbers.Add(number);
                            }

                            if (valid && numbers.Count > 0)
                            {
                                numbers.Sort();
                                correctAnswer = string.Join(",", numbers);
                                break;
                            }

                            Console.WriteLine(
                                "Invalid choices. Try again.");
                        }
                    }
                }

                Question q = new Question(
                    header, marks, level, type,
                    choices, correctAnswer);

                QuestionBank.Add(q);

                Console.WriteLine("Question added successfully!");
            }

            Console.WriteLine(
                "\nTotal questions: " + QuestionBank.Count);
        }

        public void StudentMode()
        {
            Console.WriteLine("\n===== STUDENT MODE =====");

            if (QuestionBank.Count == 0)
            {
                Console.WriteLine(
                    "No questions available. Ask the doctor to add questions.");
                return;
            }

            Console.WriteLine("1. Practical Exam");
            Console.WriteLine("2. Final Exam");

            int examNumber = ReadInt("Choose exam: ", 1, 2);
            ExamType examType = (ExamType)examNumber;

            Console.WriteLine("\n1. Easy");
            Console.WriteLine("2. Medium");
            Console.WriteLine("3. Hard");

            int levelNumber = ReadInt("Choose level: ", 1, 3);
            QuestionLevel level = (QuestionLevel)levelNumber;

            List<Question> selectedQuestions =
                new List<Question>();

            foreach (Question q in QuestionBank)
            {
                if (q.Level == level)
                {
                    selectedQuestions.Add(q);
                }
            }

            if (selectedQuestions.Count == 0)
            {
                Console.WriteLine("No questions for this level.");
                return;
            }

            if (examType == ExamType.Practical)
            {
                int half = (selectedQuestions.Count + 1) / 2;

                selectedQuestions =
                    selectedQuestions.GetRange(0, half);
            }

            int score = 0;
            int totalMarks = 0;

            Console.WriteLine("\n===== START EXAM =====");

            for (int i = 0; i < selectedQuestions.Count; i++)
            {
                Question q = selectedQuestions[i];

                Console.WriteLine("\nQuestion " + (i + 1));
                Console.WriteLine("Marks: " + q.Marks);

                q.Display();

                string answer = ReadText("Your answer: ");

                if (q.CheckAnswer(answer))
                {
                    Console.WriteLine("Correct answer!");
                    score += q.Marks;
                }
                else
                {
                    Console.WriteLine("Wrong answer!");
                }

                totalMarks += q.Marks;
            }

            Console.WriteLine("\n===== EXAM RESULT =====");
            Console.WriteLine(
                "Your Result: " + score + " / " + totalMarks);
        }

        private int ReadInt(
            string message, int min, int max)
        {
            while (true)
            {
                Console.Write(message);

                int number;

                if (int.TryParse(Console.ReadLine(), out number)
                    && number >= min && number <= max)
                {
                    return number;
                }

                Console.WriteLine(
                    "Please enter a number from "
                    + min + " to " + max);
            }
        }

        private string ReadText(string message)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine() ?? "";

                if (!string.IsNullOrWhiteSpace(input))
                    return input.Trim();

                Console.WriteLine("Input cannot be empty.");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ExaminationSystem system =
                new ExaminationSystem();

            system.MainMenu();
        }
    }
}
