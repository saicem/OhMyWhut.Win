using System.Text.Json.Serialization;
using OhMyWhut.Engine.Extentions;

namespace OhMyWhut.Engine.Responses
{
    public class BookResponse
    {
        [JsonPropertyName("pageNum")]
        public long PageNum { get; set; }

        [JsonPropertyName("pageSize")]
        public long PageSize { get; set; }

        [JsonPropertyName("size")]
        public long Size { get; set; }

        [JsonPropertyName("orderBy")]
        public object? OrderBy { get; set; }

        [JsonPropertyName("startRow")]
        public long StartRow { get; set; }

        [JsonPropertyName("endRow")]
        public long EndRow { get; set; }

        [JsonPropertyName("total")]
        public long Total { get; set; }

        [JsonPropertyName("pages")]
        public long Pages { get; set; }

        [JsonPropertyName("list")]
        public BookItem[]? BookList { get; set; }

        [JsonPropertyName("firstPage")]
        public long FirstPage { get; set; }

        [JsonPropertyName("prePage")]
        public long PrePage { get; set; }

        [JsonPropertyName("nextPage")]
        public long NextPage { get; set; }

        [JsonPropertyName("lastPage")]
        public long LastPage { get; set; }

        [JsonPropertyName("isFirstPage")]
        public bool IsFirstPage { get; set; }

        [JsonPropertyName("isLastPage")]
        public bool IsLastPage { get; set; }

        [JsonPropertyName("hasPreviousPage")]
        public bool HasPreviousPage { get; set; }

        [JsonPropertyName("hasNextPage")]
        public bool HasNextPage { get; set; }

        [JsonPropertyName("navigatePages")]
        public long NavigatePages { get; set; }

        [JsonPropertyName("navigatepageNums")]
        public long[]? NavigatepageNums { get; set; }
    }

    public class BookItem
    {
        [JsonPropertyName("SYTS")]
        public long Syts { get; set; }

        [JsonPropertyName("DQRQ"), JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly Dqrq { get; set; }

        [JsonPropertyName("ROW_ID")]
        public long RowId { get; set; }

        [JsonPropertyName("XGH")]
        public string Xgh { get; set; } = null!;

        [JsonPropertyName("TSBH")]
        public string Tsbh { get; set; } = null!;

        [JsonPropertyName("ZBT")]
        public string Zbt { get; set; } = null!;

        [JsonPropertyName("TSJYYGHBH")]
        public string? Tsjyyghbh { get; set; }

        [JsonPropertyName("JYRQ"), JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly Jyrq { get; set; }

        [JsonPropertyName("JYR")]
        public string Jyr { get; set; } = null!;
    }
}