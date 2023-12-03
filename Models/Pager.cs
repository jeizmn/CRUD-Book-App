using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDApp.Models
{
    public class Pager
    {
        //For handling pagination information
        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }

        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }

        // Default constructor
        public Pager()
        { }

        // Constructor to initialize pager properties based on total items, current page, and page size
        public Pager(int totalItems, int page, int pageSize = 10)
        {
            // Calculate total pages based on total items and page size
            int totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            int currentPage = page;

            // Calculate start and end pages in the pagination range
            int startPage = currentPage - 5;
            int endPage = currentPage + 4;

            // Adjust start and end pages to stay within valid page range
            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }

            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }
            // Set pager properties
            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
        }
    }
}