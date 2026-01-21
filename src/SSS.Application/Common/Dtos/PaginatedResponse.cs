using Microsoft.EntityFrameworkCore;

namespace LecX.Application.Common.Dtos
{
    public sealed class PaginatedResponse<T>
    {
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
        public List<T> Items { get; private set; } = new();

        // public ctor để tạo kết quả đã map sẵn Items
        public PaginatedResponse(int pageIndex, int pageSize, int totalCount, List<T> items)
        {
            PageIndex = pageIndex < 1 ? 1 : pageIndex;
            PageSize = pageSize <= 0 ? 10 : pageSize;
            TotalCount = totalCount < 0 ? 0 : totalCount;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            Items = items ?? new();
        }

        // factory tạo từ IQueryable<T>
        public static async Task<PaginatedResponse<T>> CreateAsync(
            IQueryable<T> source, int pageIndex, int pageSize, CancellationToken ct = default)
        {
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            var count = await source.CountAsync(ct);
            var items = await source.Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync(ct);

            return new PaginatedResponse<T>(pageIndex, pageSize, count, items);
        }

        // đổi Items sang kiểu khác nhưng giữ metadata
        public PaginatedResponse<TOut> MapItems<TOut>(Func<T, TOut> map)
            => new(PageIndex, PageSize, TotalCount, Items.Select(map).ToList());
    }
}
