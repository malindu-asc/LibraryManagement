using Library.Api.Contracts.Borrowings;

namespace Library.Api.Application.Interfaces;

public interface IBorrowingService
{
    Task<BorrowingResponse> BorrowAsync(BorrowRequest request);
    Task<List<BorrowingResponse>> GetAllAsync();
    Task<List<BorrowingResponse>> GetByMemberAsync(Guid memberId);
    Task<BorrowingResponse> ReturnAsync(Guid borrowingId);
}
