﻿#nullable disable

namespace SGS.NR.Repository.DTO.Condition;

public record ContainerLoadingCondition
{
    public string FilePath { get; set; }
    public string SheetName { get; set; } = "草稿";
}
