using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
}

class Program
{
    private const string FilePath = "books.json";
    private static List<Book> books = new List<Book>();

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        LoadBooks();

        while (true)
        {
            Console.WriteLine("\n=== КАТАЛОГ КНИГ ===");
            Console.WriteLine("1. Показати всі книги");
            Console.WriteLine("2. Додати книгу");
            Console.WriteLine("3. Пошук книги");
            Console.WriteLine("4. Видалити книгу");
            Console.WriteLine("5. Вихід");
            Console.Write("Виберіть дію: ");

            string choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    ShowBooks();
                    break;
                case "2":
                    AddBook();
                    break;
                case "3":
                    SearchBooks();
                    break;
                case "4":
                    DeleteBook();
                    break;
                case "5":
                    SaveBooks();
                    return;
                default:
                    Console.WriteLine("Некоректний вибір. Спробуйте ще раз.");
                    break;
            }
        }
    }

    private static void ShowBooks()
    {
        if (!books.Any())
        {
            Console.WriteLine("Каталог порожній.");
            return;
        }

        foreach (var b in books)
        {
            Console.WriteLine($"[ID: {b.Id}] \"{b.Title}\" - {b.Author} ({b.Year} р.)");
        }
    }

    private static void AddBook()
    {
        Console.Write("Введіть назву книги: ");
        string title = Console.ReadLine();

        Console.Write("Введіть автора: ");
        string author = Console.ReadLine();

        Console.Write("Введіть рік видання: ");
        int.TryParse(Console.ReadLine(), out int year);

        int newId = books.Any() ? books.Max(b => b.Id) + 1 : 1;

        books.Add(new Book { Id = newId, Title = title, Author = author, Year = year });
        SaveBooks();
        Console.WriteLine("Книгу успішно додано!");
    }

    private static void SearchBooks()
    {
        Console.Write("Введіть текст для пошуку (назва або автор): ");
        string query = Console.ReadLine()?.ToLower();

        var results = books.Where(b => b.Title.ToLower().Contains(query) || b.Author.ToLower().Contains(query)).ToList();

        if (!results.Any())
        {
            Console.WriteLine("Книг за вашим запитом не знайдено.");
            return;
        }

        Console.WriteLine("\nЗнайдені книги:");
        foreach (var b in results)
        {
            Console.WriteLine($"[ID: {b.Id}] \"{b.Title}\" - {b.Author} ({b.Year} р.)");
        }
    }

    private static void DeleteBook()
    {
        Console.Write("Введіть ID книги для видалення: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var bookToRemove = books.FirstOrDefault(b => b.Id == id);
            if (bookToRemove != null)
            {
                books.Remove(bookToRemove);
                SaveBooks();
                Console.WriteLine("Книгу видалено.");
            }
            else
            {
                Console.WriteLine("Книгу з таким ID не знайдено.");
            }
        }
        else
        {
            Console.WriteLine("Некоректний ID.");
        }
    }

    private static void SaveBooks()
    {
        try
        {
            string json = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка збереження даних: {ex.Message}");
        }
    }

    private static void LoadBooks()
    {
        if (File.Exists(FilePath))
        {
            try
            {
                string json = File.ReadAllText(FilePath);
                books = JsonSerializer.Deserialize<List<Book>>(json) ?? new List<Book>();
            }
            catch
            {
                books = new List<Book>();
            }
        }
    }
}