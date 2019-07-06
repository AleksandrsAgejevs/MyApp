using MyLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLibrary.Data
{
    public static class DbInitializer
    {
        public static void Initialize(LibraryContext context)
        {
            context.Database.EnsureCreated();

            if (context.Members.Any())
            {
                return;
            }

            var members = new Member[]
            {
            new Member{FirstName="Max",LastName="Muller"},
            new Member{FirstName="Bob",LastName="Braun"},
            new Member{FirstName="Brendon",LastName="Markson"},
            new Member{FirstName="Carson",LastName="Alexander"},
            new Member{FirstName="Meredith",LastName="Alonso"},
            new Member{FirstName="Arturo",LastName="Anand"},
            new Member{FirstName="Gytis",LastName="Barzdukas"}
            };
            foreach (Member m in members)
            {
                context.Members.Add(m);
            }
            context.SaveChanges();


            var authors = new Author[]
            {
            new Author{FullName="Jules Verne"},
            new Author{FullName="Leo Tolstoy"},
            new Author{FullName="Charlotte Bronte"},
            new Author{FullName="Herman Melville"},
            new Author{FullName="Marcel Proust"}
            };

            foreach (Author a in authors)
            {
                context.Authors.Add(a);
            }
            context.SaveChanges();


            var books = new Book[]
            {
            new Book
            {
                Title ="The Mysterious Island",
                Genre = Genre.Adventure,
                PublicationYear =1874,
                AuthorID = authors.Single(b => b.FullName == "Jules Verne" ).AuthorID
            },

            new Book
            {
                Title ="War and Peace",
                Genre =Genre.Novel,
                PublicationYear =1869,
                AuthorID = authors.Single(b => b.FullName == "Leo Tolstoy" ).AuthorID
            },
            new Book
            {
                Title ="Jane Eyre",
                Genre = Genre.Novel,
                PublicationYear =1847,
                AuthorID = authors.Single(b => b.FullName == "Charlotte Bronte" ).AuthorID
            },
            new Book
            {
                Title ="Moby-Dick",
                Genre = Genre.Adventure,
                PublicationYear =1874,
                AuthorID = authors.Single(b => b.FullName == "Herman Melville" ).AuthorID
            },
            new Book
            {
                Title ="In Search of Lost Time",
                Genre = Genre.Modernism,
                PublicationYear =1927,
                AuthorID = authors.Single(b => b.FullName == "Marcel Proust" ).AuthorID
            },
            new Book
            {
                Title ="Twenty Thousand Leagues Under the Sea",
                Genre = Genre.Adventure,
                PublicationYear =1870,
                AuthorID = authors.Single(b => b.FullName == "Jules Verne" ).AuthorID
            },
            new Book{Title="Mardi",
                Genre = Genre.Romance,
                PublicationYear =1849,
                AuthorID = authors.Single(b => b.FullName == "Herman Melville" ).AuthorID
            },
            new Book
            {
                Title ="Journey to the Center of the Earth",
                Genre = Genre.Adventure,
                PublicationYear =1864,
                AuthorID = authors.Single(b => b.FullName == "Jules Verne" ).AuthorID
            },
            new Book
            {
                Title ="Around the World in Eighty Days",
                Genre = Genre.Adventure,
                PublicationYear =1873,
                AuthorID = authors.Single(b => b.FullName == "Jules Verne" ).AuthorID
            }
            };
            foreach (Book b in books)
            {
                context.Books.Add(b);
            }
            context.SaveChanges();

            var rentals = new Rental[]
            {
                new Rental
                {
                    MemberID = members.Single(c => c.LastName == "Muller").ID,
                    BookID = books.Single(p => p.Title == "Journey to the Center of the Earth" ).BookID,
                    RentalDate = DateTime.Parse("2019-02-01"),
                },
                new Rental
                {
                    MemberID = members.Single(c => c.LastName == "Muller").ID,
                    BookID = books.Single(p => p.Title == "The Mysterious Island" ).BookID,
                    RentalDate = DateTime.Parse("2019-02-02"),
                },

                new Rental
                    {
                        MemberID = members.Single(c => c.LastName == "Braun").ID,
                        BookID = books.Single(p => p.Title == "War and Peace" ).BookID,
                        RentalDate = DateTime.Parse("2019-02-03"),
                    },
                new Rental
                    {
                        MemberID = members.Single(c => c.LastName == "Braun").ID,
                        BookID = books.Single(p => p.Title == "Around the World in Eighty Days" ).BookID,
                        RentalDate = DateTime.Parse("2019-02-04"),
                    },

                new Rental
                    {
                        MemberID = members.Single(c => c.LastName == "Markson").ID,
                        BookID = books.Single(p => p.Title == "Jane Eyre" ).BookID,
                        RentalDate = DateTime.Parse("2019-02-05"),
                    },

                new Rental
                    {
                        MemberID = members.Single(c => c.LastName == "Alexander").ID,
                        BookID = books.Single(p => p.Title == "Moby-Dick" ).BookID,
                        RentalDate = DateTime.Parse("2019-02-06"),
                    },

                new Rental
                    {
                        MemberID = members.Single(c => c.LastName == "Alonso").ID,
                        BookID = books.Single(p => p.Title == "In Search of Lost Time" ).BookID,
                        RentalDate = DateTime.Parse("2019-02-07"),
                    },

                new Rental
                    {
                        MemberID = members.Single(c => c.LastName == "Anand").ID,
                        BookID = books.Single(p => p.Title == "Twenty Thousand Leagues Under the Sea" ).BookID,
                        RentalDate = DateTime.Parse("2019-02-08"),
                    },

                new Rental
                    {
                        MemberID = members.Single(c => c.LastName == "Barzdukas").ID,
                        BookID = books.Single(p => p.Title == "Mardi" ).BookID,
                        RentalDate = DateTime.Parse("2019-02-09"),
                    }
            };
            foreach (Rental r in rentals)
            {
                context.Rentals.Add(r);
            }
            context.SaveChanges();


        }
    }
}