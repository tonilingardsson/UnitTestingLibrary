using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTestingLibrary;

namespace UnitTestingLibrary.Tests
{
    [TestClass]
    public class LibrarySystemTests
    {
        [TestMethod]
        public void AddBook_ShouldAddBook_WhenISBNIsUnique()
        {
            // Arrange
            var library = new LibrarySystem();
            var book = new Book("Clean Code", "Robert C. Martin", "1111111111", 2008);

            // Act
            var result = library.AddBook(book);

            // Assert 
            Assert.IsTrue(result);
            Assert.IsNotNull(library.SearchByISBN("1111111111"));
        }
    }
}