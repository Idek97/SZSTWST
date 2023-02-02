using System;

namespace SZSTWST_Shared.SqlTable
{
    public class UserTable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string? PrivateEmail { get; set; }
        public string CollegeEmail { get; set; }
        public int CollegeIndex { get; set; }
        public bool IsAdmin { get; set; }
        public string Password { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class AssignmentTable
    {
        public int Id { get; set; }
        public int IdCreatorUser { get; set; }
        public int IdUser { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StopDate { get; set; }
        public bool IsFinished { get; set; }
        public DateTime? FinishDate { get; set; }
        public int? IdFinishedByUser { get; set; }
    }

    public class AssignmentAssetTable
    {
        public int Id { get; set; }
        public int IdAssignment { get; set; }
        public int IdAsset { get; set; }
    }

    public class AssetTable
    {
        public AssetTable() { }
        public AssetTable(int id, string codeName, string serialNumber, string name, string description, bool isActive, DateTime creationDate)
        {
            Id = id;
            CodeName = codeName;
            SerialNumber = serialNumber;
            Name = name;
            Description = description;
            IsActive = isActive;
            CreationDate = creationDate;
        }

        public int Id { get; set; }
        public string CodeName { get; set; }
        public string? SerialNumber { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }
    }

}