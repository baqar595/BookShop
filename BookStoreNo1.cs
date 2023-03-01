using System;
using System.Collections.Generic;
using System.Linq;

// Model for book
public class Book
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Author { get; set; }
    public decimal Price { get; set; }
}

// Model for order
public class Order
{
    public int Id { get; set; }
    public int BookID { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}

// Interface for Bookstore service
public interface IBookstoreService
{
    IEnumerable<Book> GetAllBooks();
    Book GetBookById(int id);
    void AddOrder(Order order);
    void SaveChanges();
}

// Implementation of Bookstore service
public class BookstoreService : IBookstoreService
{
    private List<Book> books;
    private List<Order> orders;

    public BookstoreService()
    {
        // Initialize books
        books = new List<Book>
        {
            new Book { Id = 1, Name = "Book 1", Author = "Author 1", Price = 10.00m },
            new Book { Id = 2, Name = "Book 2", Author = "Author 2", Price = 20.00m },
            new Book { Id = 3, Name = "Book 3", Author = "Author 3", Price = 30.00m }
        };

        // Initialize orders
        orders = new List<Order>();
    }

    public IEnumerable<Book> GetAllBooks()
    {
        return books;
    }

    public Book GetBookById(int id)
    {
        return books.FirstOrDefault(b => b.Id == id);
    }

    public void AddOrder(Order order)
    {
        orders.Add(order);
    }

    public void SaveChanges()
    {
        // Save changes to database or file system
    }
}

// Controller for Bookstore
public class BookstoreController
{
    private readonly IBookstoreService bookstoreService;

    public BookstoreController(IBookstoreService bookstoreService)
    {
        this.bookstoreService = bookstoreService;
    }

    public IEnumerable<Book> GetAllBooks()
    {
        return bookstoreService.GetAllBooks();
    }

    public Book GetBookById(int id)
    {
        return bookstoreService.GetBookById(id);
    }

    public string AddOrder(Order order)
    {
        // Get book by ID
        Book book = GetBookById(order.BookID);

        if (book == null)
        {
            return "Book not found.";
        }

        // Update order with book details
        order.Id = Guid.NewGuid().GetHashCode();
        order.TotalPrice = book.Price * order.Quantity;

        // Add order to list
        bookstoreService.AddOrder(order);

        return $"Order added: {book.Name} by {book.Author} for {order.TotalPrice:C} (Order ID: {order.Id}).";
    }
}

// Sample usage
public static class Program
{
    public static void Main()
    {
        // Create bookstore service and controller
        var bookstoreService = new BookstoreService();
        var bookstoreController = new BookstoreController(bookstoreService);

        // Get all books
        IEnumerable<Book> books = bookstoreController.GetAllBooks();
        foreach (Book book in books)
        {
            Console.WriteLine($"{book.Id}: {book.Name} by {book.Author} ({book.Price:C})");
        }

        // Get book by ID
        int bookId = 1;
        Book selectedBook = bookstoreController.GetBookById(bookId);
        Console.WriteLine($"Selected book: {selected
