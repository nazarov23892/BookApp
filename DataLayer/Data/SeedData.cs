using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DataLayer.DataContexts;
using DataLayer.Entities;
using Domain;
using Domain.Entities;

namespace DataLayer.Data
{
    public static class SeedData
    {
        public static void RunSeed(
            AppIdentityDbContext efDbContext, 
            RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager)
        {
            EnsureRole(
                roleManager: roleManager,
                roleName: DomainConstants.UsersRoleName).Wait();

            string userId = EnsureUser(
                userManager: userManager,
                email: "customer@example.com",
                password: "secret123",
                roleName: DomainConstants.UsersRoleName).Result;

            SeedBooks(
                efDbContext: efDbContext,
                books: GetBooks());
        }

        private static IEnumerable<Book> GetBooks()
        {
            List<Book> books = new List<Book>();

            var martin_r = new Author
            {
                Lastname = "Martin",
                Firstname = "Robert"
            };
            var evans_e = new Author
            {
                Lastname = "Evans",
                Firstname = "Eric"
            };
            var fowler_m = new Author
            {
                Lastname = "Fowler",
                Firstname = "Martin"
            };
            var troelsen_a = new Author
            {
                Lastname = "Troelsen",
                Firstname = "Andrew"
            };
            var freeman_a = new Author
            {
                Lastname = "Freeman",
                Firstname = "Adam"
            };

            books.Add(CreateBook(
                title: "ASP.NET Core in Action, Second Edition",
                price: 51.99M,
                authors: new[] { 
                    new Author
                    {
                        Firstname = "Andrew",
                        Lastname = "Lock"
                    },
                    new Author
                    {
                        Firstname = "Julie",
                        Lastname = "Brierley"
                    }
                }));

            books.Add(CreateBook(
                title: "Pro ASP.NET Core 6: Develop Cloud-Ready Web Applications Using MVC, Blazor, and Razor Pages 9th ed.",
                price: 43.49M,
                authors: new[] {
                    freeman_a
                }));

            books.Add(CreateBook(
               title: "ASP.NET Core 6 and Angular: Full-stack web development with ASP.NET 6 and Angular 13, 5th Edition",
               price: 38.99M,
               authors: new[] {
                    new Author
                    {
                        Firstname = "Valerio", 
                        Lastname = "De Sanctis"
                    }
               }));
            books.Add(CreateBook(
               title: "Apps and Services with .NET 7: Build practical projects with Blazor, .NET MAUI, gRPC, GraphQL, and other enterprise technologies",
               price: 48.99M,
               authors: new[] {
                    new Author
                    {
                        Firstname = "Mark",
                        Lastname = "Price"
                    }
               }));
            books.Add(CreateBook(
               title: "Pro C# 10 with .NET 6: Foundational Principles and Practices in Programming 11st ed. Edition",
               price: 36.49M,
               authors: new[] {
                    troelsen_a,
                    new Author
                    {
                        Firstname = "Phil",
                        Lastname = "Japikse"
                    }
               }));

            books.Add(CreateBook(
               title: "An Atypical ASP.NET Core 6 Design Patterns Guide: A SOLID adventure into architectural principles and design patterns using .NET 6 and C# 10, 2nd Edition 2nd",
               price: 32.49M,
               authors: new[] {
                    new Author
                    {
                        Firstname = "Carl-Hugo",
                        Lastname = "Marcotte"
                    },
                    new Author
                    {
                        Firstname = "Abdelhamid",
                        Lastname = "Zebdi"
                    }
               }));
            books.Add(CreateBook(
               title: "ASP.NET Core Razor Pages in Action",
               price: 44.99M,
               authors: new[] {
                    new Author
                    {
                        Firstname = "Mike",
                        Lastname = "Brind"
                    }
               }));

            books.Add(CreateBook(
               title: "Patterns of Enterprise Application Architecture 1st Edition",
               price: 52.39M,
               authors: new[] {
                    fowler_m
               }));

            books.Add(CreateBook(
               title: "Refactoring: Improving the Design of Existing Code 2nd Edition",
               price: 45.99M,
               authors: new[] {
                    fowler_m
               }));

            books.Add(CreateBook(
              title: "Clean Code: A Handbook of Agile Software Craftsmanship 1st Edition",
              price: 42.29M,
              authors: new[] {
                    martin_r
              }));

            books.Add(CreateBook(
              title: "Clean Architecture: A Craftsman's Guide to Software Structure and Design 1st Edition",
              price: 64.99M,
              authors: new[] {
                    martin_r
              }));

            books.Add(CreateBook(
              title: "Domain-Driven Design: Tackling Complexity in the Heart of Software 1st Edition",
              price: 62.49M,
              authors: new[] {
                    evans_e
              }));

            return books;
        }

        private static Book CreateBook(
            string title,
            decimal price,
            IEnumerable<Author> authors)
        {
            return new Book
            {
                Title = title,
                Price = price,
                AuthorsLink = authors.Select((a, index) => new BookAuthor
                {
                    Order = index,
                    Author = a
                })
                .ToList()
            };
        }

        private static void SeedBooks(
            AppIdentityDbContext efDbContext,
            IEnumerable<Book> books)
        {
            if (efDbContext.Books.Any())
            {
                return;
            }
            efDbContext.AddRange(books);
            efDbContext.SaveChanges();
        }

        private static async Task EnsureRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName: roleName);
            if (role != null)
            {
                return;
            }
            var result = await roleManager.CreateAsync(new IdentityRole
            {
                Name = roleName
            });
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    message: "seed role process has errors");
            }
        }

        private static async Task<string> EnsureUser(
            UserManager<AppUser> userManager,
            string email,
            string password,
            string roleName)
        {
            var user = await userManager.FindByEmailAsync(email: email);
            if (user == null)
            {
                user = new AppUser
                {
                    Email = email,
                    UserName = email
                };
                var result = await userManager.CreateAsync(
                    user: user,
                    password: password);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        message: "seed users process has errors");
                }
            }

            bool isInRole = await userManager.IsInRoleAsync(
                user: user,
                role: roleName);

            if (!isInRole)
            {
                var result = await userManager.AddToRoleAsync(
                    user: user,
                    role: roleName);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                       message: "seed users process has errors");
                }
            }
            return user.Id;
        }

    }
}
