using System;
using System.Collections.Generic;
using System.Linq;

// Step 2: Create a Book model
public class Book
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Author { get; set; }
    public double Price { get; set; }
}

// Step 4: Create an Order model
public class Order
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int Quantity { get; set; }
    public double TotalPrice { get; set; }
}

// Step 5: Create the DbContext with two DbSets
public class BookStoreContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Order> Orders { get; set; }
}

// Step 6: Create the IBookStoreService interface
public interface IBookStoreService
{
    List<Book> GetAllBooks();
    Book GetBookById(int id);
    void AddOrder(Order order);
    void SaveChanges();
}

// Step 7: Create the BookStoreService implementation
public class BookStoreService : IBookStoreService
{
    private BookStoreContext _context;

    public BookStoreService(BookStoreContext context)
    {
        _context = context;
    }

    public List<Book> GetAllBooks()
    {
        return _context.Books.ToList();
    }

    public Book GetBookById(int id)
    {
        return _context.Books.Find(id);
    }

    public void AddOrder(Order order)
    {
        _context.Orders.Add(order);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}

// Step 12: Create the BookStoreController
public class BookStoreController : Controller
{
    private IBookStoreService _service;

    public BookStoreController(IBookStoreService service)
    {
        _service = service;
    }

    public IActionResult GetAllBooks()
    {
        var books = _service.GetAllBooks();
        return View(books);
    }

    public IActionResult GetBookById(int id)
    {
        var book = _service.GetBookById(id);
        return View(book);
    }

    [HttpPost]
    public IActionResult AddOrder(Order order)
    {
        _service.AddOrder(order);
        _service.SaveChanges();

        var book = _service.GetBookById(order.BookId);
        var message = $"Order for {book.Name} by {book.Author} (ID:{book.Id}) has been placed.";

        return View("OrderConfirmation", message);
    }
}
