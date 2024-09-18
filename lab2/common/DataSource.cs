using Bogus;
using lab2.models;

namespace lab2.common;

public static class DataSource
{
    private static IList<Book> _listBooks { get; set; }

    public static IList<Book> GetBooks()
    {
        if (_listBooks != null)
        {
            return _listBooks;
        }

        // Initialize Faker for generating Press
        var pressFaker = new Faker<Press>()
            .RuleFor(p => p.Id, f => f.IndexFaker + 1)
            .RuleFor(p => p.Name, f => f.Company.CompanyName())
            .RuleFor(p => p.Category, f => f.PickRandom<Category>());

        // Initialize Faker for generating Address
        var addressFaker = new Faker<Address>()
            .RuleFor(a => a.City, f => f.Address.City())
            .RuleFor(a => a.Street, f => f.Address.StreetName());

        // Initialize Faker for generating Book
        var bookFaker = new Faker<Book>()
            .RuleFor(b => b.Id, f => f.IndexFaker + 1)
            .RuleFor(b => b.ISBN, f => f.Commerce.Ean13())
            .RuleFor(b => b.Title, f => f.Lorem.Sentence(3))
            .RuleFor(b => b.Author, f => f.Name.FullName())
            .RuleFor(b => b.Price, f => f.Finance.Amount(10, 100))
            .RuleFor(b => b.Location, f => addressFaker.Generate())
            .RuleFor(b => b.Press, f => pressFaker.Generate());

        // Generate 40 sample books
        _listBooks = bookFaker.Generate(100);
        
        return _listBooks;
    }
}