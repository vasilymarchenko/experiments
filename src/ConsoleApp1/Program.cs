// See https://aka.ms/new-console-template for more information
using ConsoleApp1;


MyModel model1 = new MyModel()
{
    Id = "some-id",
    Name = "Name",
    Settings = new Dictionary<string, string>()
    {
        { "setting1", "value1" },
        { "setting2", "value2" }
    }
};

var jsonString = model1.ToJson();
Console.WriteLine(jsonString);

MyModel deserializedModel = MyModel.FromJson(jsonString);

Console.WriteLine($"Id: {deserializedModel.Id}");
Console.WriteLine($"Name: {deserializedModel.Name}");

foreach (var setting in deserializedModel.Settings)
{
    Console.WriteLine($"{setting.Key}: {setting.Value}");
}
