// See https://aka.ms/new-console-template for more information
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

Console.WriteLine("Hello, World!");

/*
「ツール」→「NuGetパッケージマネージャー」→「パッケージマネージャーコンソール」でコンソールを開き、以下のコマンドを実行

    Install-Package Microsoft.EntityFrameworkCore.Tools
    Add-Migration InitialCreate
    Update-Database
 */


EFTest.Create();
var item = EFTest.Read(1);
EFTest.Update();
EFTest.Delete();


[Table("Item")]
public class Item
{
    [Key]
    [Required]
    public uint Id { get; set; } 
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

public class EFTest
{

    public static void Create()
    {
        using var db = new ItemDbContext();
        db.Add(new Item() { Id = 1, Name = "Hello" });
        db.Add(new Item() { Id = 2, Name = "World" });
        db.SaveChanges();
    }

    public static Item Read(int id)
    {
        using var db = new ItemDbContext();
        return db.Items.First(b => b.Id == id);
    }

    public static void Update()
    {
        using var db = new ItemDbContext();
        var item = db.Items.First(b => b.Id == 1);
        item.Name = "Foo";
        db.Update(item);
        //db.SaveChanges();
    }

    public static void Delete()
    {
        using var db = new ItemDbContext();
        var item = db.Items.First(b => b.Id == 1);
        db.Remove(item);
        db.SaveChanges();
    }
}
