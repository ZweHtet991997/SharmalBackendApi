using SharmalRealEstateSystem.Shared.Configs;

namespace SharmalRealEstateSystem.Shared.Services.FtpServices;

public class FtpService
{
    private readonly string _userName = string.Empty;
    private readonly string _password = string.Empty;
    private readonly AsyncFtpClient _ftp;
    private readonly ILogger<FtpService> _logger;

    public FtpService(ILogger<FtpService> logger)
    {
        //_userName = Environment.GetEnvironmentVariable("Sharmal_FTP_UserName")!;
        //_password = Environment.GetEnvironmentVariable("Sharmal_FTP_Password")!;
        ////_userName = "sharmaluatresource";
        ////_password = "NKsoftwarehouse*11";
        ///
        if (Deployment.IsDevelopment())
        {
            _ftp = new AsyncFtpClient
            {
                Host = FTPConfig.UATHost,
                Credentials = new NetworkCredential(FTPConfig.UATUserName, FTPConfig.UATPassword)
            };
        }
        else
        {
            _ftp = new AsyncFtpClient
            {
                Host = FTPConfig.Host,
                Credentials = new NetworkCredential(FTPConfig.UserName, FTPConfig.Password)
            };
        }

        _logger = logger;
    }

    #region Check Directory Exists Async

    public async Task<bool> CheckDirectoryExistsAsync(string directory)
    {
        try
        {
            var token = new CancellationToken();
            await _ftp.Connect(token);

            return await _ftp.DirectoryExists(directory, token);
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Error in Checking directory: {ex.Message}");
            throw;
        }
    }

    #endregion

    #region Create Directory Async

    public async Task<bool> CreateDirectoryAsync(string directory)
    {
        try
        {
            var token = new CancellationToken();
            await _ftp.Connect(token);

            return await _ftp.CreateDirectory(directory, true, token);
        }
        catch (Exception ex)
        {
            if (ex.InnerException is not null)
            {
                _logger.LogInformation($"Create Directory Error: {ex.InnerException}");
            }

            _logger.LogInformation($"Create Directory Error: {ex.ToString()}");
            throw;
        }
    }

    #endregion

    #region Upload File Async

    public async Task UploadFileAsync(IFormFile file, string directory, string fileName)
    {
        var tempFilePath = Path.GetTempFileName();
        _logger.LogInformation($"Upload File Temp File Path: {tempFilePath}");
        try
        {
            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var token = new CancellationToken();
            await _ftp.Connect(token);
            _logger.LogInformation("Connected to FTP server.");

            var remoteFilePath = Path.Combine(directory, fileName).Replace("\\", "/");
            _logger.LogInformation($"Remote File Path: {remoteFilePath}");

            var success = await _ftp.UploadFile(tempFilePath, remoteFilePath, token: token);
            _logger.LogInformation("File Uploaded Successfully!");
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Upload File Exception: {ex.Message}");
            if (ex.InnerException != null)
            {
                _logger.LogInformation($"Upload File Inner Exception: {ex.InnerException.Message}");
            }

            throw;
        }
        finally
        {
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
                _logger.LogInformation("Temporary file deleted.");
            }
        }
    }

    #endregion

    #region Delete File Async

    public async Task DeleteFileAsync(string filePath)
    {
        try
        {
            var token = new CancellationToken();
            await _ftp.Connect(token);

            await _ftp.DeleteFile(filePath);
            _logger.LogInformation("File Deleted Successfully!");
        }
        catch (Exception ex)
        {
            if (ex.InnerException is not null)
            {
                _logger.LogInformation($"Delete File Exception: {ex.InnerException.Message}");
            }
            _logger.LogInformation(ex.Message);

            throw;
        }
    }

    #endregion
}
