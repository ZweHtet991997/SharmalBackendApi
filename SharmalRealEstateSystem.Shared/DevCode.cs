namespace DotNet7.SharmalRealEstateSample.Shared;

public static class DevCode
{
    public static async Task<string> ReadRequestBodyAsync(this HttpContext context)
    {
        context.Request.EnableBuffering();

        using (
            var reader = new StreamReader(
                context.Request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 1024,
                leaveOpen: true
            )
        )
        {
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0; // Reset the stream position for the next read
            return body;
        }
    }

    public static async Task<Dictionary<string, string>> ReadFormDataAsync(this HttpContext context)
    {
        context.Request.EnableBuffering();

        // Read the form asynchronously
        var form = await context.Request.ReadFormAsync();

        // Extract form fields into a dictionary
        var formData = form.ToDictionary(x => x.Key, x => x.Value.ToString());

        // Reset the stream position for the next read
        context.Request.Body.Position = 0;

        return formData;
    }

    public static string DatabaseConnectionString()
    {
        var DBConnection = Environment.GetEnvironmentVariable("Sharmal_DBConnection");
        var Database = Environment.GetEnvironmentVariable("Sharmal_Database");
        var UserId = Environment.GetEnvironmentVariable("Sharmal_UserID");
        var Password = Environment.GetEnvironmentVariable("Sharmal_Password");
        return $"Data Source={DBConnection};Database={Database};User ID={UserId};Password={Password};TrustServerCertificate=True;";
    }

    public static string ConvertFileToBase64(this string fileName, string basePath)
    {
        string path = Path.Combine(basePath + @"\img\exchange_rate", fileName + ".png");

        if (!File.Exists(path))
        {
            throw new FileNotFoundException("File not found.", path);
        }

        byte[] fileBytes = File.ReadAllBytes(path);

        string base64String = Convert.ToBase64String(fileBytes);

        return base64String;
    }

    public static string SerializeObject(this object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    public static T DeserializeObject<T>(this string str)
    {
        return JsonConvert.DeserializeObject<T>(str)!;
    }

    public static int ToInt32(this string str)
    {
        return Convert.ToInt32(str);
    }

    public static bool IsNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
    }

    public static string GetFileName(this IFormFile file)
    {
        return $"{DateTimeOffset.Now.ToUnixTimeMilliseconds()}_" + file.FileName;
    }

    public static string GetFileNameV1(this IFormFile file)
    {
        TimeZoneInfo myanmarTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Myanmar Standard Time");
        DateTime localDateTime = DateTime.Now;
        DateTime myanmarDateTime = TimeZoneInfo.ConvertTime(
            localDateTime,
            TimeZoneInfo.Local,
            myanmarTimeZone
        );
        long unixTimeMilliseconds = new DateTimeOffset(myanmarDateTime).ToUnixTimeMilliseconds();
        return $"{unixTimeMilliseconds}_" + file.FileName;
    }

    public static IQueryable<TSource> Pagination<TSource>(
        this IQueryable<TSource> source,
        int pageNo,
        int pageSize
    )
    {
        return source.Skip((pageNo - 1) * pageSize).Take(pageSize);
    }

    public static string GetCurrentMyanmarDateTime()
    {
        var myanmarTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Myanmar Standard Time");
        var myanmarDateTime = TimeZoneInfo.ConvertTime(
            DateTime.Now,
            TimeZoneInfo.Local,
            myanmarTimeZone
        );

        return myanmarDateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
