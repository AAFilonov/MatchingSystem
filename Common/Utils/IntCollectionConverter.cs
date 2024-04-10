using Newtonsoft.Json;

namespace Common.Utils;
class IntCollectionConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        // Проверяем, является ли тип массивом целых чисел или списком целых чисел
        return objectType == typeof(int[]) ||  
               objectType.IsGenericType &&  
               objectType.GetGenericTypeDefinition() == typeof(List<>) &&  
               objectType.GetGenericArguments()[0] == typeof(int);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException(); // Этот метод не нужен для вашей задачи
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        // Сохраняем текущие настройки форматирования
        var originalFormatting = writer.Formatting;

        // Устанавливаем Formatting.None для избежания автоматического добавления переносов
        writer.Formatting = Formatting.None;
        
        // Определяем, является ли значение массивом или списком
        writer.WriteRaw(" ");
        writer.WriteStartArray();
        // Получаем коллекцию
        var collection = value as IEnumerable<int>;
        if (collection == null)
        {
            throw new JsonSerializationException("Expected List<int> or int[]");
        }
        // Сериализуем каждый элемент без добавления переносов
        foreach (var item in collection)
        {
            writer.WriteValue(item);
        }
        // Заканчиваем массив без добавления переноса
        writer.WriteEndArray();
        
        // Восстанавливаем оригинальные настройки форматирования
        writer.Formatting = originalFormatting;
    }
}