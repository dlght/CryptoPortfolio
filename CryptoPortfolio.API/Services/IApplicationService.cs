using CryptoPortfolio.API.DTO;

namespace CryptoPortfolio.API.Services
{
    public interface IApplicationService
    {
        Task<long> ImportPortfolio(FileUploadDTO fileUpload);

        Task<PortfolioDTO?> RefreshPortfolio(long portfolioId);
    }
}
