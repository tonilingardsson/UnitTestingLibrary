using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTestingLibrary;
using System;

namespace UnitTestingLibrary.Tests
{
    [TestClass]
    public class LibrarySystemTests
    {
        [TestMethod]
        public void AddBook_ShouldAddBook_WhenISBNIsUnique()
        {
            var library = new LibrarySystem();
            var book = new Book("Clean Code", "Robert C. Martin", "1111111111", 2008);

            var result = library.AddBook(book);

            Assert.IsTrue(result);
            Assert.IsNotNull(library.SearchByISBN("1111111111"));
        }

        [TestMethod]
        public void AddBook_ShouldFail_WhenISBNAlreadyExists()
        {
            var library = new LibrarySystem();
            var book = new Book("Another 1984", "Someone", "9780451524935", 2020);

            var result = library.AddBook(book);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AddBook_ShouldFail_WhenISBNIsEmpty()
        {
            var library = new LibrarySystem();
            var book = new Book("Test", "Author", "", 2024);

            var result = library.AddBook(book);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RemoveBook_ShouldRemoveBook_WhenBookExists_AndIsNotBorrowed()
        {
            var library = new LibrarySystem();

            var result = library.RemoveBook("9780451524935");

            Assert.IsTrue(result);
            Assert.IsNull(library.SearchByISBN("9780451524935"));
        }

        [TestMethod]
        public void RemoveBook_ShouldFail_WhenBookIsBorrowed()
        {
            var library = new LibrarySystem();
            library.BorrowBook("9780451524935");

            var result = library.RemoveBook("9780451524935");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SearchByTitle_ShouldBeCaseInsensitive_AndAllowPartialMatches()
        {
            var library = new LibrarySystem();

            var results = library.SearchByTitle("hob");

            Assert.HasCount(1, results);
            Assert.AreEqual("The Hobbit", results[0].Title);
        }

        [TestMethod]
        public void SearchByAuthor_ShouldBeCaseInsensitive_AndAllowPartialMatches()
        {
            var library = new LibrarySystem();

            var results = library.SearchByAuthor("tolkien");

            Assert.HasCount(1, results);
            Assert.AreEqual("The Hobbit", results[0].Title);
        }

        [TestMethod]
        public void SearchByISBN_ShouldFindBook_WhenISBNMatches()
        {
            var library = new LibrarySystem();

            var result = library.SearchByISBN("9780451524935");

            Assert.IsNotNull(result);
            Assert.AreEqual("1984", result.Title);
        }

        [TestMethod]
        public void BorrowBook_ShouldMarkBookAsBorrowed_AndSetBorrowDate()
        {
            var library = new LibrarySystem();

            var result = library.BorrowBook("9780451524935");
            var book = library.SearchByISBN("9780451524935");

            Assert.IsTrue(result);
            Assert.IsTrue(book?.IsBorrowed);
            Assert.IsNotNull(book?.BorrowDate);
        }

        [TestMethod]
        public void BorrowBook_ShouldFail_WhenBookIsAlreadyBorrowed()
        {
            var library = new LibrarySystem();
            library.BorrowBook("9780451524935");

            var result = library.BorrowBook("9780451524935");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ReturnBook_ShouldResetBorrowStatus_AndClearBorrowDate()
        {
            var library = new LibrarySystem();
            library.BorrowBook("9780451524935");

            var result = library.ReturnBook("9780451524935");
            var book = library.SearchByISBN("9780451524935");

            Assert.IsTrue(result);
            Assert.IsFalse(book?.IsBorrowed);
            Assert.IsNull(book?.BorrowDate);
        }

        [TestMethod]
        public void ReturnBook_ShouldFail_WhenBookIsNotBorrowed()
        {
            var library = new LibrarySystem();

            var result = library.ReturnBook("9780451524935");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CalculateLateFee_ShouldReturnZero_WhenDaysLateIsZero()
        {
            var library = new LibrarySystem();

            var fee = library.CalculateLateFee("9780451524935", 0);

            Assert.AreEqual(0m, fee);
        }

        [TestMethod]
        public void CalculateLateFee_ShouldReturnZero_WhenBookDoesNotExist()
        {
            var library = new LibrarySystem();

            var fee = library.CalculateLateFee("does-not-exist", 5);

            Assert.AreEqual(0m, fee);
        }

        [TestMethod]
        public void CalculateLateFee_ShouldMultiplyFeePerDay_ByDaysLate()
        {
            var library = new LibrarySystem();

            var fee = library.CalculateLateFee("9780451524935", 4);

            Assert.AreEqual(2.0m, fee);
        }

        [TestMethod]
        public void IsBookOverdue_ShouldReturnTrue_WhenBorrowedLongerThanLoanPeriod()
        {
            var library = new LibrarySystem();
            library.BorrowBook("9780451524935");

            var book = library.SearchByISBN("9780451524935");

            Assert.IsNotNull(book);
            Assert.IsTrue(book.IsBorrowed);
            Assert.IsNotNull(book.BorrowDate);
            book.BorrowDate = DateTime.Now.AddDays(-15);

            var result = library.IsBookOverdue("9780451524935", 14);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsBookOverdue_ShouldReturnFalse_WhenBookIsNotBorrowed()
        {
            var library = new LibrarySystem();

            var result = library.IsBookOverdue("9780451524935", 14);

            Assert.IsFalse(result);
        }
    }
}