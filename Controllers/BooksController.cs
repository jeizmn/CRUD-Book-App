using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CRUDApp.Data;
using Microsoft.AspNetCore.Mvc;
using CRUDApp.Models;

namespace CRUDApp.Controllers;

public class BooksController : Controller
{
    private ApplicationDbContext _context;

    // Constructor to initialize the controller with the application's DbContext
    public BooksController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Books/Index
    // Displays a paginated list of books
    public async Task<IActionResult> Index(int pg = 1)
    {
        // Retrieve all books from the database
        var books = await _context.Books.ToListAsync();

        // Set default page size and ensure pg (page number) is not less than 1
        const int pageSize = 5;
        if (pg < 1)
            pg = 1;

        // Calculate the total number of books
        int recsCount = books.Count();

        // Create a Pager object to handle pagination information
        var pager = new Pager(recsCount, pg, pageSize);

        // Calculate the number of records to skip for the current page
        int recSkip = (pg - 1) * pageSize;

        // Retrieve a subset of books based on the current page and page size
        var data = books.Skip(recSkip).Take(pager.PageSize).ToList();

        // Store the Pager object in ViewBag for use in the view
        this.ViewBag.Pager = pager;
        return View(data);
    }

    // GET: Books/Create
    // Displays the form to create a new book
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Books/Create
    // Handles the creation of a new book
    [HttpPost]
    public async Task<IActionResult> Create(Book book)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Something went wrong {ex.Message}");
            }
        }
        ModelState.AddModelError(string.Empty, $"Something went wrong Invalid Model");
        return View(book);
    }

    // GET: Books/Edit/{id}
    // Displays the form to edit an existing book based on its id
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var exist = await _context.Books.Where(x => x.Id == id).FirstOrDefaultAsync();
        return View(exist);
    }

    // POST: Books/Edit
    // Handles the editing of an existing book
    [HttpPost]
    public async Task<IActionResult> Edit(Book book)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var exist = _context.Books.Where(x => x.Id == book.Id).FirstOrDefault();
                if (exist != null)
                {
                    exist.Title = book.Title;
                    exist.Author = book.Author;
                    exist.ISBN = book.ISBN;
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Something went wrong {ex.Message}");
            }
        }
        ModelState.AddModelError(string.Empty, $"Something went wrong Invalid Model");
        return View(book);
    }

    // GET: Books/Delete/{id}
    // Displays the confirmation page for deleting a book based on its id
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var exist = await _context.Books.Where(x => x.Id == id).FirstOrDefaultAsync();
        return View(exist);
    }

    // POST: Books/Delete
    // Handles the deletion of an existing book
    [HttpPost]
    public async Task<IActionResult> Delete(Book book)
    {
        try
        {
            var exist = _context.Books.FirstOrDefault(x => x.Id == book.Id);
            if (exist != null)
            {
                _context.Remove(exist);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Something went wrong {ex.Message}");
        }

        ModelState.AddModelError(string.Empty, $"Something went wrong Invalid Model");
        return View(book);
    }
}