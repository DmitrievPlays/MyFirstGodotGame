public interface IElectricModule
{
    IEnumerable<IElectricModule> ConnectedModules { get; set; }

    decimal PowerDemand { get; set; }
    decimal PowerOutput { get; set; }
}
