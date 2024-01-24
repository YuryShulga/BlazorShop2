using NetFrameworkTest.Models;

using (ApplicationContext db = new ApplicationContext())
{
    // создаем два объекта User
    User tom = new User { Name = "Tom", Age = 35 };
    User alice = new User { Name = "Alice", Age = 233 };

    // добавляем их в бд
    db.Users.Add(tom);
    db.Users.Add(alice);
    db.SaveChanges();
    Console.WriteLine("Объекты успешно сохранены");

    // получаем объекты из бд и выводим на консоль
    var users = db.Users.ToList();
    Console.WriteLine("Список объектов:");
    foreach (User u in users)
    {
        Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
    }
}