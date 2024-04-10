namespace SGS.NR.Service.DTO.ResultModel
{
    public record BaseResultModel
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }
}
