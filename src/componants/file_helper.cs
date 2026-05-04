public static class FileHelper
{
    // Image Save karne ke liye
public static async Task<string?> SaveImage(IFormFile? file, string folderName)
{
    if (file == null || file.Length == 0) return null;

    // 1. Root path aur folder ka path set karein
    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);

    // 2. CHECK: Agar folder nahi hai to bana do
    if (!Directory.Exists(folderPath))
    {
        Directory.CreateDirectory(folderPath);
    }

    // 3. Unique name aur full path
    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
    var fullPath = Path.Combine(folderPath, fileName);

    // 4. File save karein
    using (var stream = new FileStream(fullPath, FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }

    return fileName; 
}



//     public static string GetFullImagePath(HttpRequest? request, string folderName, string? fileName)
// {
//     if (string.IsNullOrEmpty(fileName)) return "";

//     // Agar request sahi mil rahi hai to full URL banao
//     if (request != null && !string.IsNullOrEmpty(request.Scheme) && request.Host.HasValue)
//     {
//         return $"{request.Scheme}://{request.Host}/{folderName}/{fileName}";
//     }

//     // Agar request nahi mil rahi (e.g. background service ya testing), to simple path bhej do
//     return $"/{folderName}/{fileName}"; 
// }

public static string GetFullImagePath(HttpRequest? request, string folderName, string? fileName)
{
    if (string.IsNullOrEmpty(fileName)) return "";

    // 1. Koshish karein ke Request se domain mil jaye
    var scheme = request?.Scheme ?? "http";
    var host = request?.Host.Value;

    // 2. Agar Host nahi mil raha (Render/Docker ka masla), to URL manually build karein
    if (string.IsNullOrEmpty(host))
    {
        // Agar aap localhost par hain to ye chalega, 
        // Agar live hain to yahan apna domain name bhi likh sakte hain
        return $"/profile_images/{fileName}"; 
    }

    // 3. Complete URL return karein
    return $"{scheme}://{host}/{folderName}/{fileName}";
}


}