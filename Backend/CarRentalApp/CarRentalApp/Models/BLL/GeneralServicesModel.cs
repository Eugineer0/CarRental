namespace CarRentalApp.Models.BLL;

public class GeneralServicesModel
{
    public string Name { get; set; }

    public IReadOnlyDictionary<CarTypeModel, decimal> Prices { get; set; }
}