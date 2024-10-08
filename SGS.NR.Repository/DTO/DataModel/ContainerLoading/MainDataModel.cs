﻿namespace SGS.NR.Repository.DTO.DataModel.ContainerLoading;

public record MainDataModel
{
    public MainDataModel()
    {
        SingleData = new SingleDataModel();
        CollectionData = new CollectionDataModel();
    }

    public SingleDataModel SingleData { get; set; }
    public CollectionDataModel CollectionData { get; set; }
}
