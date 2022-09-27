// See https://aka.ms/new-console-template for more information
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

Console.WriteLine("Hello, World!");

var controller = new Controller();

//Create
controller.Create(new Item { Id = 1, Name = "Hello" });
controller.Create(new Item { Id = 2, Name = "World" });

//Read
var item1 = controller.Read(1);
Console.WriteLine($"after:{item1.Name}");

//Update
item1.Name = "Foo";
controller.Update();

var item2 = controller.Read(1);
Console.WriteLine($"after:{item2.Name}");

//Delete
controller.Delete(item2);

try
{
    controller.Read(1);
}
catch
{
    Console.WriteLine($"Not found");
}

[Table("Item")]
public class Item
{
    [Key]
    [Required]
    public uint Id { get; set; } 
    [Required]
    public string Name { get; set; }
}

public class ItemDbContext : DbContext
{
    public DbSet<Item> Items { get; internal set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        var connectionString = new SqliteConnectionStringBuilder { DataSource = "c:\\tool\\db.sqlite3"}.ToString();
        optionsBuilder.UseSqlite(new SqliteConnection(connectionString));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Item>().HasKey(c => new { c.Id, c.Name });
    }
}

public class Controller : IDisposable
{
    readonly ItemDbContext DbContext;

    public Controller()
    {
        DbContext = new ItemDbContext();
    }

    public void Create(Item newItem)
    {
        DbContext.Add(newItem);
        DbContext.SaveChanges();
    }

    public Item Read(int id)
    {
        return DbContext.Items.First(b => b.Id == id);
    }

    public void Update()
    {
        DbContext.SaveChanges();
    }

    public void Delete(Item item)
    {
        DbContext.Remove(item);
        DbContext.SaveChanges();
    }

    public void Dispose()
    {
        DbContext?.Dispose();
    }
}
