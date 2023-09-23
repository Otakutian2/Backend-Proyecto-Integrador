using System.Runtime.Serialization;

namespace project_backend.Enums
{
    public enum CommandStateEnum
    {
        Generated = 1,
        Prepared = 2,
        Paid = 3,
    }

    public enum TableStateEnum
    {
        [EnumMember(Value = "Ocupado")]
        Occupied,
        [EnumMember(Value = "Libre")]
        Free,
    }
}
